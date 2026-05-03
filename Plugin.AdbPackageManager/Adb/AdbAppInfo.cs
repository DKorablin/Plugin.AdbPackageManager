using System;
using System.Diagnostics;
using System.IO;

namespace Plugin.AdbPackageManager.Adb
{
	[DebuggerDisplay("Name={" + nameof(Name) + "}, Type={" + nameof(Type) + "}, Location={" + nameof(Location) + "}, FileName={" + nameof(FileName) + "}")]
	/// <summary>Information about an installed Android application</summary>
	public class AdbAppInfo
	{
		/// <summary>Type of installed Android application</summary>
		public enum AdbAppType
		{
			/// <summary>Unknown application type</summary>
			Unknown = 0,
			/// <summary>System application</summary>
			System = 1,
			/// <summary>Privileged system application</summary>
			Privileged = 2,
			/// <summary>Third-party application</summary>
			ThirdParty = 3
		}

		/// <summary>Storage location of the installed application</summary>
		public enum AdbAppLocation
		{
			/// <summary>Application is stored on internal memory</summary>
			InternalMemory = 0,
			/// <summary>Application is stored on external memory</summary>
			ExternalMemory = 1
		}

		/// <summary>Package name of the application</summary>
		public String Name { get; private set; }
		/// <summary>Full path to the APK file on the device</summary>
		public String FilePath { get; private set; }
		/// <summary>File name of the APK package</summary>
		public String FileName => Path.GetFileName(this.FilePath);
		/// <summary>Application installation type</summary>
		public AdbAppType Type { get; private set; }
		/// <summary>Storage location of the application</summary>
		public AdbAppLocation Location { get; private set; }

		/// <summary>Initializes a new instance with the given package name and file path</summary>
		/// <param name="name">Package name</param>
		/// <param name="filePath">Full path to the APK file on the device</param>
		public AdbAppInfo(String name, String filePath)
		{
			this.Name = name;
			this.FilePath = filePath;

			if(filePath.StartsWith("/system/app/"))
				this.Type = AdbAppType.System;
			else if(filePath.StartsWith("/system/priv-app/"))
				this.Type = AdbAppType.Privileged;
			else
				this.Type = AdbAppType.ThirdParty;

			this.Location = filePath.StartsWith("/system/") || filePath.StartsWith("/data/")
				? AdbAppLocation.InternalMemory
				: AdbAppLocation.ExternalMemory;
		}
	}
}