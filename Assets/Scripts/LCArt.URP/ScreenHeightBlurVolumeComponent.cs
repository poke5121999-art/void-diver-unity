using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("LCArt/Screen Height Blur")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class ScreenHeightBlurVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Screen Blur Properties")]
	[SerializeField]
	private ClampedFloatParameter _screenHeightMaxRatio;

	[SerializeField]
	private ClampedFloatParameter _screenHeightMinRatio;

	[SerializeField]
	private ClampedIntParameter _screenHeightBlurDegree;

	public ClampedFloatParameter ScreenHeightMaxRatio => null;

	public ClampedFloatParameter ScreenHeightMinRatio => null;

	public ClampedIntParameter ScreenHeightBlurDegree => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
