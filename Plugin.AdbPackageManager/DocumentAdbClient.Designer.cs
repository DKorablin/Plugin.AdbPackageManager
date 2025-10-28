namespace Plugin.AdbPackageManager
{
	partial class DocumentAdbClient
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolStripSeparator tssPackages;
			System.Windows.Forms.ToolStrip tsMain;
			System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentAdbClient));
			this.tsbnOpenAdb = new System.Windows.Forms.ToolStripButton();
			this.tsddlDevices = new System.Windows.Forms.ToolStripComboBox();
			this.tsbnDeviceProperties = new System.Windows.Forms.ToolStripButton();
			this.tsbnDeviceInstall = new System.Windows.Forms.ToolStripButton();
			this.lvInstalled = new AlphaOmega.Windows.Forms.DbListView();
			this.colInstalledName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colInstalledFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmsInstalled = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiInstalledUninstall = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiInstalledApkView = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiInstalledDownload = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiInstalledGooglePlay = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiInstalledHide = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiInstalledShow = new System.Windows.Forms.ToolStripMenuItem();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabPackages = new System.Windows.Forms.TabPage();
			this.gridSearch = new AlphaOmega.Windows.Forms.SearchGrid();
			this.tabShell = new System.Windows.Forms.TabPage();
			this.lvShell = new System.Windows.Forms.ListView();
			this.colShellName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ctrlMessage = new Plugin.AdbPackageManager.UI.MessageCtrl();
			this.bwProcess = new System.ComponentModel.BackgroundWorker();
			tssPackages = new System.Windows.Forms.ToolStripSeparator();
			tsMain = new System.Windows.Forms.ToolStrip();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			tsMain.SuspendLayout();
			this.cmsInstalled.SuspendLayout();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tabPackages.SuspendLayout();
			this.tabShell.SuspendLayout();
			this.SuspendLayout();
			// 
			// tssPackages
			// 
			tssPackages.Name = "tssPackages";
			tssPackages.Size = new System.Drawing.Size(134, 6);
			// 
			// tsMain
			// 
			tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnOpenAdb,
            this.tsddlDevices,
            toolStripSeparator1,
            this.tsbnDeviceProperties,
            this.tsbnDeviceInstall});
			tsMain.Location = new System.Drawing.Point(0, 0);
			tsMain.Name = "tsMain";
			tsMain.Size = new System.Drawing.Size(300, 25);
			tsMain.TabIndex = 0;
			tsMain.Text = "toolStrip1";
			// 
			// tsbnOpenAdb
			// 
			this.tsbnOpenAdb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnOpenAdb.Image = global::Plugin.AdbPackageManager.Properties.Resources.iconOpen;
			this.tsbnOpenAdb.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnOpenAdb.Name = "tsbnOpenAdb";
			this.tsbnOpenAdb.Size = new System.Drawing.Size(23, 22);
			this.tsbnOpenAdb.ToolTipText = "Select adb.exe";
			this.tsbnOpenAdb.Click += new System.EventHandler(this.tsbnOpenAdb_Click);
			// 
			// tsddlDevices
			// 
			this.tsddlDevices.Name = "tsddlDevices";
			this.tsddlDevices.Size = new System.Drawing.Size(121, 25);
			this.tsddlDevices.ToolTipText = "Select connected device";
			this.tsddlDevices.DropDown += new System.EventHandler(this.tsddlDevices_DropDown);
			this.tsddlDevices.SelectedIndexChanged += new System.EventHandler(this.tsddlDevices_SelectedIndexChanged);
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbnDeviceProperties
			// 
			this.tsbnDeviceProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnDeviceProperties.Enabled = false;
			this.tsbnDeviceProperties.Image = ((System.Drawing.Image)(resources.GetObject("tsbnDeviceProperties.Image")));
			this.tsbnDeviceProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnDeviceProperties.Name = "tsbnDeviceProperties";
			this.tsbnDeviceProperties.Size = new System.Drawing.Size(23, 22);
			this.tsbnDeviceProperties.ToolTipText = "Show device properties";
			this.tsbnDeviceProperties.Click += new System.EventHandler(this.tsbnDeviceProperties_Click);
			// 
			// tsbnDeviceInstall
			// 
			this.tsbnDeviceInstall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnDeviceInstall.Enabled = false;
			this.tsbnDeviceInstall.Image = global::Plugin.AdbPackageManager.Properties.Resources.iconOpen;
			this.tsbnDeviceInstall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnDeviceInstall.Name = "tsbnDeviceInstall";
			this.tsbnDeviceInstall.Size = new System.Drawing.Size(23, 22);
			this.tsbnDeviceInstall.Text = "Install";
			this.tsbnDeviceInstall.ToolTipText = "Install Android Package";
			this.tsbnDeviceInstall.Click += new System.EventHandler(this.tsbnDeviceInstall_Click);
			// 
			// lvInstalled
			// 
			this.lvInstalled.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInstalledName,
            this.colInstalledFileName});
			this.lvInstalled.ContextMenuStrip = this.cmsInstalled;
			this.lvInstalled.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvInstalled.FullRowSelect = true;
			this.lvInstalled.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvInstalled.HideSelection = false;
			this.lvInstalled.Location = new System.Drawing.Point(3, 3);
			this.lvInstalled.MultiSelect = false;
			this.lvInstalled.Name = "lvInstalled";
			this.lvInstalled.Size = new System.Drawing.Size(186, 214);
			this.lvInstalled.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvInstalled.TabIndex = 0;
			this.lvInstalled.UseCompatibleStateImageBehavior = false;
			this.lvInstalled.View = System.Windows.Forms.View.Details;
			this.lvInstalled.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvInstalled_DragDrop);
			this.lvInstalled.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvInstalled_DragEnter);
			// 
			// colInstalledName
			// 
			this.colInstalledName.Text = "Name";
			// 
			// colInstalledFileName
			// 
			this.colInstalledFileName.Text = "File Name";
			// 
			// cmsInstalled
			// 
			this.cmsInstalled.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiInstalledUninstall,
            tssPackages,
            this.tsmiInstalledApkView,
            this.tsmiInstalledDownload,
            this.tsmiInstalledGooglePlay,
            this.tsmiInstalledHide,
            this.tsmiInstalledShow});
			this.cmsInstalled.Name = "cmsInstalled";
			this.cmsInstalled.Size = new System.Drawing.Size(138, 142);
			this.cmsInstalled.Opening += new System.ComponentModel.CancelEventHandler(this.cmsInstalled_Opening);
			this.cmsInstalled.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsInstalled_ItemClicked);
			// 
			// tsmiInstalledUninstall
			// 
			this.tsmiInstalledUninstall.Name = "tsmiInstalledUninstall";
			this.tsmiInstalledUninstall.Size = new System.Drawing.Size(137, 22);
			this.tsmiInstalledUninstall.Text = "&Uninstall";
			// 
			// tsmiInstalledApkView
			// 
			this.tsmiInstalledApkView.Name = "tsmiInstalledApkView";
			this.tsmiInstalledApkView.Size = new System.Drawing.Size(137, 22);
			this.tsmiInstalledApkView.Text = "&Apk View";
			// 
			// tsmiInstalledDownload
			// 
			this.tsmiInstalledDownload.Name = "tsmiInstalledDownload";
			this.tsmiInstalledDownload.Size = new System.Drawing.Size(137, 22);
			this.tsmiInstalledDownload.Text = "&Download";
			// 
			// tsmiInstalledGooglePlay
			// 
			this.tsmiInstalledGooglePlay.Name = "tsmiInstalledGooglePlay";
			this.tsmiInstalledGooglePlay.Size = new System.Drawing.Size(137, 22);
			this.tsmiInstalledGooglePlay.Text = "&Google Paly";
			// 
			// tsmiInstalledHide
			// 
			this.tsmiInstalledHide.Name = "tsmiInstalledHide";
			this.tsmiInstalledHide.Size = new System.Drawing.Size(137, 22);
			this.tsmiInstalledHide.Text = "&Hide";
			this.tsmiInstalledHide.Visible = false;
			// 
			// tsmiInstalledShow
			// 
			this.tsmiInstalledShow.Name = "tsmiInstalledShow";
			this.tsmiInstalledShow.Size = new System.Drawing.Size(137, 22);
			this.tsmiInstalledShow.Text = "&Show";
			this.tsmiInstalledShow.Visible = false;
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitMain.Location = new System.Drawing.Point(0, 25);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.tabMain);
			this.splitMain.Panel1.Controls.Add(this.ctrlMessage);
			this.splitMain.Size = new System.Drawing.Size(300, 275);
			this.splitMain.SplitterDistance = 200;
			this.splitMain.TabIndex = 4;
			this.splitMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.splitMain_MouseDoubleClick);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabPackages);
			this.tabMain.Controls.Add(this.tabShell);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 29);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(200, 246);
			this.tabMain.TabIndex = 2;
			// 
			// tabPackages
			// 
			this.tabPackages.Controls.Add(this.gridSearch);
			this.tabPackages.Controls.Add(this.lvInstalled);
			this.tabPackages.Location = new System.Drawing.Point(4, 22);
			this.tabPackages.Name = "tabPackages";
			this.tabPackages.Padding = new System.Windows.Forms.Padding(3);
			this.tabPackages.Size = new System.Drawing.Size(192, 220);
			this.tabPackages.TabIndex = 0;
			this.tabPackages.Text = "Packages";
			this.tabPackages.UseVisualStyleBackColor = true;
			// 
			// gridSearch
			// 
			this.gridSearch.DataGrid = null;
			this.gridSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.gridSearch.EnableFindCase = true;
			this.gridSearch.EnableFindHighlight = true;
			this.gridSearch.EnableFindPrevNext = true;
			this.gridSearch.EnableSearchHighlight = false;
			this.gridSearch.ListView = null;
			this.gridSearch.Location = new System.Drawing.Point(3, 155);
			this.gridSearch.Name = "gridSearch";
			this.gridSearch.Size = new System.Drawing.Size(440, 29);
			this.gridSearch.TabIndex = 1;
			this.gridSearch.TreeView = null;
			this.gridSearch.Visible = false;
			// 
			// tabShell
			// 
			this.tabShell.Controls.Add(this.lvShell);
			this.tabShell.Location = new System.Drawing.Point(4, 22);
			this.tabShell.Name = "tabShell";
			this.tabShell.Padding = new System.Windows.Forms.Padding(3);
			this.tabShell.Size = new System.Drawing.Size(192, 220);
			this.tabShell.TabIndex = 1;
			this.tabShell.Text = "Shell";
			this.tabShell.UseVisualStyleBackColor = true;
			// 
			// lvShell
			// 
			this.lvShell.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colShellName});
			this.lvShell.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvShell.FullRowSelect = true;
			this.lvShell.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvShell.HideSelection = false;
			this.lvShell.Location = new System.Drawing.Point(3, 3);
			this.lvShell.Name = "lvShell";
			this.lvShell.Size = new System.Drawing.Size(186, 214);
			this.lvShell.TabIndex = 0;
			this.lvShell.UseCompatibleStateImageBehavior = false;
			this.lvShell.View = System.Windows.Forms.View.Details;
			// 
			// colShellName
			// 
			this.colShellName.Text = "Name";
			// 
			// ctrlMessage
			// 
			this.ctrlMessage.BackColor = System.Drawing.Color.LightCyan;
			this.ctrlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ctrlMessage.Dock = System.Windows.Forms.DockStyle.Top;
			this.ctrlMessage.Location = new System.Drawing.Point(0, 0);
			this.ctrlMessage.Name = "ctrlMessage";
			this.ctrlMessage.Size = new System.Drawing.Size(200, 29);
			this.ctrlMessage.TabIndex = 0;
			this.ctrlMessage.Visible = false;
			// 
			// bwProcess
			// 
			this.bwProcess.WorkerReportsProgress = true;
			this.bwProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwProcess_DoWork);
			this.bwProcess.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwProcess_ProgressChanged);
			this.bwProcess.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwProcess_RunWorkerCompleted);
			// 
			// DocumentAdbClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Controls.Add(tsMain);
			this.Name = "DocumentAdbClient";
			this.Size = new System.Drawing.Size(300, 300);
			tsMain.ResumeLayout(false);
			tsMain.PerformLayout();
			this.cmsInstalled.ResumeLayout(false);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.tabMain.ResumeLayout(false);
			this.tabPackages.ResumeLayout(false);
			this.tabShell.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ToolStripComboBox tsddlDevices;
		private AlphaOmega.Windows.Forms.DbListView lvInstalled;
		private System.Windows.Forms.ColumnHeader colInstalledName;
		private System.Windows.Forms.ColumnHeader colInstalledFileName;
		private System.Windows.Forms.ContextMenuStrip cmsInstalled;
		private System.Windows.Forms.ToolStripMenuItem tsmiInstalledUninstall;
		private System.Windows.Forms.ToolStripMenuItem tsmiInstalledApkView;
		private System.Windows.Forms.ToolStripMenuItem tsmiInstalledDownload;
		private System.Windows.Forms.ToolStripMenuItem tsmiInstalledGooglePlay;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ToolStripButton tsbnOpenAdb;
		private System.Windows.Forms.ToolStripButton tsbnDeviceProperties;
		private AlphaOmega.Windows.Forms.SearchGrid gridSearch;
		private UI.MessageCtrl ctrlMessage;
		private System.ComponentModel.BackgroundWorker bwProcess;
		private System.Windows.Forms.ToolStripButton tsbnDeviceInstall;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabPackages;
		private System.Windows.Forms.TabPage tabShell;
		private System.Windows.Forms.ListView lvShell;
		private System.Windows.Forms.ColumnHeader colShellName;
		private System.Windows.Forms.ToolStripMenuItem tsmiInstalledHide;
		private System.Windows.Forms.ToolStripMenuItem tsmiInstalledShow;
	}
}
