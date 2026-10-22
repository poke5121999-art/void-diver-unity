using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Project DE/Camera Background Color")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class CameraBackgroundColorVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Camera Background Color Properties")]
	[Tooltip("카메라 배경 색상")]
	[SerializeField]
	private ColorParameter _backgroundColor;

	public ColorParameter BackgroundColor => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
