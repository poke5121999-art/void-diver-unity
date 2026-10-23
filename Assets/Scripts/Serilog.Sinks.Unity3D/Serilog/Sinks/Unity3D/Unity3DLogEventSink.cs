using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using UnityEngine;

namespace Serilog.Sinks.Unity3D
{
	public sealed class Unity3DLogEventSink : ILogEventSink
	{
		private readonly ITextFormatter _formatter;

		private readonly UnityEngine.ILogger _unityLogger;

		public Unity3DLogEventSink(ITextFormatter formatter, UnityEngine.ILogger unityLogger)
		{
			_formatter = formatter;
			_unityLogger = unityLogger;
		}

		public void Emit(LogEvent logEvent)
		{
			using StringWriter buffer = new StringWriter();
			_formatter.Format(logEvent, buffer);
			LogType logType = logEvent.Level switch
			{
				LogEventLevel.Verbose => LogType.Log,
				LogEventLevel.Debug => LogType.Log,
				LogEventLevel.Information => LogType.Log,
				LogEventLevel.Warning => LogType.Warning,
				LogEventLevel.Error => LogType.Error,
				LogEventLevel.Fatal => LogType.Error,
				_ => throw new ArgumentOutOfRangeException("Level", "Unknown log level"),
			};
			object message = buffer.ToString().Trim();
			UnityEngine.Object context = null;
			if (logEvent.Properties.TryGetValue(UnityObjectEnricher.UnityContextKey, out LogEventPropertyValue contextValue) && contextValue is ScalarValue contextScalar && contextScalar.Value is UnityEngine.Object contextObject)
			{
				context = contextObject;
			}
			string tag = null;
			if (logEvent.Properties.TryGetValue(UnityTagEnricher.UnityTagKey, out LogEventPropertyValue tagValue) && tagValue is ScalarValue tagScalar && tagScalar.Value is string tagString)
			{
				tag = tagString;
			}
			if (context != null)
			{
				if (tag != null)
				{
					_unityLogger.Log(logType, tag, message, context);
				}
				else
				{
					_unityLogger.Log(logType, message, context);
				}
			}
			else if (tag != null)
			{
				_unityLogger.Log(logType, tag, message);
			}
			else
			{
				_unityLogger.Log(logType, message);
			}
		}
	}
}
