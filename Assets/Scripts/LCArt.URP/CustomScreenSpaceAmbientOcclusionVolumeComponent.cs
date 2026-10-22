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
	private ColorParameter _ssaoColor = new ColorParameter(Color.white, true);

	[SerializeField]
	private ClampedFloatParameter _ssaoIntensity = new ClampedFloatParameter(0.75f, 0f, 10f, true);

	[SerializeField]
	private ClampedFloatParameter _ssaoRadius = new ClampedFloatParameter(0.4f, 0f, 10f, true);

	public ColorParameter SSAOColor => _ssaoColor;

	public ClampedFloatParameter SSAOIntensity => _ssaoIntensity;

	public ClampedFloatParameter SSAORadius => _ssaoRadius;

	public bool IsActive()
	{
		return _ssaoIntensity.value > 0f;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
