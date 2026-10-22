using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[Serializable]
[VolumeComponentMenu("Project DE/Shadow")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class FakeShadowVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Common")]
	[FormerlySerializedAs("_screenSpaceShadowColor")]
	[SerializeField]
	private ColorParameter _shadowColor;

	[FormerlySerializedAs("_screenSpaceShadowWorldY")]
	[SerializeField]
	private FloatParameter _shadowStandardY;

	[Header("Screen Space Shadow")]
	[SerializeField]
	private ClampedFloatParameter _screenSpaceShadowAlpha;

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowDirectionalAlpha;

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowDirectionalLength;

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowAdditionalAlpha;

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowAdditionalLength;

	[Header("Shadow Color (BG / Character)")]
	[SerializeField]
	private ClampedFloatParameter _backgroundNomralIntensity;

	[SerializeField]
	private ColorParameter _backgroundShadowColor;

	[SerializeField]
	private ColorParameter _characterShadowColor;

	[SerializeField]
	private ColorParameter _characterOutOfSightColor;

	public ColorParameter ShadowColor => null;

	public FloatParameter ShadowStandardY => null;

	public ClampedFloatParameter ScreenSpaceShadowAlpha => null;

	public ClampedFloatParameter ScreenSpaceObjectShadowDirectionalAlpha => null;

	public ClampedFloatParameter ScreenSpaceObjectShadowDirectionalLength => null;

	public ClampedFloatParameter ScreenSpaceObjectShadowAdditionalAlpha => null;

	public ClampedFloatParameter ScreenSpaceObjectShadowAdditionalLength => null;

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
