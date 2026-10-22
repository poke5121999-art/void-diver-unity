using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Project DE/Directional Light")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class DirectionalLightVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Main Light Properties")]
	[SerializeField]
	private ColorParameter _mainLightColor;

	[SerializeField]
	private ClampedFloatParameter _mainLightIntensity;

	[SerializeField]
	private ClampedFloatParameter _mainLightRotationX;

	[SerializeField]
	private ClampedFloatParameter _mainLightRotationY;

	public ColorParameter MainLightColor => null;

	public ClampedFloatParameter MainLightIntensity => null;

	public ClampedFloatParameter MainLightRotationX => null;

	public ClampedFloatParameter MainLightRotationY => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
