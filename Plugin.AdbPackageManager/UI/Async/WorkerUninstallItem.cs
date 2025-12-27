using System;
using System.Linq;
using System.Collections.Generic;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	internal class WorkerUninstallItem : WorkerItem
	{
		public AdbAppInfo[] PackageNames { get; private set; }

		public WorkerUninstallItem(IEnumerable<AdbAppInfo> packageNames)
			=> this.PackageNames = packageNames.ToArray();
	}
}