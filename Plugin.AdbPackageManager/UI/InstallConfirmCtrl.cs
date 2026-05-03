using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Plugin.AdbPackageManager.UI
{
	/// <summary>Panel control for confirming an APK installation with optional resource files</summary>
	internal partial class InstallConfirmCtrl : UserControl
	{
		/// <summary>Event arguments for the installation confirmation result</summary>
		public class InstallConfirmEventArgs : EventArgs
		{
			/// <summary>True when the user confirmed the installation</summary>
			public Boolean IsConfirm { get; set; }
			/// <summary>Package to install when the user confirmed</summary>
			public Async.WorkerInstallItem.PackageInstallItem Package { get; set; }
		}

		/// <summary>Package name entered by the user</summary>
		public String PackageName => txtPackage.Text;

		/// <summary>Local path to the APK file being installed</summary>
		public String PackagePath { get; private set; }

		/// <summary>Directory containing the associated resource files</summary>
		public String ResourcesDirectory { get; private set; }

		/// <summary>Raised when the user confirms or cancels the installation</summary>
		public event EventHandler<InstallConfirmEventArgs> OnConfirm;

		/// <summary>Initializes the control</summary>
		public InstallConfirmCtrl()
			=> InitializeComponent();

		/// <summary>Populates the control with the given package details and resource file list</summary>
		/// <param name="packageName">Android package name</param>
		/// <param name="packagePath">Local path to the APK file</param>
		/// <param name="resourceFiles">Associated resource files to display</param>
		public void ShowConfirm(String packageName, String packagePath, String[] resourceFiles)
		{
			this.PackagePath = packagePath;
			this.ResourcesDirectory = Path.GetDirectoryName(resourceFiles[0]);
			txtPackage.Text = packageName;
			clbResources.Items.Clear();
			clbResources.Items.AddRange(Directory.GetFileSystemEntries(this.ResourcesDirectory).Select(p => Path.GetFileName(p)).ToArray());
			for(Int32 loop = 0; loop < clbResources.Items.Count; loop++)
				clbResources.SetItemChecked(loop, true);
		}

		private void bnOk_Click(Object sender, EventArgs e)
		{
			String[] resources = clbResources.CheckedItems.Cast<String>().Select(p => Path.Combine(this.ResourcesDirectory, p)).ToArray();
			this.OnConfirm(this, new InstallConfirmEventArgs()
			{
				IsConfirm = true,
				Package = new Async.WorkerInstallItem.PackageInstallItem(this.PackageName, this.PackagePath, resources),
			});
		}

		private void bnCancel_Click(Object sender, EventArgs e)
			=> this.OnConfirm(this, new InstallConfirmEventArgs() { IsConfirm = false, });
	}
}