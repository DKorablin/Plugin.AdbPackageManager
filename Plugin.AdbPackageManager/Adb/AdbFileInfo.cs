using System;
using System.Diagnostics;

namespace Plugin.AdbPackageManager.Adb
{
	[DebuggerDisplay("Name={" + nameof(Name) + "}, Size={" + nameof(Size) + "}, Mode={" + nameof(Mode) + "}")]
	/// <summary>Represents a file or directory entry on a remote Android device</summary>
	public class AdbFileInfo
	{
		/// <summary>Full path to the file on the device</summary>
		public String FullName { get; private set; }
		/// <summary>File or directory name</summary>
		public String Name { get; private set; }
		/// <summary>File size in bytes</summary>
		public Int32 Size { get; private set; }
		/// <summary>File mode flags</summary>
		public Int32 Mode { get; private set; }
		/// <summary>Last modification date and time</summary>
		public DateTime Modified { get; private set; }

		/// <summary>Indicates whether the entry is a regular file</summary>
		public Boolean IsFile => (this.Mode & 0x8000) > 0;
		/// <summary>Indicates whether the entry is a directory</summary>
		public Boolean IsDirectory => (this.Mode & 0x4000) > 0;
		/// <summary>Indicates whether the entry is a symbolic link</summary>
		public Boolean IsSymbolicLink => (this.Mode & 0xA000) > 0;

		/// <summary>Initializes a new instance with the given file attributes</summary>
		/// <param name="fullName">Full path to the file on the device</param>
		/// <param name="name">File or directory name</param>
		/// <param name="size">File size in bytes</param>
		/// <param name="mode">File mode flags</param>
		/// <param name="modified">Last modification date and time</param>
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