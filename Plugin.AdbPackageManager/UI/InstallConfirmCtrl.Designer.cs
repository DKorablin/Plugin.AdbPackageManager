namespace Plugin.AdbPackageManager.UI
{
	partial class InstallConfirmCtrl
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
			System.Windows.Forms.Label lblTitle;
			System.Windows.Forms.Label label2;
			this.bnCancel = new System.Windows.Forms.Button();
			this.bnOk = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPackage = new System.Windows.Forms.TextBox();
			this.clbResources = new System.Windows.Forms.CheckedListBox();
			lblTitle = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			lblTitle.AutoSize = true;
			lblTitle.Location = new System.Drawing.Point(4, 4);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new System.Drawing.Size(192, 13);
			lblTitle.TabIndex = 2;
			lblTitle.Text = "Confirm package && resource installation";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label2.Location = new System.Drawing.Point(4, 58);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(71, 13);
			label2.TabIndex = 5;
			label2.Text = "&Resources:";
			// 
			// bnCancel
			// 
			this.bnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bnCancel.Location = new System.Drawing.Point(203, 243);
			this.bnCancel.Name = "bnCancel";
			this.bnCancel.Size = new System.Drawing.Size(75, 23);
			this.bnCancel.TabIndex = 1;
			this.bnCancel.Text = "&Cancel";
			this.bnCancel.UseVisualStyleBackColor = true;
			this.bnCancel.Click += new System.EventHandler(this.bnCancel_Click);
			// 
			// bnOk
			// 
			this.bnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bnOk.Location = new System.Drawing.Point(122, 243);
			this.bnOk.Name = "bnOk";
			this.bnOk.Size = new System.Drawing.Size(75, 23);
			this.bnOk.TabIndex = 0;
			this.bnOk.Text = "&Ok";
			this.bnOk.UseVisualStyleBackColor = true;
			this.bnOk.Click += new System.EventHandler(this.bnOk_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(4, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "&Package:";
			// 
			// txtPackage
			// 
			this.txtPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPackage.BackColor = System.Drawing.SystemColors.Control;
			this.txtPackage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtPackage.Location = new System.Drawing.Point(74, 32);
			this.txtPackage.Name = "txtPackage";
			this.txtPackage.ReadOnly = true;
			this.txtPackage.Size = new System.Drawing.Size(204, 13);
			this.txtPackage.TabIndex = 4;
			// 
			// clbResources
			// 
			this.clbResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.clbResources.FormattingEnabled = true;
			this.clbResources.Location = new System.Drawing.Point(7, 74);
			this.clbResources.Name = "clbResources";
			this.clbResources.Size = new System.Drawing.Size(271, 139);
			this.clbResources.TabIndex = 6;
			// 
			// InstallConfirmCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.clbResources);
			this.Controls.Add(label2);
			this.Controls.Add(this.txtPackage);
			this.Controls.Add(this.label1);
			this.Controls.Add(lblTitle);
			this.Controls.Add(this.bnOk);
			this.Controls.Add(this.bnCancel);
			this.MinimumSize = new System.Drawing.Size(200, 141);
			this.Name = "InstallConfirmCtrl";
			this.Size = new System.Drawing.Size(281, 269);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button bnCancel;
		private System.Windows.Forms.Button bnOk;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPackage;
		private System.Windows.Forms.CheckedListBox clbResources;
	}
}
