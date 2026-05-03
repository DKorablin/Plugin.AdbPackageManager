using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Plugin.AdbPackageManager.Adb
{
	/// <summary>Low-level TCP socket wrapper for the ADB protocol</summary>
	public class AdbSocket : IDisposable
	{
		private TcpClient _tcpClient;
		private NetworkStream _tcpStream;

		private readonly Encoding _encoding = Encoding.ASCII;

		/// <summary>Initializes a new socket connected to the given ADB server</summary>
		/// <param name="adbHost">Host address of the ADB server</param>
		/// <param name="adbPort">Port number of the ADB server</param>
		public AdbSocket(String adbHost, Int32 adbPort)
		{
			this._tcpClient = new TcpClient(adbHost, adbPort);
			this._tcpStream = this._tcpClient.GetStream();
		}

		/// <summary>Releases all resources used by the socket</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases managed resources when disposing</summary>
		/// <param name="disposing">True to release managed resources</param>
		protected virtual void Dispose(Boolean disposing)
		{
			this._tcpClient?.Close();
			this._tcpClient = null;

			this._tcpStream?.Close();
			this._tcpStream = null;
		}

		/// <summary>Destructor to close native handle</summary>
		~AdbSocket()
			=> this.Dispose(false);

		/// <summary>Writes the specified number of bytes to the stream</summary>
		/// <param name="data">Buffer containing the data to write</param>
		/// <param name="size">Number of bytes to write</param>
		public void Write(Byte[] data, Int32 size)
			=> this._tcpStream.Write(data, 0, size);

		/// <summary>Writes all bytes of the given buffer to the stream</summary>
		/// <param name="data">Buffer to write</param>
		public void Write(Byte[] data)
			=> this.Write(data, data.Length);

		/// <summary>Encodes a string as ASCII and writes it to the stream</summary>
		/// <param name="text">String to write</param>
		public void WriteString(String text)
		{
			Byte[] buffer = new Byte[65535];
			Int32 size = this._encoding.GetBytes(text, 0, text.Length, buffer, 0);
			this.Write(buffer, size);
		}

		/// <summary>Writes a 32-bit integer as four bytes in little-endian order</summary>
		/// <param name="number">Integer value to write</param>
		public void WriteInt32(Int32 number)
		{
			Byte[] bytes = BitConverter.GetBytes(number);
			this.Write(bytes);
		}

		/// <summary>Sends an ADB host command and validates the OKAY response</summary>
		/// <param name="command">Command string to send</param>
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

		/// <summary>Sends an ADB sync command with a parameter and optionally reads the response token</summary>
		/// <param name="command">Four-character sync command</param>
		/// <param name="parameter">Command parameter string</param>
		/// <param name="readResponse">Whether to read and return the four-character response token</param>
		/// <returns>Four-character response token, or null when readResponse is false</returns>
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

		/// <summary>Reads exactly the specified number of bytes into the buffer</summary>
		/// <param name="data">Buffer to read into</param>
		/// <param name="size">Number of bytes to read</param>
		public void Read(Byte[] data, Int32 size)
		{
			Int32 total = 0;
			while(total < size)
			{
				Int32 read = this._tcpStream.Read(data, total, size - total);
				total += read;
			}
		}

		/// <summary>Reads exactly the specified number of bytes and returns them as a new array</summary>
		/// <param name="size">Number of bytes to read</param>
		/// <returns>Array containing the read bytes</returns>
		public Byte[] Read(Int32 size)
		{
			Byte[] bytes = new Byte[size];
			this.Read(bytes, size);
			return bytes;
		}

		/// <summary>Reads a length-prefixed hex string from the stream</summary>
		/// <returns>Decoded string value</returns>
		public String ReadHexString()
		{
			Int32 length = this.ReadInt32Hex();
			return this.ReadString(length);
		}

		/// <summary>Reads a sync protocol length-prefixed string from the stream</summary>
		/// <returns>Decoded string value</returns>
		public String ReadSyncString()
		{
			Int32 length = this.ReadInt32();
			return this.ReadString(length);
		}

		/// <summary>Reads the specified number of bytes and decodes them as an ASCII string</summary>
		/// <param name="length">Number of bytes to read</param>
		/// <returns>Decoded string value</returns>
		public String ReadString(Int32 length)
		{
			Byte[] buffer = this.Read(length);
			return this._encoding.GetString(buffer, 0, length);
		}

		/// <summary>Reads four bytes and returns them as a little-endian 32-bit integer</summary>
		/// <returns>Integer value read from the stream</returns>
		public Int32 ReadInt32()
		{
			Byte[] buffer = this.Read(4);
			return BitConverter.ToInt32(buffer, 0);
		}

		/// <summary>Reads a four-character hex string and returns its integer value</summary>
		/// <returns>Integer value parsed from the hex string</returns>
		public Int32 ReadInt32Hex()
		{
			Byte[] buffer = this.Read(4);
			String hex = this._encoding.GetString(buffer, 0, 4);
			return Convert.ToInt32(hex, 16);
		}

		/// <summary>Reads all remaining lines from the stream until the connection closes</summary>
		/// <returns>Array of trimmed lines read from the stream</returns>
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