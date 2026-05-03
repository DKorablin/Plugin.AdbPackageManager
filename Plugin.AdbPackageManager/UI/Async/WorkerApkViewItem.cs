using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	/// <summary>Worker item describing a set of applications to download and open in the APK viewer</summary>
	internal class WorkerApkViewItem : WorkerItem
	{
		/// <summary>Applications to download and view</summary>
		public AdbAppInfo[] Applications { get; private set; }
		/// <summary>Temporary local paths of the downloaded APK files</summary>
		public List<String> TempPath { get; private set; }

		/// <summary>Initializes a new instance with the given applications</summary>
		/// <param name="applications">Applications to download and view</param>
		public WorkerApkViewItem(IEnumerable<AdbAppInfo> applications)
		{
			this.Applications = applications.ToArray();
			this.TempPath = new List<String>();
		}
	}
}