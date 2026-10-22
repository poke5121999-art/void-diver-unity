using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.Unity3D;
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
				Level = level;
				Message = message;
				SourceContext = sourceContext;
			}
		}

		private const string ExternalCaptureKey = "ExternalCapture";

		private bool _useFileLogger;

		private readonly ConcurrentQueue<PendingLog> _pendingLogs = new ConcurrentQueue<PendingLog>();

		[ThreadStatic]
		private static bool _isEmitting;

		public readonly Subject<string> OnErrorOccured = new Subject<string>();

		public bool IsClosed { get; private set; }

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}

		private void OnEnable()
		{
			Application.logMessageReceived += OnLogReceived;
		}

		private void OnDisable()
		{
			Application.logMessageReceived -= OnLogReceived;
		}

		private void Update()
		{
			PendingLog pending;
			while (_pendingLogs.TryDequeue(out pending))
			{
				WritePending(pending);
			}
		}

		public void Init(LogEventLevel unityLogLevel, LogEventLevel fileLogLevel, bool useFileLogger)
		{
			_useFileLogger = useFileLogger;
			UnityEngine.Debug.Log("Build Version: " + Application.version);
			UnityEngine.Debug.unityLogger.filterLogType = ToFilterLogType(unityLogLevel);
			string unityOutputTemplate = "<color=#{HexColor}>[{Level:u3}] {SourceContext}</color> {Message:lj}{NewLine}{Exception}";
			LoggerConfiguration configuration = new LoggerConfiguration().MinimumLevel.Debug().Enrich.FromLogContext();
			configuration = configuration.WriteTo.Logger(delegate(LoggerConfiguration lc)
			{
				lc.Filter.ByExcluding(e => e.Properties.ContainsKey(ExternalCaptureKey))
					.WriteTo.Unity3D(outputTemplate: unityOutputTemplate)
					.MinimumLevel.Is(unityLogLevel);
			});
			if (useFileLogger)
			{
				string logFilePath = "./Logs/GameLog_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
				Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
				string fileOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}";
				configuration.WriteTo.Logger(delegate(LoggerConfiguration lc)
				{
					lc.WriteTo.File(logFilePath, outputTemplate: fileOutputTemplate, fileSizeLimitBytes: 1073741824L, rollingInterval: RollingInterval.Hour, retainedFileCountLimit: 31)
						.MinimumLevel.Is(fileLogLevel);
				});
			}
			Serilog.Log.Logger = configuration.CreateLogger();
		}

		public void Trace(string message = "", Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Verbose, GetSourceContext(), color, message, external: false);
			}
		}

		public void Debug(Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Debug, GetSourceContext(), color, "", external: false);
			}
		}

		public void Debug(string message, Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Debug, GetSourceContext(), color, message, external: false);
			}
		}

		public void Info(Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Information, GetSourceContext(), color, "", external: false);
			}
		}

		public void Info(string message, Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Information, GetSourceContext(), color, message, external: false);
			}
		}

		public void Warning(string message = "", Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Warning, GetSourceContext(), color, message, external: false);
			}
		}

		public void Error(string message = "", Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Error, GetSourceContext(), color, message, external: false);
				OnErrorOccured.OnNext(message);
			}
		}

		public void Fatal(string message = "", Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(LogEventLevel.Fatal, GetSourceContext(), color, message, external: false);
				OnErrorOccured.OnNext(message);
			}
		}

		private void Write(LogEventLevel logLevel, string message, Color? color = null)
		{
			if (!IsClosed)
			{
				EmitGuarded(logLevel, GetSourceContext(), color, message, external: false);
				if (logLevel > LogEventLevel.Warning)
				{
					OnErrorOccured.OnNext(message);
				}
			}
		}

		private void EmitGuarded(LogEventLevel level, string sourceContext, Color? color, string message, bool external)
		{
			IDisposable externalProperty = null;
			IDisposable sourceProperty = null;
			IDisposable colorProperty = null;
			try
			{
				if (external)
				{
					externalProperty = LogContext.PushProperty(ExternalCaptureKey, true);
				}
				_isEmitting = true;
				sourceProperty = LogContext.PushProperty("SourceContext", sourceContext);
				colorProperty = LogContext.PushProperty("HexColor", GetHexColor(level, color));
				Serilog.Log.Write(level, "{LogText}", message);
			}
			finally
			{
				colorProperty?.Dispose();
				sourceProperty?.Dispose();
				externalProperty?.Dispose();
				_isEmitting = false;
			}
		}

		public void EnqueueTrace(string message, string sourceContext = null)
		{
			EnqueueLog(LogEventLevel.Verbose, message, sourceContext);
		}

		public void EnqueueDebug(string message, string sourceContext = null)
		{
			EnqueueLog(LogEventLevel.Debug, message, sourceContext);
		}

		public void EnqueueInfo(string message, string sourceContext = null)
		{
			EnqueueLog(LogEventLevel.Information, message, sourceContext);
		}

		public void EnqueueWarning(string message, string sourceContext = null)
		{
			EnqueueLog(LogEventLevel.Warning, message, sourceContext);
		}

		public void EnqueueError(string message, string sourceContext = null)
		{
			EnqueueLog(LogEventLevel.Error, message, sourceContext);
		}

		public void EnqueueFatal(string message, string sourceContext = null)
		{
			EnqueueLog(LogEventLevel.Fatal, message, sourceContext);
		}

		private void EnqueueLog(LogEventLevel level, string message, string sourceContext = null)
		{
			if (!IsClosed)
			{
				_pendingLogs.Enqueue(new PendingLog(level, message, sourceContext));
			}
		}

		private void WritePending(PendingLog pending)
		{
			if (!IsClosed)
			{
				string sourceContext = (string.IsNullOrEmpty(pending.SourceContext) ? "[BackgroundThread]" : pending.SourceContext);
				EmitGuarded(pending.Level, sourceContext, null, pending.Message, external: false);
			}
		}

		private static string GetSourceContext()
		{
			MethodBase method = new StackTrace().GetFrame(3)?.GetMethod();
			Type declaringType = method?.DeclaringType;
			string typeName = declaringType?.Name;
			string methodName = method?.Name;
			if (typeName != null && methodName != null)
			{
				return "[" + typeName + "] " + methodName;
			}
			return "Unknown";
		}

		private static string GetHexColor(LogEventLevel level, Color? color)
		{
			return ColorUtility.ToHtmlStringRGBA(color ?? GetDefaultColor(level));
		}

		private static Color GetDefaultColor(LogEventLevel level)
		{
			switch (level)
			{
			case LogEventLevel.Verbose:
			case LogEventLevel.Debug:
				return Color.gray;
			case LogEventLevel.Warning:
				return new Color(1f, 0.92156863f, 0.015686275f, 1f);
			case LogEventLevel.Error:
			case LogEventLevel.Fatal:
				return new Color(1f, 0f, 0f, 1f);
			default:
				return new Color(1f, 1f, 1f, 1f);
			}
		}

		private void OnApplicationQuit()
		{
			IsClosed = true;
			Serilog.Log.CloseAndFlush();
		}

		private void OnLogReceived(string message, string stackTrace, LogType type)
		{
			if (_isEmitting || !_useFileLogger || IsClosed)
			{
				return;
			}
			LogEventLevel level = ToLevel(type);
			if (type != LogType.Log)
			{
				message = message + "\n" + stackTrace;
			}
			EmitGuarded(level, "[Unity]", null, message, external: true);
		}

		private static LogType ToFilterLogType(LogEventLevel level)
		{
			if (level == LogEventLevel.Warning)
			{
				return LogType.Warning;
			}
			if (level == LogEventLevel.Error || level == LogEventLevel.Fatal)
			{
				return LogType.Error;
			}
			return LogType.Log;
		}

		private static LogEventLevel ToLevel(LogType type)
		{
			switch (type)
			{
			case LogType.Warning:
				return LogEventLevel.Warning;
			case LogType.Log:
				return LogEventLevel.Debug;
			default:
				return LogEventLevel.Error;
			}
		}
	}
}
