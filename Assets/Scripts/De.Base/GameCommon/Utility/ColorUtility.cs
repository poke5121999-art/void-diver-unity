using UnityEngine;

namespace GameCommon.Utility
{
	public static class ColorUtility
	{
		public static Color ParseOrWhite(string colorCode)
		{
			if (!UnityEngine.ColorUtility.TryParseHtmlString(colorCode, out Color color))
			{
				return Color.white;
			}
			return color;
		}

		public static Color ParseOrDefault(string colorCode, Color defaultColor)
		{
			if (!UnityEngine.ColorUtility.TryParseHtmlString(colorCode, out Color color))
			{
				return defaultColor;
			}
			return color;
		}
	}
}
