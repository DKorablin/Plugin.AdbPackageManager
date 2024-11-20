using System;
using System.Diagnostics;

namespace Plugin.AdbPackageManager.Adb
{
	internal class ProcessWrapper
	{
		private readonly String _path;

		public event EventHandler<DataReceivedEventArgs> DataRecieved;
		public event EventHandler<DataReceivedEventArgs> ErrorRecieved;

		public ProcessWrapper(String path)
		{
			//if(!File.Exists(path))
			//	throw new FileNotFoundException("File not found", path);

			this._path = path;
		}

		public Process Invoke(String arguments)
		{
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo(this._path)
				{
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					Arguments = arguments,
				}
			};
			if(this.DataRecieved != null)
				process.OutputDataReceived += Process_OutputDataReceived;
			if(this.ErrorRecieved != null)
				process.ErrorDataReceived += Process_ErrorDataReceived;

			process.Start();
			return process;
		}

		private void Process_OutputDataReceived(Object sender, DataReceivedEventArgs e)
		{
			if(e.Data != null)
				this.DataRecieved(this, e);
		}

		private void Process_ErrorDataReceived(Object sender, DataReceivedEventArgs e)
		{
			if(e.Data != null)
				this.ErrorRecieved(this, e);
		}
	}
}