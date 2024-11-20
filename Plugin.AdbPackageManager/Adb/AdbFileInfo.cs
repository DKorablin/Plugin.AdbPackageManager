using System;
using System.Diagnostics;

namespace Plugin.AdbPackageManager.Adb
{
	[DebuggerDisplay("Name={" + nameof(Name) + "}, Size={" + nameof(Size) + "}, Mode={" + nameof(Mode) + "}")]
	public class AdbFileInfo
	{
		public String FullName { get; private set; }
		public String Name { get; private set; }
		public Int32 Size { get; private set; }
		public Int32 Mode { get; private set; }
		public DateTime Modified { get; private set; }

		public Boolean IsFile => (this.Mode & 0x8000) > 0;
		public Boolean IsDirectory => (this.Mode & 0x4000) > 0;
		public Boolean IsSymbolicLink => (this.Mode & 0xA000) > 0;

		internal AdbFileInfo(String fullName, String name, Int32 size, Int32 mode, DateTime modified)
		{
			this.FullName = fullName;
			this.Name = name;
			this.Size = size;
			this.Mode = mode;
			this.Modified = modified;
		}
	}
}