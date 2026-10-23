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
	private ClampedFloatParameter _screenHeightMaxRatio = new ClampedFloatParameter(0.85f, 0f, 1f, true);

	[SerializeField]
	private ClampedFloatParameter _screenHeightMinRatio = new ClampedFloatParameter(0.2f, 0f, 1f, true);

	[SerializeField]
	private ClampedIntParameter _screenHeightBlurDegree = new ClampedIntParameter(1, 0, 6, true);

	public ClampedFloatParameter ScreenHeightMaxRatio => _screenHeightMaxRatio;

	public ClampedFloatParameter ScreenHeightMinRatio => _screenHeightMinRatio;

	public ClampedIntParameter ScreenHeightBlurDegree => _screenHeightBlurDegree;

	public bool IsActive()
	{
		return _screenHeightMaxRatio.overrideState || _screenHeightMinRatio.overrideState || _screenHeightBlurDegree.overrideState;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
