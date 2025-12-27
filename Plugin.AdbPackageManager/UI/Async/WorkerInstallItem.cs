using System;

namespace Plugin.AdbPackageManager.UI.Async
{
	internal class WorkerInstallItem : WorkerItem
	{
		internal class PackageInstallItem
		{
			public String PackageName { get; private set; }
			public String FilePath { get; private set; }
			public String[] Resources { get; private set; }

			public PackageInstallItem(String packageName, String filePath, String[] resources)
			{
				this.PackageName = packageName;
				this.FilePath = filePath;
				this.Resources = resources;
			}
		}

		public PackageInstallItem[] Packages { get; private set; }

		public WorkerInstallItem(params PackageInstallItem[] packages)
			=> this.Packages = packages;
	}
}