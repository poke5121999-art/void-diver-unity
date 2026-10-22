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
	private BoolParameter _enable;

	[Tooltip("안개 색상")]
	[SerializeField]
	private ColorParameter _fogColor;

	[Tooltip("안개 시작 거리 (미터)")]
	[FormerlySerializedAs("_fogStart")]
	[SerializeField]
	private ClampedFloatParameter _fogDistanceMin;

	[Tooltip("안개 최대 거리 (미터) - 이 거리에서 완전히 불투명")]
	[FormerlySerializedAs("_fogEnd")]
	[SerializeField]
	private ClampedFloatParameter _fogDistanceMax;

	public BoolParameter Enable => null;

	public ColorParameter FogColor => null;

	public ClampedFloatParameter FogDistanceMin => null;

	public ClampedFloatParameter FogDistanceMax => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
