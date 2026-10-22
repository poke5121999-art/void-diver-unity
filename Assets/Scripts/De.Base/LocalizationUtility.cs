using System;
using GameCommon.Enum;

public static class LocalizationUtility
{
	private static ELanguage _language;

	public static void SetLanguage(ELanguage language)
	{
	}

	public static bool TryGetLocalizedText(string key, out string text)
	{
		text = null;
		return false;
	}

	public static string GetLocalizedText(string key)
	{
		return null;
	}

	public static Type[] GetLocalizableEnums()
	{
		return null;
	}
}
