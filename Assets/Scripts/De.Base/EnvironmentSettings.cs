using GameCommon.Enum;
using Serilog.Events;
using UnityEngine;

public class EnvironmentSettings : ScriptableObject
{
	public EEnvironment Environment;

	public EPlatform Platform;

	public LogEventLevel UnityLogLevel = LogEventLevel.Debug;

	public bool UseFileLogger;

	public LogEventLevel FileLogLevel = LogEventLevel.Information;

	public bool UseGameAnalytics;

	public bool UsePadFocusCursor;
}
