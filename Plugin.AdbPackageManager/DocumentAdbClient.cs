using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Plugin.AdbPackageManager.Adb;
using Plugin.AdbPackageManager.Plugins;
using Plugin.AdbPackageManager.Properties;
using Plugin.AdbPackageManager.UI;
using Plugin.AdbPackageManager.UI.Async;
using SAL.Windows;

namespace Plugin.AdbPackageManager
{
	public partial class DocumentAdbClient : UserControl
	{
		private const String Caption = "ADB Client";
		private static readonly Color HiddenPackageColor = Color.Gray;
		private AdbClient _client;
		private ApkImageViewPlugin _apkViewPlugin;
		private PluginWindows Plugin => (PluginWindows)this.Window.Plugin;

		private IWindow Window => (IWindow)base.Parent;
		private ApkImageViewPlugin ApkViewPlugin
			=> this._apkViewPlugin ?? (this._apkViewPlugin = new ApkImageViewPlugin(this.Plugin.HostWindows));

		private AdbClient Client
			=> this._client ?? (this._client = new AdbClient(this.Plugin.Settings.AdbPath));

		private AdbDevice SelectedDevice => (AdbDevice)tsddlDevices.SelectedItem;

		public DocumentAdbClient()
		{
			this.InitializeComponent();
			splitMain.Panel2Collapsed = true;
			gridSearch.ListView = lvInstalled;
		}

		protected override void OnCreateControl()
		{
			this.Window.Caption = DocumentAdbClient.Caption;
			this.Window.SetTabPicture(Resources.Icon);

			// Update buttons when settings change
			this.Settings_PropertyChanged(this, new PropertyChangedEventArgs(nameof(PluginSettings.AdbPath)));
			this.Plugin.Settings.PropertyChanged += this.Settings_PropertyChanged;
			this.Window.Closed += new EventHandler(this.Window_Closed);

			foreach(AdbAppInfo.AdbAppType type in Enum.GetValues(typeof(AdbAppInfo.AdbAppType)))
				lvInstalled.Groups.Add(type.ToString(), type.ToString());

			base.OnCreateControl();
		}

		private void Settings_PropertyChanged(Object sender, PropertyChangedEventArgs e)
		{
			switch(e.PropertyName)
			{
			case nameof(PluginSettings.AdbPath):
				if(File.Exists(this.Plugin.Settings.AdbPath))
				{
					tsddlDevices.Enabled = true;
					tsbnOpenAdb.Visible = false;
				} else
				{
					tsddlDevices.Enabled = false;
					tsddlDevices.Items.Clear();
					tsbnOpenAdb.Visible = true;
				}
				break;
			}
		}

		private void Window_Closed(Object sender, EventArgs e)
		{
			this.Plugin.Settings.PropertyChanged -= this.Settings_PropertyChanged;

			if(this._apkViewPlugin != null && this._apkViewPlugin.IsPluginLoaded)
				foreach(String filePath in this.Plugin.Settings.GetApkTempFiles(true))
					this.ApkViewPlugin.CloseApkFile(filePath);
		}

		/// <summary>Delete all data after changing the device</summary>
		private void ClearDeviceData()
		{
			this.Window.Caption = DocumentAdbClient.Caption;
			lvInstalled.Items.Clear();
			tsbnDeviceInstall.Enabled = tsbnDeviceProperties.Enabled = false;
			this.RemoveSubPanelCtrl();
		}

		private void ShowConfirmCtrl(String packageName, String packagePath, String[] resourceFiles)
		{
			InstallConfirmCtrl ctrl = this.CreateSubPanelCtrl<InstallConfirmCtrl>(out Boolean isCtrlCreated);
			if(isCtrlCreated)
				ctrl.OnConfirm += this.Ctrl_OnConfirm;

			ctrl.ShowConfirm(packageName, packagePath, resourceFiles);
		}

		private T CreateSubPanelCtrl<T>(out Boolean ctrlCreated) where T : Control, new()
		{
			T result = null;
			if(splitMain.Panel2.Controls.Count > 0)
				result = splitMain.Panel2.Controls[0] as T;
			if(result == null)
			{
				ctrlCreated = true;
				this.RemoveSubPanelCtrl();
				result = new T
				{
					Dock = DockStyle.Fill
				};
				splitMain.Panel2MinSize = result.MinimumSize.Width;
				splitMain.SplitterDistance = splitMain.Width - result.Width;
				splitMain.Panel2.Controls.Add(result);
			} else
				ctrlCreated = false;

			splitMain.Panel2Collapsed = false;
			result.Focus();
			return result;
		}

