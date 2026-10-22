using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Project DE/Background Color")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class BackgroundColorVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Background Color Properties")]
	[SerializeField]
	private ColorParameter _backgroundColor;

	[SerializeField]
	private ClampedFloatParameter _backgroundColorIntensity;

	public ColorParameter BackgroundColor => null;

	public ClampedFloatParameter BackgroundColorIntensity => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
