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

	public ClampedFloatParameter ScreenHeightMaxRatio => _screenHeightMaxRatio;

	public ClampedFloatParameter ScreenHeightMinRatio => _screenHeightMinRatio;

	public ClampedIntParameter ScreenHeightBlurDegree => _screenHeightBlurDegree;

	public FloatParameter ScreenDepthMaxDistance => _screenDepthMaxDistance;

	public FloatParameter ScreenDepthMinDistance => _screenDepthMinDistance;

	public ClampedIntParameter ScreenDepthBlurDegree => _screenDepthBlurDegree;

	public bool IsActive()
	{
		return _screenHeightMaxRatio.overrideState || _screenHeightMinRatio.overrideState || _screenHeightBlurDegree.overrideState || _screenDepthMaxDistance.overrideState || _screenDepthMinDistance.overrideState || _screenDepthBlurDegree.overrideState;
	}

	public bool IsTileCompatible()
	{
		return false;
	}

	public ScreenBlurUnifiedVolumeComponent()
	{
		_screenHeightMaxRatio = new ClampedFloatParameter(0.85f, 0f, 1f, true);
		_screenHeightMinRatio = new ClampedFloatParameter(0.2f, 0f, 1f, true);
		_screenHeightBlurDegree = new ClampedIntParameter(1, 0, 6, true);
		_screenDepthMaxDistance = new FloatParameter(0f, true);
		_screenDepthMinDistance = new FloatParameter(-10f, true);
		_screenDepthBlurDegree = new ClampedIntParameter(2, 0, 6, true);
		active = true;
	}
}
