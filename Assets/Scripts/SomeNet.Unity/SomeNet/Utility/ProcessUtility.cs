using System;
using System.Diagnostics;

namespace SomeNet.Utility
{
	public static class ProcessUtility
	{
		public static void RunExe(string exePath, string arguments)
		{
			try
			{
				ProcessStartInfo startInfo = new ProcessStartInfo
				{
					FileName = exePath,
					Arguments = arguments,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				};
				using (Process process = Process.Start(startInfo))
				{
					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();
					process.WaitForExit();
					Console.Write("Output: " + output);
					if (!string.IsNullOrEmpty(error))
					{
						Console.Write("Error: " + error);
					}
				}
			}
			catch (Exception ex)
			{
				Console.Write("Failed to execute exe: " + ex);
			}
		}
	}
}
