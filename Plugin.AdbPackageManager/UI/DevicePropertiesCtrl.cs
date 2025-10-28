using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Plugin.AdbPackageManager.UI
{
	internal partial class DevicePropertiesCtrl : UserControl
	{
		public DevicePropertiesCtrl()
			=> this.InitializeComponent();

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