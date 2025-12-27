namespace Plugin.AdbPackageManager.UI
{
	partial class DevicePropertiesCtrl
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
			this.lvProperties = new System.Windows.Forms.ListView();
			this.colPropertyKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colPropertyValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// lvProperties
			// 
			this.lvProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPropertyKey,
            this.colPropertyValue});
			this.lvProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvProperties.FullRowSelect = true;
			this.lvProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvProperties.HideSelection = false;
			this.lvProperties.Location = new System.Drawing.Point(0, 0);
			this.lvProperties.Name = "lvProperties";
			this.lvProperties.Size = new System.Drawing.Size(150, 150);
			this.lvProperties.TabIndex = 0;
			this.lvProperties.UseCompatibleStateImageBehavior = false;
			this.lvProperties.View = System.Windows.Forms.View.Details;
			// 
			// colPropertyKey
			// 
			this.colPropertyKey.Text = "Key";
			// 
			// colPropertyValue
			// 
			this.colPropertyValue.Text = "Value";
			// 
			// DevicePropertiesCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lvProperties);
			this.Name = "DevicePropertiesCtrl";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvProperties;
		private System.Windows.Forms.ColumnHeader colPropertyKey;
		private System.Windows.Forms.ColumnHeader colPropertyValue;
	}
}
