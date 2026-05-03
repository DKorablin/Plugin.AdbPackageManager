using System;
using System.IO;

namespace Plugin.AdbPackageManager.Adb
{
	/// <summary>Utility methods for path manipulation and Unix time conversion</summary>
	internal static class Utils
	{
		/// <summary>Combines two path segments using forward slashes</summary>
		/// <param name="path1">First path segment</param>
		/// <param name="path2">Second path segment</param>
		/// <returns>Combined path with forward slashes</returns>
		public static String CombinePath(String path1, String path2)
			=> Path.Combine(path1, path2).Replace('\\', '/');

		/// <summary>Returns the Unix epoch as a UTC DateTime</summary>
		/// <returns>DateTime representing the Unix epoch</returns>
		private static DateTime GetUnixEpoch()
			=> new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>Converts a Unix timestamp to a local DateTime</summary>
		/// <param name="unixTime">Seconds elapsed since the Unix epoch</param>
		/// <returns>Equivalent local DateTime</returns>
		public static DateTime FromUnixTime(Int32 unixTime)
			=> GetUnixEpoch().AddSeconds(unixTime).ToLocalTime();

		/// <summary>Converts a DateTime to a Unix timestamp</summary>
		/// <param name="dateTime">DateTime to convert</param>
		/// <returns>Seconds elapsed since the Unix epoch</returns>
		public static Int32 ToUnixTime(DateTime dateTime)
		{
			var timeSpan = dateTime.ToUniversalTime() - Utils.GetUnixEpoch();
			return Convert.ToInt32(timeSpan.TotalSeconds);
		}
	}
}