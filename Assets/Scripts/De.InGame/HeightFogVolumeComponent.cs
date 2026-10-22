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
	private BoolParameter _enable = new BoolParameter(value: false, overrideState: true);

	[Tooltip("안개 시작 높이 (월드 Y 좌표)")]
	[SerializeField]
	private ClampedFloatParameter _fogHeightMin = new ClampedFloatParameter(-10f, -100f, 100f, overrideState: true);

	[Tooltip("안개 최대 높이 (월드 Y 좌표) - 이 높이에서 완전히 불투명")]
	[SerializeField]
	private ClampedFloatParameter _fogHeightMax = new ClampedFloatParameter(0f, -100f, 100f, overrideState: true);

	public BoolParameter Enable => _enable;

	public ClampedFloatParameter FogHeightMin => _fogHeightMin;

	public ClampedFloatParameter FogHeightMax => _fogHeightMax;

	public bool IsActive()
	{
		return _enable.value;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
