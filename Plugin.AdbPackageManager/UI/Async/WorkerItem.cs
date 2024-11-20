using System;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	internal class WorkerItem
	{
		public AdbDevice Device { get; set; }

		protected WorkerItem()
		{ }
	}
}