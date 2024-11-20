using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Plugin.AdbPackageManager.Adb
{
	public class AdbClient
	{
		public String AdbHost { get; private set; }
		public Int32 AdbPort { get; private set; }
		public String DeviceSerialNumber { get; private set; }
		public String AdbPath { get; private set; }

		public AdbClient(String adbPath, String adbHost = "127.0.0.1", Int32 adbPort = 5037)
		{
			if(String.IsNullOrEmpty(adbPath))
				throw new ArgumentNullException(nameof(adbPath));
			if(!File.Exists(adbPath))
				throw new FileNotFoundException("ADB executable not found", adbPath);

			this.AdbHost = adbHost;
			this.AdbPort = adbPort;
			this.AdbPath = adbPath;

			try
			{
				this.Initialize();
			}catch(SocketException)
			{
				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 2, "ADB server not running. Starting executable {0}", adbPath);
				this.StartServer(adbPath);
				this.Initialize();//Бросаем исключение наверх
			}
		}

		/// <summary>Попытка получить серзию запущеннгог сервера</summary>
		private void Initialize()
		{
			Int32 serverVersion = this.GetServerVersion();
			PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "ADB Server: {0}:{1} Version: {2:N0}", this.AdbHost, this.AdbPort, serverVersion);
		}

		public void StartServer(String adbPath)
		{
			Process result = new Process()
			{
				StartInfo = new ProcessStartInfo(adbPath)
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					Arguments = "start-server",
				}
			};

			result.Start();
			result.WaitForExit();
		}

		public void StopServer()
		{
			using(AdbSocket adbSocket = new AdbSocket(this.AdbHost,this.AdbPort))
				adbSocket.SendCommand("host:kill");
		}

		public Int32 GetServerVersion()
		{
			using(AdbSocket adbSocket = new AdbSocket(this.AdbHost,this.AdbPort))
			{
				adbSocket.SendCommand("host:version");
				return adbSocket.ReadInt32Hex();
			}
		}

		public AdbDevice[] GetDevices()
		{
			String response = String.Empty;
			using(AdbSocket adbSocket = new AdbSocket(this.AdbHost,this.AdbPort))
			{
				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbRequest (command): host:devices-l");
				adbSocket.SendCommand("host:devices-l");
				response = adbSocket.ReadHexString();
			}
			PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbResponse (command): {0}", response);

			String[] lines = response.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			List<AdbDevice> devices = new List<AdbDevice>(lines.Length);

			// "b3819a41               device product:a3ultexx model:SM_A300FU device:a3ulte"
			foreach(var line in lines)
			{
				String[] parts = line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				String product = String.Empty;
				String model = String.Empty;
				String device = String.Empty;

				for(Int32 i = 2; i < parts.Length; i++)
				{
					String[] halves = parts[i].Split(new Char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					if(halves.Length == 2)
					{
						switch(halves[0])
						{
						case "product":
							product = halves[1];
							break;
						case "model":
							model = halves[1];
							break;
						case "device":
							device = halves[1];
							break;
						}
					}
				}

				devices.Add(new AdbDevice(parts[0], product, model, device));
			}

			return devices.ToArray();
		}

		public void SetDevice(String serialNumber)
			=> this.DeviceSerialNumber = serialNumber;

		/// <summary>Получить информацию о устройстве</summary>
		/// <returns></returns>
		public Dictionary<String, String> GetDeviceProperties()
		{
			Dictionary<String, String> props = new Dictionary<String, String>();

			String[] lines = this.ExecuteRemoteCommand("/system/bin/getprop");

			foreach(String line in lines)
			{
				// "[persist.sys.dalvik.vm.lib.2]: [libart.so]"
				String[] keyValue = line.Split(':');
				if(keyValue.Length == 2)
					props.Add(keyValue[0], keyValue[1]);
				else
					PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "Invalid property: '{0}'", line);
			}

			return props;
		}

		/// <summary>Выполнить команду на удалённом устройстве</summary>
		/// <param name="command">Команда для выполнения</param>
		/// <returns></returns>
		public String[] ExecuteRemoteCommand(String command)
		{
			using(AdbSocket adbSocket = new AdbSocket(this.AdbHost,this.AdbPort))
			{
				this.SetDevice(adbSocket);

				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbRequest (shell): {0}", command);
				adbSocket.SendCommand(String.Format("shell:{0}", command));

				String[] response = adbSocket.ReadAllLines();
				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbResponse (shell):\r\n\t{0}", String.Join("\r\n\t", response));
				return response;
			}
		}

		/// <summary>Получить информацию о файле</summary>
		/// <param name="fileName">Путь к файлу</param>
		/// <returns>Информация о файле</returns>
		public AdbFileInfo GetFileInfo(String fileName)
		{
			using(AdbSocket adbSocket = new AdbSocket(this.AdbHost,this.AdbPort))
			{
				String response = this.SendSyncCommand(adbSocket, "STAT", "\"" + fileName + "\"");

				if(response != "STAT")
					throw new InvalidOperationException(response);

				return this.GetFileInfo(adbSocket, fileName, null);
			}
		}

		public IEnumerable<AdbFileInfo> GetDirectoryListing()
			=> throw new NotImplementedException();

		/// <summary>Получить список файлов на удалённом устройстве</summary>
		/// <param name="directoryName">Корневая директория</param>
		/// <returns>Файлы и папки</returns>
		public IEnumerable<AdbFileInfo> GetDirectoryListing(String directoryName)
		{
			using(AdbSocket adbSocket = new AdbSocket(this.AdbHost,this.AdbPort))
			{
				String response = this.SendSyncCommand(adbSocket, "LIST", directoryName);

				Boolean realDirectory = false;
				while(true)
				{
					if(response == "DONE")
					{
						if(realDirectory)
							break;
						else
							throw new DirectoryNotFoundException();
					} else if(response != "DENT")
						throw new InvalidOperationException(response);

					AdbFileInfo adbFileInfo = this.GetFileInfo(adbSocket, null, directoryName);
					if(adbFileInfo == null)
						realDirectory = true; // has "." and ".."
					else
						yield return adbFileInfo;

					response = adbSocket.ReadString(4);
				}
			}
		}

		private AdbFileInfo GetFileInfo(AdbSocket adbSocket, String fullName, String directoryName)
		{
			Int32 mode = adbSocket.ReadInt32();
			Int32 size = adbSocket.ReadInt32();
			DateTime time = Utils.FromUnixTime(adbSocket.ReadInt32());

			String name = String.Empty;
			if(String.IsNullOrEmpty(fullName))
			{
				name = adbSocket.ReadSyncString();
				if(name.Equals(".") || name.Equals(".."))
					return null;
				fullName = Utils.CombinePath(directoryName, name);
			} else
				name = Path.GetFileName(fullName);

			return new AdbFileInfo(fullName, name, size, mode, time);
		}

		/// <summary>Загрузить файл с удалённого устройства</summary>
		/// <param name="remoteFileName">Удалённый путь к файлу</param>
		/// <param name="localFileName">Локальный путь к файлу</param>
		public void DownloadFile(String remoteFileName, String localFileName)
		{
			using(AdbSocket adbSocket = new AdbSocket(this.AdbHost,this.AdbPort))
			{
				String response = this.SendSyncCommand(adbSocket, "RECV", remoteFileName);

				Int32 total = 0;
				using(FileStream stream = File.Open(localFileName, FileMode.Create))
				{
					Byte[] bytes = new Byte[65536];
					while(true)
					{
						if(response == "DONE")
							break;
						else if(response != "DATA")
							throw new InvalidOperationException(response);

						Int32 size = adbSocket.ReadInt32();

						adbSocket.Read(bytes, size);
						stream.Write(bytes, 0, size);
						total += size;

						response = adbSocket.ReadString(4);
					}
				}
			}
		}

		/// <summary>Загрузить файл на устройство</summary>
		/// <param name="localFilePath">Локальный путь к файлу</param>
		/// <param name="remoteFilePath">Удалённый путь куда загружаем файл</param>
		/// <param name="remoteFilePermissions">Разрешения для создаваемого файла по умочанию</param>
		public void UploadFile(String localFilePath, String remoteFilePath, Int32 remoteFilePermissions = 0x0666)
		{
			AdbFileInfo adbFileInfo = this.GetFileInfo(remoteFilePath);
			if(adbFileInfo.IsDirectory || adbFileInfo.IsSymbolicLink)
				remoteFilePath = Utils.CombinePath(remoteFilePath, Path.GetFileName(localFilePath));

			using(AdbSocket adbSocket = GetSocket())
			{
				String response = this.SendSyncCommand(adbSocket, "SEND", String.Format("{0},{1}", remoteFilePath, remoteFilePermissions), false);

				FileInfo localFileInfo = new FileInfo(localFilePath);
				Int32 left = (Int32)localFileInfo.Length;

				using(FileStream stream = File.OpenRead(localFilePath))
				{
					Byte[] bytes = new Byte[65536];
					while(left > 0)
					{
						Int32 size = left < bytes.Length ? left : bytes.Length;
						stream.Read(bytes, 0, size);

						adbSocket.WriteString("DATA");
						adbSocket.WriteInt32(size);
						adbSocket.Write(bytes, size);

						left -= size;
					}
				}

				adbSocket.WriteString("DONE");
				adbSocket.WriteInt32(Utils.ToUnixTime(localFileInfo.LastWriteTime));

				response = adbSocket.ReadString(4);

				if(response != "OKAY")
					throw new InvalidOperationException(response);
			}
		}

		private String SendSyncCommand(AdbSocket adbSocket, String command, String parameter, Boolean readResponse = true)
		{
			_ = parameter ?? throw new ArgumentNullException(nameof(parameter));

			this.SetDevice(adbSocket);

			PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbRequest (sync): {0} {1}", command, parameter);
			adbSocket.SendCommand("sync:");

			String response = adbSocket.SendSyncCommand(command, parameter, readResponse);
			PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbResponse (sync): {0}", response);
			return response;
		}

		/// <summary>Удалить существующий файл с файловой системы</summary>
		/// <param name="remoteFileName">Удалённый путь к файлу</param>
		public void DeleteFile(String remoteFileName)
			=> ExecuteRemoteCommand(String.Format("rm -f \"{0}\"", remoteFileName));

		/// <summary>Установка приложения с файловой системы</summary>
		/// <param name="localFilePath">Локальный путь к файлу</param>
		/// <param name="installOnSdCard">Установить на внешнее хранилище</param>
		/// <param name="reinstallExisting">Переустановка существующего пакета с сохранением данных</param>
		public void InstallApplication(String localFilePath, String tempFolderPath, Boolean installOnSdCard, Boolean reinstallExisting)
		{
			// legacy install

			String baseName = Path.GetFileName(localFilePath).Replace(' ', '_');
			String remoteFileName = tempFolderPath + baseName;

			if(this.GetFileInfo(remoteFileName).IsFile)
				this.DeleteFile(remoteFileName);//Удаляем существующий файл

			this.UploadFile(localFilePath, remoteFileName);

			try
			{
				String[] options = new String[]
				{
					installOnSdCard?"-s":String.Empty,
					reinstallExisting?"-r":String.Empty,
				};
				this.ExecutePm(String.Format("install {0} {1}", String.Join(" ", options), remoteFileName));
			} finally
			{
				//this.DeleteFile(remoteFileName);
			}
		}

		public void UploadFile2(String localPath, String remotePath)
		{
			if(!File.Exists(localPath))
				throw new FileNotFoundException("File not found", localPath);
			String arguments = String.Format("-s {0} push \"{1}\" \"{2}\"", this.DeviceSerialNumber, localPath, remotePath);
			PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbRequest {0}", arguments);

			ProcessWrapper processWrapper = new ProcessWrapper(this.AdbPath);
			Int32 exitCode = 0;
			String resultMessage;
			String errorMessage;
			using(Process process = processWrapper.Invoke(arguments))
			{
				process.WaitForExit();
				resultMessage = process.StandardOutput.ReadToEnd();
				errorMessage = process.StandardError.ReadToEnd();
				exitCode = process.ExitCode;
			}

			if(!String.IsNullOrEmpty(resultMessage))
				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbResponse (result): {0}", resultMessage);
			if(!String.IsNullOrEmpty(errorMessage))
			{
				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbResponse (error): {0}", errorMessage);
				throw new Exception(errorMessage);
			}
		}

		/// <summary>Установить Android Package на устройство, в качестве аргумента передаётся путь к локальному файлу</summary>
		/// <param name="reinstallExisting">Обновить/переустановить ранее установленное приложение (Оставить данные)</param>
		public void InstallAndroidPackage(String apkPath, Boolean reinstallExisting = true)
		{
			if(!File.Exists(apkPath))
				throw new FileNotFoundException("Android package not found", apkPath);

			String options = reinstallExisting ? "-r" : String.Empty;

			String arguments = String.Format("-s {0} install {1} \"{2}\"", this.DeviceSerialNumber, options, apkPath);
			PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbRequest: {0}", arguments);

			ProcessWrapper processWrapper = new ProcessWrapper(this.AdbPath);
			Int32 exitCode = 0;
			String resultMessage;
			String errorMessage;
			using(Process process = processWrapper.Invoke(arguments))
			{
				process.WaitForExit();
				resultMessage = process.StandardOutput.ReadToEnd();
				errorMessage = process.StandardError.ReadToEnd();
				exitCode = process.ExitCode;
			}

			if(!String.IsNullOrEmpty(resultMessage))
				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbResponse (result): {0}", resultMessage);
			if(!String.IsNullOrEmpty(errorMessage))
			{
				PluginWindows.Trace.TraceEvent(TraceEventType.Verbose, 1, "AdbResponse (error): {0}", errorMessage);
				switch(exitCode)
				{
				case 0://Success\r\n
					return;
				default:
					throw new ApplicationException(errorMessage);
				}
			}
			//this.ExecutePm(String.Format("install {0} \"{1}\"", options, apkPath));
		}

		/// <summary>Удаление ранее установленного приложения</summary>
		/// <param name="applicationName">Наименование установленного приложения</param>
		/// <param name="keepDataAndCache">Оставить данные приложения</param>
		public void UninstallApplication(String applicationName, Boolean keepDataAndCache = false)
		{
			// legacy uninstall

			String options = keepDataAndCache ? "-k" : String.Empty;
			this.ExecutePm(String.Format("uninstall {0} {1}", options, applicationName));
		}

		/// <summary>Получть список всех установленных приложения</summary>
		/// <returns>Список установленных приложений</returns>
		public IEnumerable<AdbAppInfo> GetApplications()
		{
			String[] response = this.ExecuteRemoteCommand("pm list packages -f");
			foreach(String line in response)
			{
				const String PackageConst = "package:";
				Int32 indexOfConst = line.IndexOf(PackageConst);
				Int32 indexOfPackage = line.LastIndexOf('=');
				if(indexOfConst == -1 || indexOfConst != 0 || indexOfPackage == -1)
					throw new FormatException(line);

				String fileName = line.Substring(PackageConst.Length, indexOfPackage - PackageConst.Length);
				String packageName = line.Substring(indexOfPackage + 1);

				yield return new AdbAppInfo(packageName, fileName);
			}
		}

		private void ExecutePm(String commandLine)
		{
			String[] response = this.ExecuteRemoteCommand(String.Format("pm {0}", commandLine));

			if(response == null || response.Length == 0)
				throw new Exception("Wrong pm output");

			String line = response[response.Length - 1];
			if(line.Equals("Success"))
				return;

			Match match = Regex.Match(line, @"\[(.+?)]");
			throw new Exception(2 == match.Groups.Count ? match.Groups[1].Value : line);
		}

		private void SetDevice(AdbSocket adbSocket)
			=> adbSocket.SendCommand(String.Format("host:transport:{0}", this.DeviceSerialNumber));

		private AdbSocket GetSocket()
			=> new AdbSocket(this.AdbHost, this.AdbPort);
	}
}