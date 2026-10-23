using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using UnityEngine;

namespace Serilog.Sinks.Unity3D
{
	public static class UnitySinkExtensions
	{
		private const string DefaultDebugOutputTemplate = "[{Level:u3}] {Message:lj}{NewLine}{Exception}";

		public static LoggerConfiguration Unity3D(this LoggerSinkConfiguration sinkConfiguration, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "[{Level:u3}] {Message:lj}{NewLine}{Exception}", IFormatProvider? formatProvider = null, LoggingLevelSwitch? levelSwitch = null, UnityEngine.ILogger? unityLogger = null)
		{
			if (sinkConfiguration == null)
			{
				throw new ArgumentNullException("sinkConfiguration");
			}
			if (outputTemplate == null)
			{
				throw new ArgumentNullException("outputTemplate");
			}
			MessageTemplateTextFormatter formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
			return sinkConfiguration.Unity3D(formatter, restrictedToMinimumLevel, levelSwitch, unityLogger);
		}

		public static LoggerConfiguration Unity3D(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null, UnityEngine.ILogger? unityLogger = null)
		{
			if (sinkConfiguration == null)
			{
				throw new ArgumentNullException("sinkConfiguration");
			}
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			return sinkConfiguration.Sink(new Unity3DLogEventSink(formatter, unityLogger ?? Debug.unityLogger), restrictedToMinimumLevel, levelSwitch);
		}
	}
}
