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
	private ColorParameter _backgroundColor = new ColorParameter(new Color(0f, 0f, 0f, 1f), overrideState: true);

	[SerializeField]
	private ClampedFloatParameter _backgroundColorIntensity = new ClampedFloatParameter(0f, 0f, 1f, overrideState: true);

	public ColorParameter BackgroundColor => _backgroundColor;

	public ClampedFloatParameter BackgroundColorIntensity => _backgroundColorIntensity;

	public bool IsActive()
	{
		return _backgroundColorIntensity.value > 0f;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
