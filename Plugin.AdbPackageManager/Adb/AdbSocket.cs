using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Plugin.AdbPackageManager.Adb
{
	public class AdbSocket : IDisposable
	{
		private TcpClient _tcpClient;
		private NetworkStream _tcpStream;

		private Encoding _encoding = Encoding.ASCII;

		public AdbSocket(String adbHost, Int32 adbPort)
		{
			this._tcpClient = new TcpClient(adbHost, adbPort);
			this._tcpStream = this._tcpClient.GetStream();
		}

		public void Dispose()
		{
			if(this._tcpClient != null)
			{
				this._tcpClient.Close();
				this._tcpClient = null;
			}

			if(this._tcpStream != null)
			{
				this._tcpStream.Close();
				this._tcpStream = null;
			}
		}

		public void Write(Byte[] data, Int32 size)//Plugin.Trace.TraceEvent(TraceEventType.Verbose, 1, "Sending {0} bytes", size);
			=> this._tcpStream.Write(data, 0, size);

		public void Write(Byte[] data)
			=> this.Write(data, data.Length);

		public void WriteString(String text)
		{
			Byte[] buffer = new Byte[65535];
			Int32 size = this._encoding.GetBytes(text, 0, text.Length, buffer, 0);
			this.Write(buffer, size);
		}

		public void WriteInt32(Int32 number)
		{
			Byte[] bytes = BitConverter.GetBytes(number);
			this.Write(bytes);
		}

		public void SendCommand(String command)
		{
			this.WriteString(String.Format("{0:X04}", command.Length));
			this.WriteString(command);

			String response = ReadString(4);
			switch(response)
			{
			case "OKAY":
				return;
			case "FAIL":
				String message = this.ReadHexString();
				throw new ArgumentException(message);
			default:
				if(response.StartsWith("device unauthorized"))
					throw new UnauthorizedAccessException(response);
				else
					throw new InvalidOperationException(response);
			}
		}

		public String SendSyncCommand(String command, String parameter, Boolean readResponse = true)
		{
			this.WriteString(command);
			this.WriteInt32(parameter.Length);
			this.WriteString(parameter);

			if(!readResponse)
				return null;

			String response = this.ReadString(4);
			if(response == "FAIL")
			{
				String message = this.ReadSyncString();
				throw new ArgumentException(message);
			}

			return response;
		}

		public void Read(Byte[] data, Int32 size)
		{
			Int32 total = 0;
			while(total < size)
			{
				Int32 read = this._tcpStream.Read(data, total, size - total);
				total += read;
			}
		}

		public Byte[] Read(Int32 size)
		{
			Byte[] bytes = new Byte[size];
			this.Read(bytes, size);
			return bytes;
		}

		public String ReadHexString()
		{
			Int32 length = this.ReadInt32Hex();
			return this.ReadString(length);
		}

		public String ReadSyncString()
		{
			Int32 length = this.ReadInt32();
			return this.ReadString(length);
		}

		public String ReadString(Int32 length)
		{
			Byte[] buffer = this.Read(length);
			return this._encoding.GetString(buffer, 0, length);
		}

		public Int32 ReadInt32()
		{
			Byte[] buffer = this.Read(4);
			return BitConverter.ToInt32(buffer, 0);
		}

		public Int32 ReadInt32Hex()
		{
			Byte[] buffer = this.Read(4);
			String hex = this._encoding.GetString(buffer, 0, 4);
			return Convert.ToInt32(hex, 16);
		}

		public String[] ReadAllLines()
		{
			List<String> lines = new List<String>();
			using(StreamReader reader = new StreamReader(this._tcpStream, this._encoding))
			{
				while(true)
				{
					String line = reader.ReadLine();

					if(null == line)
						break;

					lines.Add(line.Trim());
				}
			}

			return lines.ToArray();
		}
	}
}