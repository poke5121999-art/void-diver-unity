using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[Serializable]
[VolumeComponentMenu("Project DE/Distance Fog")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class FogVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Distance Fog Properties")]
	[Tooltip("거리 기반 안개 활성화 여부")]
	[SerializeField]
	private BoolParameter _enable = new BoolParameter(value: false, overrideState: true);

	[Tooltip("안개 색상")]
	[SerializeField]
	private ColorParameter _fogColor = new ColorParameter(new Color(1f, 1f, 1f, 1f), overrideState: true);

	[Tooltip("안개 시작 거리 (미터)")]
	[FormerlySerializedAs("_fogStart")]
	[SerializeField]
	private ClampedFloatParameter _fogDistanceMin = new ClampedFloatParameter(40f, 0f, 1000f, overrideState: true);

	[Tooltip("안개 최대 거리 (미터) - 이 거리에서 완전히 불투명")]
	[FormerlySerializedAs("_fogEnd")]
	[SerializeField]
	private ClampedFloatParameter _fogDistanceMax = new ClampedFloatParameter(80f, 0f, 1000f, overrideState: true);

	public BoolParameter Enable => _enable;

	public ColorParameter FogColor => _fogColor;

	public ClampedFloatParameter FogDistanceMin => _fogDistanceMin;

	public ClampedFloatParameter FogDistanceMax => _fogDistanceMax;

	public bool IsActive()
	{
		return _enable.value;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
