using System;
using System.IO;

namespace Plugin.AdbPackageManager.Adb
{
	internal static class Utils
	{
		public static String CombinePath(String path1, String path2)
			=> Path.Combine(path1, path2).Replace('\\', '/');

		private static DateTime GetUnixEpoch()
			=> new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime FromUnixTime(Int32 unixTime)
			=> GetUnixEpoch().AddSeconds(unixTime).ToLocalTime();

		public static Int32 ToUnixTime(DateTime dateTime)
		{
			var timeSpan = dateTime.ToUniversalTime() - Utils.GetUnixEpoch();
			return Convert.ToInt32(timeSpan.TotalSeconds);
		}
	}
}