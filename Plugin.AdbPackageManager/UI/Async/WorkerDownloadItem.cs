using System;
using System.Linq;
using System.Collections.Generic;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	internal class WorkerDownloadItem : WorkerItem
	{
		public AdbAppInfo[] Applications { get; private set; }
		public String FilePath { get; set; }

		public WorkerDownloadItem(IEnumerable<AdbAppInfo> applications, String filePath)
		{
			this.Applications = applications.ToArray();
			this.FilePath = filePath;
		}
	}
}