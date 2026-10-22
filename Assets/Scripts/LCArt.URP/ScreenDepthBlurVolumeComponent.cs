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

	public FloatParameter ScreenDepthMaxDistance => null;

	public FloatParameter ScreenDepthMinDistance => null;

	public ClampedIntParameter ScreenDepthBlurDegree => null;

	public ClampedFloatParameter BokehRadius => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