		private void RemoveSubPanelCtrl<T>() where T : Control
		{
			if(splitMain.Panel2.Controls.Count > 0
				&& splitMain.Panel2.Controls[0].GetType() == typeof(T))
				this.RemoveSubPanelCtrl();
		}

		private void RemoveSubPanelCtrl()
		{
			foreach(Control ctrl in splitMain.Panel2.Controls)
				ctrl.Dispose();
			splitMain.Panel2.Controls.Clear();
			splitMain.Panel2Collapsed = true;
		}

		private WorkerInstallItem.PackageInstallItem ParsePackage(String filePath,Boolean ignoreWarnings=false)
		{
			String packageName = null;
			String versionCode = null;
			String expansionDirectory = null;
			String[] expansionFiles = new String[] { };
			try
			{
				using(AlphaOmega.Debug.ApkFile reader = new AlphaOmega.Debug.ApkFile(filePath))
					if(reader.IsValid && reader.AndroidManifest != null)
					{
						packageName = reader.AndroidManifest.Package;
						versionCode = reader.AndroidManifest.VersionCode;
					}
			}catch(Exception exc)
			{
				PluginWindows.Trace.TraceData(TraceEventType.Error, 7, exc);
			}

			if(packageName == null && !ignoreWarnings && MessageBox.Show("Can't determine valid apk file. Do you want to continue installing this package?", "Android package installer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return null;//If the user still wants to continue the installation, then that's their problem...

			if(packageName != null)
			{
				//https://developer.android.com/google/play/expansion-files
				expansionDirectory = Path.Combine(Path.GetDirectoryName(filePath), packageName);
				Boolean showWarning = false;
				if(Directory.Exists(expansionDirectory))
				{
					showWarning = true;
					expansionFiles = Directory.GetFileSystemEntries(expansionDirectory);
					String[] validExpansions = new String[] { String.Format("main.{0}.{1}.obb", versionCode, packageName), String.Format("path.{0}.{1}.obb", versionCode, packageName) };
					if(expansionFiles.Length == 0 || expansionFiles.Select(p => Path.GetFileName(p)).All(p => validExpansions.Any(v => v.Equals(p, StringComparison.OrdinalIgnoreCase))))
						showWarning = false;
				}
				if(showWarning && !ignoreWarnings)
				{
					this.ShowConfirmCtrl(packageName, filePath, expansionFiles);
					return null;
				}
			}

			return new WorkerInstallItem.PackageInstallItem(packageName, filePath, expansionFiles);
		}

		/// <summary>Get a list of selected applications in the list</summary>
		/// <returns>The selected application in the list</returns>
		private IEnumerable<AdbAppInfo> GetSelectedApplications()
		{
			foreach(ListViewItem item in lvInstalled.SelectedItems)
				yield return (AdbAppInfo)item.Tag;
		}

		private void Ctrl_OnConfirm(Object sender, InstallConfirmCtrl.InstallConfirmEventArgs e)
		{
			this.RemoveSubPanelCtrl();
			if(e.IsConfirm)
				this.StartProcess<WorkerInstallItem>(new WorkerInstallItem(e.Package));
		}

		private void splitMain_MouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if(splitMain.SplitterRectangle.Contains(e.Location))
			{
				this.RemoveSubPanelCtrl();
				splitMain.Panel2Collapsed = true;
			}
		}

		private void tsbnOpenAdb_Click(Object sender, EventArgs e)
		{
			if(MessageBox.Show("Do you want to download adb.exe from google website?", "adb.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				try
				{
					this.Plugin.Settings.AdbPath = this.Plugin.Settings.DownloadAdbClient();
					return;
				} catch(Exception exc)
				{
					PluginWindows.Trace.TraceData(TraceEventType.Error, 6, exc);
				}

			using(OpenFileDialog dlg = new OpenFileDialog() { Filter = "Android Debug Bridge|adb.exe|All files|*.*", CheckFileExists = true, FileName = "adb.exe", })
				if(dlg.ShowDialog() == DialogResult.OK)
					this.Plugin.Settings.AdbPath = dlg.FileName;
		}

		private void tsbnDeviceProperties_Click(Object sender, EventArgs e)
		{
			base.Cursor = Cursors.WaitCursor;
			try
			{
				Dictionary<String, String> properties = this.Client.GetDeviceProperties();

				DevicePropertiesCtrl ctrl = this.CreateSubPanelCtrl<DevicePropertiesCtrl>(out Boolean _);
				ctrl.ShowProperties(properties);
			} finally
			{
				base.Cursor = Cursors.Default;
			}
		}

		private void tsbnDeviceInstall_Click(Object sender, EventArgs e)
		{
			this.RemoveSubPanelCtrl<InstallConfirmCtrl>();

			String filePath;
			using(OpenFileDialog dlg = new OpenFileDialog() { Filter = "Android Package|*.apk|All Files|*.*", })
				if(dlg.ShowDialog() == DialogResult.OK)
					filePath = dlg.FileName;
				else
					return;

			var package = this.ParsePackage(filePath);
			if(package != null)
				this.StartProcess<WorkerInstallItem>(new WorkerInstallItem(package));
		}

		private void tsddlDevices_DropDown(Object sender, EventArgs e)
		{
			if(tsddlDevices.Items.Count > 0)
				return;

			base.Cursor = Cursors.WaitCursor;
			try
			{
				foreach(AdbDevice device in this.Client.GetDevices())
					tsddlDevices.Items.Add(device);
			} finally
			{
				base.Cursor = Cursors.Default;
			}
		}

		private void tsddlDevices_SelectedIndexChanged(Object sender, EventArgs e)
		{
			base.Cursor = Cursors.WaitCursor;
			try
			{
				AdbDevice device = this.SelectedDevice;
				if(device == null)
					this.ClearDeviceData();
				else
				{
					this.Window.Caption = String.Join(" - ", device.ToString(), DocumentAdbClient.Caption);
					this.Client.SetDevice(device.SerialNumber);
					tsbnDeviceProperties.Enabled = tsbnDeviceInstall.Enabled = true;
					if(tabMain.SelectedTab == tabPackages)
					{
						List<ListViewItem> itemsToAdd = new List<ListViewItem>();
						String[] subItems = Array.ConvertAll(new String[lvShell.Columns.Count + 1], str => String.Empty);
						foreach(AdbAppInfo application in this.Client.GetApplications())
						{
							ListViewItem item = new ListViewItem(subItems)
							{
								Tag = application,
								Group = lvInstalled.Groups[application.Type.ToString()],
							};

							item.SubItems[colInstalledName.Index].Text = application.Name;
							item.SubItems[colInstalledFileName.Index].Text = application.FilePath;

							Boolean isHidden = false;
							if(application.Type == AdbAppInfo.AdbAppType.System || application.Type == AdbAppInfo.AdbAppType.Privileged)
								isHidden = true;
							else if(this.Plugin.Settings.HiddenPackages != null && Array.Exists(this.Plugin.Settings.HiddenPackages, str => str == application.Name))
								isHidden = true;

							if(isHidden)
								if(this.Plugin.Settings.ShowHiddenPackages)//Mark hidden app
									item.ForeColor = DocumentAdbClient.HiddenPackageColor;
								else//Hide hidden or system app
									continue;
							itemsToAdd.Add(item);
						}

						if(itemsToAdd.Count > 0)
						{
							lvInstalled.Items.Clear();
							lvInstalled.Items.AddRange(itemsToAdd.ToArray());
							lvInstalled.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
						}
					} else if(tabMain.SelectedTab == tabShell)
					{
						List<ListViewItem> itemsToAdd = new List<ListViewItem>();
						String[] subItems = Array.ConvertAll<String, String>(new String[lvShell.Columns.Count + 1], str => String.Empty);
						foreach(AdbFileInfo file in this.Client.GetDirectoryListing())
						{
							ListViewItem item = new ListViewItem(subItems)
							{
								Tag = file,
							};
							item.SubItems[colShellName.Index].Text = file.Name;
							itemsToAdd.Add(item);
						}

						if(itemsToAdd.Count > 0)
						{
							lvShell.Items.Clear();
							lvShell.Items.AddRange(itemsToAdd.ToArray());
							lvShell.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
						}
					}
				}
			} finally
			{
				base.Cursor = Cursors.Default;
			}
		}

		private void cmsInstalled_Opening(Object sender, CancelEventArgs e)
		{
			tsmiInstalledApkView.Visible = this.ApkViewPlugin.IsPluginLoaded;
			Boolean isHidden = false;
			Boolean isVisible = false;
			foreach(ListViewItem item in lvInstalled.SelectedItems)
				if(item.ForeColor == HiddenPackageColor)
					isHidden = true;
				else
					isVisible = true;

			tsmiInstalledHide.Visible = isVisible == true && isHidden == false;
			tsmiInstalledShow.Visible = isVisible == false && isHidden == true;

			tsmiInstalledUninstall.Enabled
				= tsmiInstalledDownload.Enabled
				= tsmiInstalledGooglePlay.Enabled
				= tsmiInstalledApkView.Enabled
				= isVisible || isHidden;
		}

		private void cmsInstalled_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			cmsInstalled.Close(ToolStripDropDownCloseReason.ItemClicked);
			AdbDevice device = this.SelectedDevice;

			if(e.ClickedItem == tsmiInstalledUninstall)
			{
				if(MessageBox.Show("Are you sure you want to uninstall selected application(s)?", "Uninstall", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					this.StartProcess<WorkerUninstallItem>(new WorkerUninstallItem(this.GetSelectedApplications()));
			} else if(e.ClickedItem == tsmiInstalledDownload)
			{
				WorkerDownloadItem download = new WorkerDownloadItem(this.GetSelectedApplications(), null);
				if(download.Applications.Length == 1)
				{
					using(SaveFileDialog dlg = new SaveFileDialog() { OverwritePrompt = true, AddExtension = true, FileName = download.Applications[0].Name + ".apk", Filter = "All files (*.*)|*.*", })
						if(dlg.ShowDialog() == DialogResult.OK)
							download.FilePath = dlg.FileName;
						else
							return;
				} else
				{
					using(FolderBrowserDialog dlg = new FolderBrowserDialog() { Description = "Choose path to store packages", ShowNewFolderButton = true, })
						if(dlg.ShowDialog() == DialogResult.OK)
							download.FilePath = dlg.SelectedPath;
				}

				this.StartProcess<WorkerDownloadItem>(download);
			} else if(e.ClickedItem == tsmiInstalledApkView)
			{
				WorkerApkViewItem apkView = new WorkerApkViewItem(this.GetSelectedApplications());
				this.StartProcess<WorkerApkViewItem>(apkView);
			} else if(e.ClickedItem == tsmiInstalledGooglePlay)
			{
				foreach(AdbAppInfo application in this.GetSelectedApplications())
					Process.Start(String.Format(this.Plugin.Settings.AndroidPackageInfoUrl, application.Name));
			}else if(e.ClickedItem == tsmiInstalledHide || e.ClickedItem == tsmiInstalledShow)
			{
				Boolean isHide = e.ClickedItem == tsmiInstalledHide;
				List<String> hiddenPackages = new List<String>(this.Plugin.Settings.HiddenPackages ?? new String[] { });
				foreach(ListViewItem item in lvInstalled.SelectedItems)
				{
					AdbAppInfo application = (AdbAppInfo)item.Tag;
					if(application.Type == AdbAppInfo.AdbAppType.ThirdParty)
					{
						if(isHide)
						{
							hiddenPackages.Add(application.Name);
							item.ForeColor = DocumentAdbClient.HiddenPackageColor;
						} else
						{
							hiddenPackages.Remove(application.Name);
							item.ForeColor = Form.DefaultForeColor;
						}
					}
				}
				this.Plugin.Settings.HiddenPackages = hiddenPackages.Count == 0 ? null : hiddenPackages.ToArray();
			}
		}

		private void StartProcess<T>(T worker) where T : WorkerItem
		{
			if(bwProcess.IsBusy)
			{
				ctrlMessage.ShowMessage(MessageCtrl.StatusMessageType.Failed, "Worker is busy");
				return;
			}

			base.Cursor = Cursors.WaitCursor;
			tsddlDevices.Enabled = false;
			worker.Device = this.SelectedDevice;
			bwProcess.RunWorkerAsync(worker);
		}

		private void bwProcess_DoWork(Object sender, DoWorkEventArgs e)
		{
			if(e.Argument is WorkerUninstallItem uninstall)
			{
				foreach(AdbAppInfo package in uninstall.PackageNames)
				{
					bwProcess.ReportProgress(1, $"Uninstalling {package.Name}");
					this.Client.UninstallApplication(package.Name);
				}
			}else if(e.Argument is WorkerInstallItem install)
			{
				foreach(var package in install.Packages)
				{
					bwProcess.ReportProgress(1, $"Installing {package.PackageName}");
					this.Client.InstallAndroidPackage(package.FilePath, this.Plugin.Settings.ReinstallExisting);

					foreach(String file in package.Resources)
					{
						String remoteFilePath = Path.Combine(this.Plugin.Settings.AndroidObbPath + package.PackageName, Path.GetFileName(file));
						bwProcess.ReportProgress(2, $"Uploading {remoteFilePath}");
						this.Client.UploadFile2(file, remoteFilePath);
					}
				}
				e.Result = install;
			}else if(e.Argument is WorkerDownloadItem download)
			{
				foreach(AdbAppInfo package in download.Applications)
				{
					bwProcess.ReportProgress(1, $"Downloading {package.Name}");
					if(download.Applications.Length == 1)
						this.Client.DownloadFile(package.FilePath, download.FilePath);
					else
						this.Client.DownloadFile(package.FilePath, Path.Combine(download.FilePath, package.Name + ".apk"));
				}
			}else if(e.Argument is WorkerApkViewItem apkView)
			{
				foreach(AdbAppInfo package in apkView.Applications)
				{
					bwProcess.ReportProgress(1, $"Downloading {package.Name}");
					String tempFilePath = this.Plugin.Settings.GetApkTempPath(package.FileName);
					this.Client.DownloadFile(package.FilePath, tempFilePath);
					apkView.TempPath.Add(tempFilePath);
				}
				e.Result = apkView;
			}
		}

		private void bwProcess_ProgressChanged(Object sender, ProgressChangedEventArgs e)
		{
			if(e.UserState is WorkerReportProgress report)
				ctrlMessage.ShowMessage(report.Status, report.Message);
			else if(e.UserState is String message)
				ctrlMessage.ShowMessage(MessageCtrl.StatusMessageType.Progress, message);
		}

		private void bwProcess_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			base.Cursor = Cursors.Default;
			tsddlDevices.Enabled = true;

			if(e.Error != null)
			{
				ctrlMessage.ShowMessage(MessageCtrl.StatusMessageType.Failed, e.Error.Message);
				PluginWindows.Trace.TraceData(TraceEventType.Error, 1, e.Error);
			} else if(e.Result is String message)
				ctrlMessage.ShowMessage(MessageCtrl.StatusMessageType.Success, message);
			else
			{
				ctrlMessage.ShowMessage(MessageCtrl.StatusMessageType.Success, null);
				if(e.Result is WorkerUninstallItem uninstall)
				{
					foreach(AdbAppInfo application in uninstall.PackageNames)
						foreach(ListViewItem item in lvInstalled.Items)
							if(((AdbAppInfo)item.Tag).FileName == application.FileName)
							{
								item.Remove();
								break;
							}
				} else if(e.Result is WorkerApkViewItem apkView)
				{
					foreach(String path in apkView.TempPath)
						this.ApkViewPlugin.OpenApkFile(path);
				}else if(e.Result is WorkerInstallItem install)
				{
					this.tsddlDevices_SelectedIndexChanged(sender, e);
					foreach(ListViewItem item in lvInstalled.Items)
						foreach(var package in install.Packages)
							if(((AdbAppInfo)item.Tag).Name == package.PackageName)
							{
								item.Selected = true;
								break;
							}
				}
			}
		}

		private void lvInstalled_DragEnter(Object sender, DragEventArgs e)
			=> e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;

		private void lvInstalled_DragDrop(Object sender, DragEventArgs e)
		{
			String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
			List<WorkerInstallItem.PackageInstallItem> packages = new List<WorkerInstallItem.PackageInstallItem>();
			if(files.Length == 1)
			{
				WorkerInstallItem.PackageInstallItem package = this.ParsePackage(files[0]);
				if(package != null)
					packages.Add(package);
			} else
				foreach(String file in files)
					if(".apk".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase))
					{
						WorkerInstallItem.PackageInstallItem package = this.ParsePackage(file, true);
						if(package != null)
							packages.Add(package);
						else
							PluginWindows.Trace.TraceEvent(TraceEventType.Information, 11, "Can't parse file: {0}", file);
					} else
						PluginWindows.Trace.TraceEvent(TraceEventType.Information, 11, "Only .apk files supported for Drag'n'Drop. File: {0}", file);

			if(packages.Count > 0)
				this.StartProcess<WorkerInstallItem>(new WorkerInstallItem(packages.ToArray()));
		}
	}
}