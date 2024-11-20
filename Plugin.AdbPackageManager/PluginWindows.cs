using System;
using System.Collections.Generic;
using System.Diagnostics;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.AdbPackageManager
{
	public class PluginWindows : IPlugin, IPluginSettings<PluginSettings>
	{
		private static TraceSource _trace;
		private PluginSettings _settings;
		private Dictionary<String, DockState> _documentTypes;

		internal IHostWindows HostWindows { get; }
		internal static TraceSource Trace => PluginWindows._trace ?? (PluginWindows._trace = PluginWindows.CreateTraceSource<PluginWindows>());

		/// <summary>Настройки для взаимодействия из хоста</summary>
		Object IPluginSettings.Settings => this.Settings;

		/// <summary>Настройки для взаимодействия из плагина</summary>
		public PluginSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new PluginSettings();
					this.HostWindows.Plugins.Settings(this).LoadAssemblyParameters(this._settings);
				}
				return this._settings;
			}
		}

		internal IMenuItem AdbClientMenu { get; set; }

		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{ typeof(DocumentAdbClient).ToString(), DockState.Document },
					};
				return this._documentTypes;
			}
		}

		public PluginWindows(IHostWindows hostWindows)
			=> this.HostWindows = hostWindows ?? throw new ArgumentNullException(nameof(hostWindows));

		public IWindow GetPluginControl(String typeName, Object args)
			=> this.CreateWindow(typeName, false, args);

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			IHostWindows host = this.HostWindows;
			if(host == null)
				PluginWindows.Trace.TraceEvent(TraceEventType.Error, 10, "Plugin {0} requires {1}", this, typeof(IHostWindows));
			else
			{
				IMenuItem menuTools = host.MainMenu.FindMenuItem("Tools");
				if(menuTools == null)
					PluginWindows.Trace.TraceEvent(TraceEventType.Error, 10, "Menu item 'Tools' not found");
				else
				{
					this.AdbClientMenu = menuTools.Create("ADB Client");
					this.AdbClientMenu.Name = "Tools.AdbClient";
					this.AdbClientMenu.Click += (sender, e) => { this.CreateWindow(typeof(DocumentAdbClient).ToString(), true); };
					menuTools.Items.Add(this.AdbClientMenu);
					return true;
				}
			}

			return false;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this.AdbClientMenu != null)
				this.HostWindows.MainMenu.Items.Remove(this.AdbClientMenu);
			return true;
		}

		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
			=> this.DocumentTypes.TryGetValue(typeName, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}
	}
}