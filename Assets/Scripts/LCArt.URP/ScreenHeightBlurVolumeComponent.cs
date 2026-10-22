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

	public ScreenHeightBlurVolumeComponent()
	{
		_screenHeightMaxRatio = new ClampedFloatParameter(0.85f, 0f, 1f, true);
		_screenHeightMinRatio = new ClampedFloatParameter(0.2f, 0f, 1f, true);
		_screenHeightBlurDegree = new ClampedIntParameter(1, 0, 6, true);
		active = true;
	}
}
