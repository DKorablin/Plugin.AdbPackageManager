using System;
using System.Linq;
using System.Collections.Generic;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	/// <summary>Worker item describing a set of applications to uninstall</summary>
	internal class WorkerUninstallItem : WorkerItem
	{
		/// <summary>Applications to uninstall</summary>
		public AdbAppInfo[] PackageNames { get; private set; }

		/// <summary>Initializes a new instance with the given applications</summary>
		/// <param name="packageNames">Applications to uninstall</param>
		public WorkerUninstallItem(IEnumerable<AdbAppInfo> packageNames)
			=> this.PackageNames = packageNames.ToArray();
	}
}