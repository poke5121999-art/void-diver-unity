using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("LCArt/Custom SSAO")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class CustomScreenSpaceAmbientOcclusionVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Custom SSAO Properties")]
	[Tooltip("SSAO 색상")]
	[SerializeField]
	private ColorParameter _ssaoColor;

	[SerializeField]
	private ClampedFloatParameter _ssaoIntensity;

	[SerializeField]
	private ClampedFloatParameter _ssaoRadius;

	public ColorParameter SSAOColor => null;

	public ClampedFloatParameter SSAOIntensity => null;

	public ClampedFloatParameter SSAORadius => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
