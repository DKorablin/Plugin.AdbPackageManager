using System;
using System.Diagnostics;

namespace Plugin.AdbPackageManager.Adb
{
	[DebuggerDisplay("SerialNumber={" + nameof(SerialNumber) + "}, Product={" + nameof(Product) + "}, Model={" + nameof(Model) + "}, Device={" + nameof(Device) + "}")]
	public class AdbDevice
	{
		/// <summary>Серийный номер устройства</summary>
		public String SerialNumber { get; private set; }

		/// <summary>Кодование наименование устройства</summary>
		public String Product { get; private set; }

		/// <summary>Модель устройства</summary>
		public String Model { get; private set; }

		/// <summary>Устройство (?)</summary>
		public String Device { get; private set; }

		public AdbDevice(String id, String product, String model, String device)
		{
			SerialNumber = id;
			Product = product;
			Model = model;
			Device = device;
		}

		public override String ToString()
			=> $"{this.Model} ({this.Product})";
	}
}