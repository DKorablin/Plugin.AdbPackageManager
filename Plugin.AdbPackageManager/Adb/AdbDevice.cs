using System;
using System.Diagnostics;

namespace Plugin.AdbPackageManager.Adb
{
	[DebuggerDisplay("SerialNumber={" + nameof(SerialNumber) + "}, Product={" + nameof(Product) + "}, Model={" + nameof(Model) + "}, Device={" + nameof(Device) + "}")]
	/// <summary>Represents an Android device connected via ADB</summary>
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

		/// <summary>Initializes a new instance with the given device identifiers</summary>
		/// <param name="id">Serial number of the device</param>
		/// <param name="product">Product code name</param>
		/// <param name="model">Device model name</param>
		/// <param name="device">Device code name</param>
		public AdbDevice(String id, String product, String model, String device)
		{
			SerialNumber = id;
			Product = product;
			Model = model;
			Device = device;
		}

		/// <summary>Returns a human-readable representation of the device</summary>
		public override String ToString()
			=> $"{this.Model} ({this.Product})";
	}
}