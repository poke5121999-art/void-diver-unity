using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Project DE/Height Fog")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class HeightFogVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Height Fog Properties")]
	[Tooltip("높이 기반 안개 활성화 여부")]
	[SerializeField]
	private BoolParameter _enable;

	[Tooltip("안개 시작 높이 (월드 Y 좌표)")]
	[SerializeField]
	private ClampedFloatParameter _fogHeightMin;

	[Tooltip("안개 최대 높이 (월드 Y 좌표) - 이 높이에서 완전히 불투명")]
	[SerializeField]
	private ClampedFloatParameter _fogHeightMax;

	public BoolParameter Enable => null;

	public ClampedFloatParameter FogHeightMin => null;

	public ClampedFloatParameter FogHeightMax => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
