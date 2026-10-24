using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

	private static readonly int _maxRtHeightPropertyId = Shader.PropertyToID("_VD_RT_MAX_HEIGHT");

	public static int CurrentMaxRTHeight()
	{
		// FUN_180069e90 is Mathf.RoundToInt (machine code checked, same body as @0x183ee7ca0)
		int value = Mathf.RoundToInt(Shader.GetGlobalFloat(_maxRtHeightPropertyId));
		return value > 0 ? value : DEFAULT_MAX_RT_HEIGHT;
	}

	public static void GetCappedRTSize(int width, int height, out int cappedWidth, out int cappedHeight)
	{
		GetCappedRTSize(width, height, out cappedWidth, out cappedHeight, CurrentMaxRTHeight());
	}

	public static void GetCappedRTSize(int width, int height, out int cappedWidth, out int cappedHeight, int maxHeight)
	{
		if (height < 1 || width < 1 || height <= maxHeight)
		{
			cappedWidth = width;
			cappedHeight = height;
			return;
		}
		cappedHeight = maxHeight;
		int w = Mathf.RoundToInt((float)width / (float)height * (float)maxHeight);
		cappedWidth = w < 1 ? 1 : w;
	}

	public static float GetDownsamplingFloat(in ECustomCameraResolutionType screenResolutionType)
	{
		switch (screenResolutionType)
		{
		case ECustomCameraResolutionType.Full:
			return 1f;
		case ECustomCameraResolutionType.Quarter:
			return 0.25f;
		case ECustomCameraResolutionType.Eighth:
			return 0.125f;
		case ECustomCameraResolutionType.Sixteenth:
			return 0.0625f;
		default:
			return 0.5f;
		}
	}

	public static bool IsValidCamera(Camera cam)
	{
		if (cam == null)
		{
			return false;
		}
		if (!cam.TryGetComponent<UniversalAdditionalCameraData>(out var data))
		{
			return false;
		}
		if (data.renderType == CameraRenderType.Overlay)
		{
			return false;
		}
		if (cam.cameraType == CameraType.SceneView || cam.cameraType == CameraType.Preview)
		{
			return false;
		}
		return cam.cameraType == CameraType.Game && data.renderType == CameraRenderType.Base;
	}
}
