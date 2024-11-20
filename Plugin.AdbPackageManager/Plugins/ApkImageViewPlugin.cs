using System;
using System.Diagnostics;
using SAL.Flatbed;

namespace Plugin.AdbPackageManager.Plugins
{
	/// <summary>Логика взаимодействия с плагином просмотра APK файла</summary>
	internal class ApkImageViewPlugin
	{
		/// <summary>Плагин таймера</summary>
		private static class Constants
		{
			/// <summary>ID плагина с просмотра APK файла</summary>
			public const String Name = "ca0bd89a-318f-4fa3-9a5e-49b3e1358a53";

			/// <summary>Публичные методы плагина</summary>
			public static class Methods
			{
				/// <summary>Открыть файл для просмотра</summary>
				public const String OpenFile = "OpenFile";
				/// <summary>Закрыть файл для просмотра</summary>
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

		/// <summary>Плагин загружен</summary>
		public Boolean IsPluginLoaded => this.ApkViewPlugin != null;

		public ApkImageViewPlugin(IHost host)
			=> this._host = host;

		/// <summary>Открыть APK файл для просмотра внутреннего содержимого</summary>
		/// <param name="filePath">Путь к файлу</param>
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

		/// <summary>Закрыть открытый ранее APK файл</summary>
		/// <param name="filePath">Путь к файлу</param>
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