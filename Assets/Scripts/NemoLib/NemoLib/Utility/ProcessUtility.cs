using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace NemoLib.Utility
{
	public static class ProcessUtility
	{
		public static void RunExe(string exePath, string arguments = null)
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
					CreateNoWindow = true,
					WorkingDirectory = Path.GetDirectoryName(exePath)
				};
				Process process = Process.Start(startInfo);
				string output = process.StandardOutput.ReadToEnd();
				string error = process.StandardError.ReadToEnd();
				process.WaitForExit();
				UnityEngine.Debug.Log("Output: " + output);
				if (!string.IsNullOrEmpty(error))
				{
					UnityEngine.Debug.LogError("Error: " + error);
				}
			}
			catch (Exception ex)
			{
				// TODO(decomp): catch body is reconstructed from the disassembly (string "Failed to execute exe: " + Message; the log call is COMDAT-folded, LogError assumed) @0x182d162a0
				UnityEngine.Debug.LogError("Failed to execute exe: " + ex.Message);
			}
		}
	}
}
