using System;
using Plugin.AdbPackageManager.Adb;

namespace Plugin.AdbPackageManager.UI.Async
{
	/// <summary>Base class for background worker operation descriptors</summary>
	internal class WorkerItem
	{
		/// <summary>Target device for the operation</summary>
		public AdbDevice Device { get; set; }

		/// <summary>Initializes a new instance</summary>
		protected WorkerItem()
		{ }
	}
}