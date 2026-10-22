using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("LCArt/Screen Depth Blur")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class ScreenDepthBlurVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Screen Blur Properties")]
	[SerializeField]
	private FloatParameter _screenDepthMaxDistance;

	[SerializeField]
	private FloatParameter _screenDepthMinDistance;

	[SerializeField]
	private ClampedIntParameter _screenDepthBlurDegree;

	[Tooltip("보케 반경 (Circle of Confusion 크기)")]
	[SerializeField]
	private ClampedFloatParameter _bokehRadius;

	public FloatParameter ScreenDepthMaxDistance => _screenDepthMaxDistance;

	public FloatParameter ScreenDepthMinDistance => _screenDepthMinDistance;

	public ClampedIntParameter ScreenDepthBlurDegree => _screenDepthBlurDegree;

	public ClampedFloatParameter BokehRadius => _bokehRadius;

	public bool IsActive()
	{
		return _screenDepthMaxDistance.overrideState || _screenDepthMinDistance.overrideState || _screenDepthBlurDegree.overrideState;
	}

	public bool IsTileCompatible()
	{
		return false;
	}

	public ScreenDepthBlurVolumeComponent()
	{
		_screenDepthMaxDistance = new FloatParameter(0f, true);
		_screenDepthMinDistance = new FloatParameter(-10f, true);
		_screenDepthBlurDegree = new ClampedIntParameter(2, 0, 6, true);
		_bokehRadius = new ClampedFloatParameter(1f, 0.5f, 3f, true);
		active = true;
	}
}
