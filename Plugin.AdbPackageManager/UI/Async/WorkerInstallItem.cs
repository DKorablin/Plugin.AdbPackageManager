using System;

namespace Plugin.AdbPackageManager.UI.Async
{
	/// <summary>Worker item describing one or more APK packages to install</summary>
	internal class WorkerInstallItem : WorkerItem
	{
		/// <summary>Describes a single APK package and its associated resource files</summary>
		internal class PackageInstallItem
		{
			/// <summary>Android package name</summary>
			public String PackageName { get; private set; }
			/// <summary>Local path to the APK file</summary>
			public String FilePath { get; private set; }
			/// <summary>Associated resource files to upload alongside the APK</summary>
			public String[] Resources { get; private set; }

			/// <summary>Initializes a new instance with the given package details</summary>
			/// <param name="packageName">Android package name</param>
			/// <param name="filePath">Local path to the APK file</param>
			/// <param name="resources">Associated resource files</param>
			public PackageInstallItem(String packageName, String filePath, String[] resources)
			{
				this.PackageName = packageName;
				this.FilePath = filePath;
				this.Resources = resources;
			}
		}

		/// <summary>Packages to install</summary>
		public PackageInstallItem[] Packages { get; private set; }

		/// <summary>Initializes a new instance with the given packages</summary>
		/// <param name="packages">One or more packages to install</param>
		public WorkerInstallItem(params PackageInstallItem[] packages)
			=> this.Packages = packages;
	}
}