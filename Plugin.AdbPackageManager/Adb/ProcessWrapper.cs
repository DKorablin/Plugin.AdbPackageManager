using System;
using System.Diagnostics;

namespace Plugin.AdbPackageManager.Adb
{
	/// <summary>Wraps a process invocation with optional stdout and stderr event forwarding</summary>
	internal class ProcessWrapper
	{
		private readonly String _path;

		/// <summary>Raised when the process writes to standard output</summary>
		public event EventHandler<DataReceivedEventArgs> DataReceived;
		/// <summary>Raised when the process writes to standard error</summary>
		public event EventHandler<DataReceivedEventArgs> ErrorReceived;

		/// <summary>Initializes a new instance with the path to the executable</summary>
		/// <param name="path">Path to the executable to invoke</param>
		public ProcessWrapper(String path)
		{
			//if(!File.Exists(path))
			//	throw new FileNotFoundException("File not found", path);

			this._path = path;
		}

		/// <summary>Starts the process with the given arguments and returns the running Process instance</summary>
		/// <param name="arguments">Command-line arguments to pass to the executable</param>
		/// <returns>The started Process instance</returns>
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