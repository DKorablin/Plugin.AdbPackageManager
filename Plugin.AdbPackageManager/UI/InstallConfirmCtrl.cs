using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Plugin.AdbPackageManager.UI
{
	internal partial class InstallConfirmCtrl : UserControl
	{
		public class InstallConfirmEventArgs : EventArgs
		{
			public Boolean IsConfirm { get; set; }
			public Async.WorkerInstallItem.PackageInstallItem Package { get; set; }
		}

		public String PackageName => txtPackage.Text;

		public String PackagePath { get; private set; }

		public String ResourcesDirectory { get; private set; }

		public event EventHandler<InstallConfirmEventArgs> OnConfirm;

		public InstallConfirmCtrl()
			=> InitializeComponent();

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