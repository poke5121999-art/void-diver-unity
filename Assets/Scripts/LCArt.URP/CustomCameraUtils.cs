using System;
using UnityEngine;

public class CustomCameraUtils
{
	[Serializable]
	public enum ECustomCameraResolutionType
	{
		Full = 1,
		Half = 2,
		Quarter = 4,
		Eighth = 8,
		Sixteenth = 0x10
	}

	public const int DEFAULT_MAX_RT_HEIGHT = 1080;

	public const string MAX_RT_HEIGHT_PROPERTY = "_VD_RT_MAX_HEIGHT";

	private static readonly int _maxRtHeightPropertyId;

	public static int CurrentMaxRTHeight()
	{
		return 0;
	}

	public static void GetCappedRTSize(int width, int height, out int cappedWidth, out int cappedHeight)
	{
		cappedWidth = default;
		cappedHeight = default;
	}

	public static void GetCappedRTSize(int width, int height, out int cappedWidth, out int cappedHeight, int maxHeight)
	{
		cappedWidth = default;
		cappedHeight = default;
	}

	public static float GetDownsamplingFloat(in ECustomCameraResolutionType screenResolutionType)
	{
		return 0f;
	}

	public static bool IsValidCamera(Camera cam)
	{
		return false;
	}
}
