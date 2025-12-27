using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Windows.Forms.Design;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace Plugin.AdbPackageManager
{
	public class PluginSettings : INotifyPropertyChanged
	{
		private const String AdbPackageManagerTempPath = "Plugin.AdbPackageManager";

		private static class Default
		{
			public const String AndroidObbPath = "/sdcard/Android/obb/";
			public const String AndroidPackageInfoUrl = "https://play.google.com/store/apps/details?id={0}";
		}

		private String _adbPath;
		private String _androidObbPath = Default.AndroidObbPath;
		private String _androidPackageInfoUrl = Default.AndroidPackageInfoUrl;
		private Boolean _isTempFolderCreated = false;
		private Boolean _installToSdCard = false;
		private Boolean _reinstallExisting = true;
		private Boolean _showHiddenPackages = false;
		private String[] _hiddenPackages = new String[] { };

		[Description("Path to adb.exe file")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public String AdbPath
		{//https://dl.google.com/android/repository/platform-tools-latest-windows.zip
			get
			{
				if(this._adbPath != null)
					return this._adbPath;
				String path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Android SDK Tools", "Path", null) as String
					?? Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Android SDK Tools", "Path", null) as String;
				return path == null
					? null
					: Path.Combine(path, @"platform-tools\adb.exe");
			}
			set
			{
				if(!String.IsNullOrEmpty(value) && !File.Exists(value))
					return;

				this.SetField(ref this._adbPath, value, nameof(this.AdbPath));
			}
		}

		[DisplayName("APK info url")]
		[Description("Uri where you can view Android Package details.\r\n{0} - required\r\nAbsolute Uri - required")]
		public String AndroidPackageInfoUrl
		{
			get => this._androidPackageInfoUrl;
			set
			{
				if(String.IsNullOrEmpty(value))
					value = null;
				else if(value.IndexOf("{0}") == -1)
					value = null;
				else if(!Uri.IsWellFormedUriString(value, UriKind.Absolute))
					value = null;

				this.SetField(ref this._androidPackageInfoUrl, value ?? Default.AndroidPackageInfoUrl, nameof(this.AndroidPackageInfoUrl));
			}
		}

		[Category("Installation")]
		[DisplayName("APK Expansion Files")]
		[Description("Path for apk expansions files on Android device")]
		[DefaultValue(Default.AndroidObbPath)]
		public String AndroidObbPath
		{
			get => this._androidObbPath;
			set
			{
				if(String.IsNullOrEmpty(value))
					value = Default.AndroidObbPath;

				this.SetField(ref this._androidObbPath, value, nameof(this.AndroidObbPath));
			}
		}

		[Category("Installation")]
		[DisplayName("Install to SD card")]
		[Description("Install APK to SD card")]
		[DefaultValue(false)]
		public Boolean InstallToSdCard
		{
			get => this._installToSdCard;
			set => this.SetField(ref this._installToSdCard, value, nameof(this.InstallToSdCard));
		}

		[Category("Installation")]
		[DisplayName("Reinstall existing")]
		[Description("Reinstall an existing app, keeping its data")]
		[DefaultValue(true)]
		public Boolean ReinstallExisting
		{
			get => this._reinstallExisting;
			set => this.SetField(ref this._reinstallExisting, value, nameof(this.ReinstallExisting));
		}

		[Category("UI")]
		[DisplayName("Show Hidden")]
		[Description("Show system and hidden files and packages")]
		[DefaultValue(false)]
		public Boolean ShowHiddenPackages
		{
			get => this._showHiddenPackages;
			set => this.SetField(ref this._showHiddenPackages, value, nameof(this.ShowHiddenPackages));
		}

		[Category("UI")]
		[DisplayName("Hidden Packages")]
		[Description("Custom list of hidden files and packages")]
		public String[] HiddenPackages
		{
			get => this._hiddenPackages;
			set => this.SetField(ref this._hiddenPackages, value, nameof(this.HiddenPackages));
		}

		/// <summary>Get a link to a temporary file for saving an APK file from the device</summary>
		/// <param name="fileName">APK file name</param>
		/// <returns>Link to save the file</returns>
		public String GetApkTempPath(String fileName)
		{
			String path = Path.Combine(Path.GetTempPath(), PluginSettings.AdbPackageManagerTempPath);
			if(!Directory.Exists(path))
				Directory.CreateDirectory(path);

			this._isTempFolderCreated = true;

			return Path.Combine(path, fileName);
		}

		public String DownloadAdbClient()
		{
			String location = typeof(PluginSettings).Assembly.Location;
			String directoryName = Path.GetDirectoryName(location);
			String destination = Path.Combine(directoryName, "adb-platform-tools");
			if(Directory.Exists(destination))
			{
				if(Directory.GetFiles(destination, "*.*", SearchOption.AllDirectories).Length > 0)
				{
					String[] foundFile = Directory.GetFiles(destination, "adb.exe", SearchOption.AllDirectories);
					return foundFile.Length == 1
						? foundFile[0]
						: throw new FileNotFoundException($"Found adb-platform-tools folder at: {destination} but it does not contains adb.exe");
				}
			} else
				Directory.CreateDirectory(destination);

			String zipPath = this.GetApkTempPath("platform-tools-latest-windows.zip");
			using(WebClient client = new WebClient())
				client.DownloadFile(new Uri("https://dl.google.com/android/repository/platform-tools-latest-windows.zip"), zipPath);

			FastZip zip = new FastZip();
			zip.ExtractZip(zipPath, destination, FastZip.Overwrite.Always, null, null, null, true);

			String[] foundFiles1 = Directory.GetFiles(destination, "adb.exe", SearchOption.AllDirectories);
			if(foundFiles1.Length == 1)
				return foundFiles1[0];
			else
			{
				Directory.Delete(destination);
				throw new FileNotFoundException("Can't find adb.exe in downloaded package. Please find Android platform tools manually");
			}
		}

		/// <summary>Get a list of files in the temporary directory and delete the directory</summary>
		/// <param name="isDeleteDirectory">The directory must be deleted after cleaning</param>
		/// <returns>List of files in the temporary folder</returns>
		public IEnumerable<String> GetApkTempFiles(Boolean isDeleteDirectory = true)
		{
			if(!this._isTempFolderCreated)
				yield break;

			String path = Path.Combine(Path.GetTempPath(), PluginSettings.AdbPackageManagerTempPath);
			if(!Directory.Exists(path))
				yield break;

			foreach(String filePath in Directory.GetFiles(path))
				yield return filePath;

			if(isDeleteDirectory)
				Directory.Delete(path, true);
		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private Boolean SetField<T>(ref T field, T value, String propertyName)
		{
			if(EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
		#endregion INotifyPropertyChanged
	}
}