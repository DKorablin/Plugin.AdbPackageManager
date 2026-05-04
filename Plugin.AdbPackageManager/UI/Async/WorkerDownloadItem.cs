using System;
using System.Linq;
using System.Collections.Generic;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	/// <summary>Worker item describing a set of applications to download from the device</summary>
	internal class WorkerDownloadItem : WorkerItem
	{
		/// <summary>Applications to download</summary>
		public AdbAppInfo[] Applications { get; private set; }
		/// <summary>Local destination path for the downloaded files</summary>
		public String FilePath { get; set; }

		/// <summary>Initializes a new instance with the given applications and destination path</summary>
		/// <param name="applications">Applications to download</param>
		/// <param name="filePath">Local destination path</param>
		public WorkerDownloadItem(IEnumerable<AdbAppInfo> applications, String filePath)
		{
			this.Applications = applications.ToArray();
			this.FilePath = filePath;
		}
	}
}