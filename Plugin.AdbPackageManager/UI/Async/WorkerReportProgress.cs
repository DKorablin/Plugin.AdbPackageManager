using System;

namespace Plugin.AdbPackageManager.UI.Async
{
	internal class WorkerReportProgress
	{
		public MessageCtrl.StatusMessageType Status { get; set; }
		public String Message { get; set; }
	}
}