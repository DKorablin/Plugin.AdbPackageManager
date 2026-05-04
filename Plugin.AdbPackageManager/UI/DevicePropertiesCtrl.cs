using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Plugin.AdbPackageManager.UI
{
	/// <summary>Panel control for displaying Android device property key-value pairs</summary>
	internal partial class DevicePropertiesCtrl : UserControl
	{
		/// <summary>Initializes the control</summary>
		public DevicePropertiesCtrl()
			=> this.InitializeComponent();

		/// <summary>Populates or updates the list with the given device properties</summary>
		/// <param name="properties">Dictionary of property key-value pairs to display</param>
		public void ShowProperties(Dictionary<String,String> properties)
		{
			List<ListViewItem> items = new List<ListViewItem>();
			foreach(var item in properties)
			{
				Boolean isExists = false;
				foreach(ListViewItem listItem in lvProperties.Items)
					if(listItem.SubItems[colPropertyKey.Index].Text == item.Key)
					{
						listItem.SubItems[colPropertyValue.Index].Text = item.Value;
						isExists = true;
						break;
					}
				if(!isExists)
					items.Add(new ListViewItem(new String[] { item.Key, item.Value, }));
			}
			if(items.Count > 0)
			{
				lvProperties.Items.AddRange(items.ToArray());
				lvProperties.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}
	}
}