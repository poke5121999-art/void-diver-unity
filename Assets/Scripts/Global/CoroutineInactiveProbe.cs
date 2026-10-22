using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

public static class CoroutineInactiveProbe
{
	private const string TargetMessage = "Coroutine couldn't be started";

	private static readonly HashSet<string> _loggedObjects = new HashSet<string>();

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Install()
	{
		Application.logMessageReceived -= OnLogMessageReceived;
		Application.logMessageReceived += OnLogMessageReceived;
	}

	private static void OnLogMessageReceived(string message, string stackTrace, LogType type)
	{
		if (string.IsNullOrEmpty(message) || message.IndexOf(TargetMessage, System.StringComparison.Ordinal) < 0)
		{
			return;
		}
		string text = ExtractObjectName(message);
		if (_loggedObjects.Add(text))
		{
			EmitDeferred(text, System.Environment.StackTrace).Forget();
		}
	}

	private static async UniTaskVoid EmitDeferred(string objectName, string managedStack)
	{
		await UniTask.Yield();
		UnityEngine.Debug.LogError("[CoroutineInactiveProbe] inactive coroutine host '" + objectName + "'\n--- managed stack (StartCoroutine caller) ---\n" + managedStack);
	}

	private static string ExtractObjectName(string message)
	{
		int num = message.IndexOf('\'');
		if (num >= 0)
		{
			int num2 = message.IndexOf('\'', num + 1);
			if (num2 >= 0)
			{
				return message.Substring(num + 1, num2 - num - 1);
			}
		}
		return message;
	}
}
