using System;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

// Web player quality tier, not in the original (2026-11-05, owner: playable on weak PCs and PCs without a graphics
// card). The game's own quality levels do the work: the low tier is EQualityLevel.Low (GraphicOptionController adds
// web-only cuts to it), the high tier is the level the game picks today. Chosen by GraphicSettingService:
//   ?quality=low|medium|high|ultra in the page URL forces a level (a manual choice);
//   first run: a software renderer (no GPU) gets Low, anything else the game's default;
//   while the choice is automatic, a measured frame rate under SlowFps drops it to Low once;
//   choosing a level in Options makes it manual for good.
// Elsewhere (editor, Standalone) nothing here runs.
public static class WebQualityTier
{
	public const string AutoPrefKey = "VD.WebQualityAuto";

	public const float SlowFps = 22f;

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern string VdGpuRenderer();
#endif

	private static string s_renderer;

	public static string Renderer
	{
		get
		{
			if (s_renderer == null)
			{
#if UNITY_WEBGL && !UNITY_EDITOR
				try
				{
					s_renderer = VdGpuRenderer() ?? string.Empty;
				}
				catch (Exception e)
				{
					s_renderer = "unknown (" + e.Message + ")";
				}
#else
				s_renderer = SystemInfo.graphicsDeviceName ?? string.Empty;
#endif
			}
			return s_renderer;
		}
	}

	// Chrome/Edge without a GPU: "SwiftShader"; Firefox/Linux: "llvmpipe"/"softpipe"; Windows WARP: "Microsoft Basic
	// Render Driver"; some report "Software" in the name.
	public static bool IsSoftwareRenderer(string renderer)
	{
		string r = (renderer ?? string.Empty).ToLowerInvariant();
		return r.Contains("swiftshader") || r.Contains("llvmpipe") || r.Contains("softpipe") || r.Contains("basic render") || r.Contains("software");
	}

	public static EQualityLevel? UrlOverride()
	{
		string url = Application.absoluteURL ?? string.Empty;
		int q = url.IndexOf("quality=", StringComparison.OrdinalIgnoreCase);
		if (q < 0)
		{
			return null;
		}
		string v = url.Substring(q + "quality=".Length);
		int end = v.IndexOfAny(new[] { '&', '#' });
		if (end >= 0)
		{
			v = v.Substring(0, end);
		}
		switch (v.ToLowerInvariant())
		{
		case "low":
			return EQualityLevel.Low;
		case "medium":
			return EQualityLevel.Medium;
		case "high":
			return EQualityLevel.High;
		case "ultra":
			return EQualityLevel.Ultra;
		default:
			return null;
		}
	}

	public static bool Auto
	{
		get { return PlayerPrefs.GetInt(AutoPrefKey, 1) == 1; }
		set
		{
			PlayerPrefs.SetInt(AutoPrefKey, value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}
}
