using System;
using System.Collections.Concurrent;
using Serilog.Events;
using UniRx;
using UnityEngine;

namespace NemoLib.Log
{
	public class SeriLogger : MonoBehaviour
	{
		private readonly struct PendingLog
		{
			public readonly LogEventLevel Level;

			public readonly string Message;

			public readonly string SourceContext;

			public PendingLog(LogEventLevel level, string message, string sourceContext)
			{
				Level = LogEventLevel.Verbose;
				Message = null;
				SourceContext = null;
			}
		}

		private const string ExternalCaptureKey = "ExternalCapture";

		private bool _useFileLogger;

		private readonly ConcurrentQueue<PendingLog> _pendingLogs;

		[ThreadStatic]
		private static bool _isEmitting;

		public readonly Subject<string> OnErrorOccured;

		public bool IsClosed { get; private set; }

		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Update()
		{
		}

		public void Init(LogEventLevel unityLogLevel, LogEventLevel fileLogLevel, bool useFileLogger)
		{
		}

		public void Trace(string message = "", Color? color = null)
		{
		}

		public void Debug(Color? color = null)
		{
		}

		public void Debug(string message, Color? color = null)
		{
		}

		public void Info(Color? color = null)
		{
		}

		public void Info(string message, Color? color = null)
		{
		}

		public void Warning(string message = "", Color? color = null)
		{
		}

		public void Error(string message = "", Color? color = null)
		{
		}

		public void Fatal(string message = "", Color? color = null)
		{
		}

		private void Write(LogEventLevel logLevel, string message, Color? color = null)
		{
		}

		private void EmitGuarded(LogEventLevel level, string sourceContext, Color? color, string message, bool external)
		{
		}

		public void EnqueueTrace(string message, string sourceContext = null)
		{
		}

		public void EnqueueDebug(string message, string sourceContext = null)
		{
		}

		public void EnqueueInfo(string message, string sourceContext = null)
		{
		}

		public void EnqueueWarning(string message, string sourceContext = null)
		{
		}

		public void EnqueueError(string message, string sourceContext = null)
		{
		}

		public void EnqueueFatal(string message, string sourceContext = null)
		{
		}

		private void EnqueueLog(LogEventLevel level, string message, string sourceContext = null)
		{
		}

		private void WritePending(PendingLog pending)
		{
		}

		private static string GetSourceContext()
		{
			return null;
		}

		private static string GetHexColor(LogEventLevel level, Color? color)
		{
			return null;
		}

		private static Color GetDefaultColor(LogEventLevel level)
		{
			return default;
		}

		private void OnApplicationQuit()
		{
		}

		private void OnLogReceived(string message, string stackTrace, LogType type)
		{
		}

		private static LogType ToFilterLogType(LogEventLevel level)
		{
			return LogType.Error;
		}

		private static LogEventLevel ToLevel(LogType type)
		{
			return LogEventLevel.Verbose;
		}
	}
}
