using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	internal class WorkerApkViewItem : WorkerItem
	{
		public AdbAppInfo[] Applications { get; private set; }
		public List<String> TempPath { get; private set; }

		public WorkerApkViewItem(IEnumerable<AdbAppInfo> applications)
		{
			this.Applications = applications.ToArray();
			this.TempPath = new List<String>();
		}
	}
}