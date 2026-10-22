using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("LCArt/Screen Blur (Unified)")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class ScreenBlurUnifiedVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Screen Height Blur")]
	[SerializeField]
	private ClampedFloatParameter _screenHeightMaxRatio;

	[SerializeField]
	private ClampedFloatParameter _screenHeightMinRatio;

	[SerializeField]
	private ClampedIntParameter _screenHeightBlurDegree;

	[Header("Screen Depth Blur")]
	[SerializeField]
	private FloatParameter _screenDepthMaxDistance;

	[SerializeField]
	private FloatParameter _screenDepthMinDistance;

	[SerializeField]
	private ClampedIntParameter _screenDepthBlurDegree;

	public ClampedFloatParameter ScreenHeightMaxRatio => null;

	public ClampedFloatParameter ScreenHeightMinRatio => null;

	public ClampedIntParameter ScreenHeightBlurDegree => null;

	public FloatParameter ScreenDepthMaxDistance => null;

	public FloatParameter ScreenDepthMinDistance => null;

	public ClampedIntParameter ScreenDepthBlurDegree => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
