using System;
using System.Diagnostics;
using System.IO;

namespace Plugin.AdbPackageManager.Adb
{
	[DebuggerDisplay("Name={" + nameof(Name) + "}, Type={" + nameof(Type) + "}, Location={" + nameof(Location) + "}, FileName={" + nameof(FileName) + "}")]
	public class AdbAppInfo
	{
		public enum AdbAppType
		{
			Unknown = 0,
			System = 1,
			Privileged = 2,
			ThirdParty = 3
		}

		public enum AdbAppLocation
		{
			InternalMemory = 0,
			ExternalMemory = 1
		}

		public String Name { get; private set; }
		public String FilePath { get; private set; }
		public String FileName => Path.GetFileName(this.FilePath);
		public AdbAppType Type { get; private set; }
		public AdbAppLocation Location { get; private set; }

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