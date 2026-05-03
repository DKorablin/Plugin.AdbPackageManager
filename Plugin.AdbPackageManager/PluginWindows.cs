using System;
using System.Collections.Generic;
using System.Diagnostics;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.AdbPackageManager
{
	/// <summary>Plugin entry point that integrates the ADB package manager into the host application</summary>
	public class PluginWindows : IPlugin, IPluginSettings<PluginSettings>
	{
		private PluginSettings _settings;
		private Dictionary<String, DockState> _documentTypes;

		/// <summary>Host windows interface provided by the application</summary>
		internal IHostWindows HostWindows { get; }

		/// <summary>Trace source for diagnostic logging</summary>
		internal static ITraceSource Trace { get; private set; }

		/// <summary>Settings for interaction from the host</summary>
		Object IPluginSettings.Settings => this.Settings;

		/// <summary>Settings for interaction from the plugin</summary>
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

		/// <summary>Menu item added to the Tools menu for opening the ADB client window</summary>
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

		/// <summary>Initializes a new plugin instance with the given host and trace source</summary>
		/// <param name="hostWindows">Host windows interface</param>
		/// <param name="trace">Trace source for diagnostic logging</param>
		public PluginWindows(IHostWindows hostWindows, ITraceSource trace)
		{
			this.HostWindows = hostWindows ?? throw new ArgumentNullException(nameof(hostWindows));
			Trace = trace ?? throw new ArgumentNullException(nameof(trace));
		}

		/// <summary>Creates and returns the plugin window control for the given type name</summary>
		/// <param name="typeName">Full type name of the window to create</param>
		/// <param name="args">Optional arguments passed to the window</param>
		/// <returns>Created window instance, or null if the type is not registered</returns>
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
					this.AdbClientMenu.Click += (sender, e) => this.CreateWindow(typeof(DocumentAdbClient).ToString(), true);
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

		/// <summary>Creates or activates a window of the given type in the host environment</summary>
		/// <param name="typeName">Full type name of the window</param>
		/// <param name="searchForOpened">True to reuse an already-open window</param>
		/// <param name="args">Optional arguments passed to the window</param>
		/// <returns>The window instance, or null if the type is not registered</returns>
		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
			=> this.DocumentTypes.TryGetValue(typeName, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;
	}
}