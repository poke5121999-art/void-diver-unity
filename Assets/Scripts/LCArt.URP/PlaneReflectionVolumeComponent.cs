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
	private ColorParameter _reflectionColor = new ColorParameter(new Color(0f, 0f, 0f, 1f), true);

	[SerializeField]
	private FloatParameter _reflectionWorldY = new FloatParameter(0f, true);

	[SerializeField]
	private ClampedFloatParameter _reflectionOpacity = new ClampedFloatParameter(1f, 0f, 1f, true);

	public ColorParameter RelfectionColor => _reflectionColor;

	public FloatParameter ReflectionWorldY => _reflectionWorldY;

	public ClampedFloatParameter ReflectionOpacity => _reflectionOpacity;

	public bool IsActive()
	{
		return _reflectionWorldY.overrideState || _reflectionOpacity.overrideState;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
