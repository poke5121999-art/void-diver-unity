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
	private ClampedFloatParameter _backgroundNomralIntensity;

	[SerializeField]
	private ColorParameter _backgroundShadowColor;

	[SerializeField]
	private ColorParameter _characterShadowColor;

	[SerializeField]
	private ColorParameter _characterOutOfSightColor;

	public ClampedFloatParameter BackgroundNomralIntensity => null;

	public ColorParameter BackgroundShadowColor => null;

	public ColorParameter CharacterShadowColor => null;

	public ColorParameter CharacterOutOfSightColor => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
