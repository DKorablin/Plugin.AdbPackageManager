using System;
using System.Diagnostics;

namespace Plugin.AdbPackageManager.Adb
{
	internal class ProcessWrapper
	{
		private readonly String _path;

		public event EventHandler<DataReceivedEventArgs> DataReceived;
		public event EventHandler<DataReceivedEventArgs> ErrorReceived;

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
			if(this.DataReceived != null)
				process.OutputDataReceived += this.Process_OutputDataReceived;
			if(this.ErrorReceived != null)
				process.ErrorDataReceived += this.Process_ErrorDataReceived;

			process.Start();
			return process;
		}

		private void Process_OutputDataReceived(Object sender, DataReceivedEventArgs e)
		{
			if(e.Data != null)
				this.DataReceived(this, e);
		}

		private void Process_ErrorDataReceived(Object sender, DataReceivedEventArgs e)
		{
			if(e.Data != null)
				this.ErrorReceived(this, e);
		}
	}
}