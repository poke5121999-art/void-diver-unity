using System;
using System.Globalization;

public static class SystemExtensions
{
	public static string ToTimerString(this float time)
	{
		int minutes = (int)Math.Floor(time / 60f);
		int seconds = (int)Math.Floor(time % 60f);
		int centiseconds = (int)Math.Floor((time - UnityEngine.Mathf.Floor(time)) * 100f);
		return string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, centiseconds);
	}

	public static string ToNumberString(this long number)
	{
		return number.ToString("N0", CultureInfo.InvariantCulture) ?? string.Empty;
	}

	public static string ToNumberString(this int number)
	{
		return number.ToString("N0", CultureInfo.InvariantCulture) ?? string.Empty;
	}

	public static string ToFloatString(this float value)
	{
		if (value % 1f == 0f)
		{
			return string.Format("{0}", (int)value);
		}
		return value.ToString("0.##");
	}
}
