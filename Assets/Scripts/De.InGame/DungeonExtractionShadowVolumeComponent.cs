using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Project DE/Shadow Color")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class DungeonExtractionShadowVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Shadow Color Properties")]
	[SerializeField]
	private ClampedFloatParameter _backgroundNomralIntensity = new ClampedFloatParameter(1f, 0f, 5f, true);

	[SerializeField]
	private ColorParameter _backgroundShadowColor = new ColorParameter(new Color(0.5f, 0.5f, 0.6f, 1f), true);

	[SerializeField]
	private ColorParameter _characterShadowColor = new ColorParameter(new Color(0.2f, 0.2f, 0.3f, 1f), true);

	[SerializeField]
	private ColorParameter _characterOutOfSightColor = new ColorParameter(new Color(0.1f, 0.1f, 0.1f, 1f), true);

	public ClampedFloatParameter BackgroundNomralIntensity => _backgroundNomralIntensity;

	public ColorParameter BackgroundShadowColor => _backgroundShadowColor;

	public ColorParameter CharacterShadowColor => _characterShadowColor;

	public ColorParameter CharacterOutOfSightColor => _characterOutOfSightColor;

	public bool IsActive()
	{
		return _backgroundShadowColor.overrideState || _characterShadowColor.overrideState || _characterOutOfSightColor.overrideState;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
