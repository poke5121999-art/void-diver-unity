using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("LCArt/Plane Reflection")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class PlaneReflectionVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Fog Properties")]
	[SerializeField]
	private ColorParameter _reflectionColor;

	[SerializeField]
	private FloatParameter _reflectionWorldY;

	[SerializeField]
	private ClampedFloatParameter _reflectionOpacity;

	public ColorParameter RelfectionColor => null;

	public FloatParameter ReflectionWorldY => null;

	public ClampedFloatParameter ReflectionOpacity => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
