using System;

namespace Plugin.AdbPackageManager.UI.Async
{
	/// <summary>Carries a status message reported from a background worker operation</summary>
	internal class WorkerReportProgress
	{
		/// <summary>Status type of the message</summary>
		public MessageCtrl.StatusMessageType Status { get; set; }
		/// <summary>Message text to display</summary>
		public String Message { get; set; }
	}
}