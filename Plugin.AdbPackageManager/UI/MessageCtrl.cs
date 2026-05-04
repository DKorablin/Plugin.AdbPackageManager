using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.AdbPackageManager.UI
{
	/// <summary>Status bar control for displaying progress and result messages</summary>
	internal partial class MessageCtrl : UserControl
	{
		/// <summary>Severity level of a status message</summary>
		public enum StatusMessageType
		{
			/// <summary>Operation completed successfully</summary>
			Success = 0,
			/// <summary>Operation is in progress</summary>
			Progress = 1,
			/// <summary>Operation failed</summary>
			Failed = 2,
		}

		private static readonly Color[] StatusMessageColor = new Color[] { Color.LightCyan, Color.AntiqueWhite, Color.Pink, };

		/// <summary>Initializes the control in hidden state</summary>
		public MessageCtrl()
		{
			InitializeComponent();
			this.Visible = false;
		}

		/// <summary>Displays a status message with the corresponding background color</summary>
		/// <param name="type">Severity level determining the background color</param>
		/// <param name="message">Message text; pass null to hide the control</param>
		public void ShowMessage(StatusMessageType type, String message)
		{
			if(message == null)
				this.Visible = false;
			else
			{
				this.Visible = true;
				base.BackColor = MessageCtrl.StatusMessageColor[(Int32)type];
				lblMessage.Text = message;
			}
		}

		private void bnClose_MouseHover(Object sender, EventArgs e)
			=> bnClose.ImageIndex = 1;

		private void bnClose_MouseLeave(Object sender, EventArgs e)
			=> bnClose.ImageIndex = 0;

		private void bnClose_Click(Object sender, EventArgs e)
			=> this.Visible = false;
	}
}