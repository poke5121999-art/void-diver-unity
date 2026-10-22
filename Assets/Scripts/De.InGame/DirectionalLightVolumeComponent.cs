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
	private ColorParameter _mainLightColor = new ColorParameter(new Color(1f, 1f, 1f, 1f), true);

	[SerializeField]
	private ClampedFloatParameter _mainLightIntensity = new ClampedFloatParameter(1f, 0f, 10f, true);

	[SerializeField]
	private ClampedFloatParameter _mainLightRotationX = new ClampedFloatParameter(45f, 0f, 90f, true);

	[SerializeField]
	private ClampedFloatParameter _mainLightRotationY = new ClampedFloatParameter(45f, 0f, 360f, true);

	public ColorParameter MainLightColor => _mainLightColor;

	public ClampedFloatParameter MainLightIntensity => _mainLightIntensity;

	public ClampedFloatParameter MainLightRotationX => _mainLightRotationX;

	public ClampedFloatParameter MainLightRotationY => _mainLightRotationY;

	public bool IsActive()
	{
		return _mainLightIntensity.value > 0f;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
