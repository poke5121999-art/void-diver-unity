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
	private ColorParameter _shadowColor = new ColorParameter(new Color(0f, 0f, 0f, 1f), overrideState: true);

	[FormerlySerializedAs("_screenSpaceShadowWorldY")]
	[SerializeField]
	private FloatParameter _shadowStandardY = new FloatParameter(0f, overrideState: true);

	[Header("Screen Space Shadow")]
	[SerializeField]
	private ClampedFloatParameter _screenSpaceShadowAlpha = new ClampedFloatParameter(1f, 0f, 1f, overrideState: true);

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowDirectionalAlpha = new ClampedFloatParameter(0.5f, 0f, 4f, overrideState: true);

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowDirectionalLength = new ClampedFloatParameter(1f, 0.5f, 4f, overrideState: true);

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowAdditionalAlpha = new ClampedFloatParameter(0.8f, 0f, 4f, overrideState: true);

	[SerializeField]
	private ClampedFloatParameter _screenSpaceObjectShadowAdditionalLength = new ClampedFloatParameter(1.5f, 0.5f, 4f, overrideState: true);

	[Header("Shadow Color (BG / Character)")]
	[SerializeField]
	private ClampedFloatParameter _backgroundNomralIntensity = new ClampedFloatParameter(1f, 0f, 5f, overrideState: true);

	[SerializeField]
	private ColorParameter _backgroundShadowColor = new ColorParameter(new Color(0.5f, 0.5f, 0.6f, 1f), overrideState: true);

	[SerializeField]
	private ColorParameter _characterShadowColor = new ColorParameter(new Color(0.2f, 0.2f, 0.3f, 1f), overrideState: true);

	[SerializeField]
	private ColorParameter _characterOutOfSightColor = new ColorParameter(new Color(0.1f, 0.1f, 0.1f, 1f), overrideState: true);

	public ColorParameter ShadowColor => _shadowColor;

	public FloatParameter ShadowStandardY => _shadowStandardY;

	public ClampedFloatParameter ScreenSpaceShadowAlpha => _screenSpaceShadowAlpha;

	public ClampedFloatParameter ScreenSpaceObjectShadowDirectionalAlpha => _screenSpaceObjectShadowDirectionalAlpha;

	public ClampedFloatParameter ScreenSpaceObjectShadowDirectionalLength => _screenSpaceObjectShadowDirectionalLength;

	public ClampedFloatParameter ScreenSpaceObjectShadowAdditionalAlpha => _screenSpaceObjectShadowAdditionalAlpha;

	public ClampedFloatParameter ScreenSpaceObjectShadowAdditionalLength => _screenSpaceObjectShadowAdditionalLength;

	public ClampedFloatParameter BackgroundNomralIntensity => _backgroundNomralIntensity;

	public ColorParameter BackgroundShadowColor => _backgroundShadowColor;

	public ColorParameter CharacterShadowColor => _characterShadowColor;

	public ColorParameter CharacterOutOfSightColor => _characterOutOfSightColor;

	public bool IsActive()
	{
		return true;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
