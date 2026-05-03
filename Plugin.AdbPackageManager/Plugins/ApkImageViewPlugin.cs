using System;
using System.Diagnostics;
using SAL.Flatbed;

namespace Plugin.AdbPackageManager.Plugins
{
	/// <summary>Logic for interacting with the APK file viewing plugin</summary>
	internal class ApkImageViewPlugin
	{
		/// <summary>Plugin constants</summary>
		private static class Constants
		{
			/// <summary>ID of the APK file viewing plugin</summary>
			public const String Name = "ca0bd89a-318f-4fa3-9a5e-49b3e1358a53";

			/// <summary>Public plugin methods</summary>
			public static class Methods
			{
				/// <summary>Open file for viewing</summary>
				public const String OpenFile = "OpenFile";
				/// <summary>Close file</summary>
				public const String CloseFile = "CloseFile";
			}
		}

		private readonly IHost _host;
		private Boolean _isLoaded = false;
		private IPluginDescription _apkViewPlugin;
		private IPluginDescription ApkViewPlugin
		{
			get
			{
				if(this._apkViewPlugin == null && !this._isLoaded)
				{
					this._apkViewPlugin = this._host.Plugins[Constants.Name];
					this._isLoaded = true;
				}
				return this._apkViewPlugin;
			}
		}

		/// <summary>Plugin loaded</summary>
		public Boolean IsPluginLoaded => this.ApkViewPlugin != null;

		public ApkImageViewPlugin(IHost host)
			=> this._host = host;

		/// <summary>Open APK file to view internal content</summary>
		/// <param name="filePath">Path to file</param>
		public void OpenApkFile(String filePath)
		{
			IPluginDescription plugin = this.ApkViewPlugin;
			if(plugin == null)
				PluginWindows.Trace.TraceEvent(TraceEventType.Error, 10, "Required plugin ID={0} not found", Constants.Name);
			else
			{
				plugin.Type.GetMember<IPluginMethodInfo>(Constants.Methods.OpenFile)
					.Invoke(filePath);
			}
		}

		/// <summary>Close previously opened APK file</summary>
		/// <param name="filePath">Path to file</param>
		public void CloseApkFile(String filePath)
		{
			IPluginDescription plugin = this.ApkViewPlugin;
			if(plugin == null)
				PluginWindows.Trace.TraceEvent(TraceEventType.Error, 10, "Required plugin ID={0} not found", Constants.Name);
			else
			{
				plugin.Type.GetMember<IPluginMethodInfo>(Constants.Methods.CloseFile)
					.Invoke(filePath);
			}
		}
	}
}