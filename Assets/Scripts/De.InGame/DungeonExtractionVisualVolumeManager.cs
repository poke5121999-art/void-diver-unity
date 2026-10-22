using UnityEngine;
using UnityEngine.Rendering;

[DefaultExecutionOrder(-100)]
[ExecuteAlways]
public class DungeonExtractionVisualVolumeManager : CustomVisualVolumeManager
{
	private static class ShaderIDs
	{
		public static readonly int BackgroundColor = Shader.PropertyToID("_GlobalBackgroundColor");

		public static readonly int BackgroundColorIntensity = Shader.PropertyToID("_GlobalBackgroundColorIntensity");

		public static readonly int FogHeightEnable = Shader.PropertyToID("_GlobalFogHeightEnable");

		public static readonly int FogHeightMax = Shader.PropertyToID("_GlobalFogHeightMax");

		public static readonly int FogHeightMin = Shader.PropertyToID("_GlobalFogHeightMin");

		public static readonly int DialogBaseColorProperty = Shader.PropertyToID("_GlobalDialogBaseColor");

		public static readonly int DialogRimLightColorProperty = Shader.PropertyToID("_GlobalDialogRimLightColor");

		public static readonly int DialogRimLightAngleProperty = Shader.PropertyToID("_GlobalDialogRimLightAngle");

		public static readonly int DialogRimLightOffsetProperty = Shader.PropertyToID("_GlobalDialogRimLightOffset");

		public static readonly int WeatherIntensity = Shader.PropertyToID("_WeatherIntensity");

		public static readonly int WeatherIndoorMask = Shader.PropertyToID("_WeatherIndoorMask");

		public static readonly int WeatherFogColor = Shader.PropertyToID("_WeatherFogColor");

		public static readonly int WeatherFogVignetteCenter = Shader.PropertyToID("_WeatherFogVignetteCenter");

		public static readonly int WeatherFogVignetteStart = Shader.PropertyToID("_WeatherFogVignetteStart");

		public static readonly int WeatherFogVignetteEnd = Shader.PropertyToID("_WeatherFogVignetteEnd");

		public static readonly int WeatherFogVignetteIntensity = Shader.PropertyToID("_WeatherFogVignetteIntensity");

		public static readonly int WeatherFogNoiseScale = Shader.PropertyToID("_WeatherFogNoiseScale");

		public static readonly int WeatherFogNoiseSpeed = Shader.PropertyToID("_WeatherFogNoiseSpeed");

		public static readonly int WeatherFogNoiseIntensity = Shader.PropertyToID("_WeatherFogNoiseIntensity");

		public static readonly int WeatherStreakAngle = Shader.PropertyToID("_WeatherStreakAngle");

		public static readonly int WeatherStreakSpeed = Shader.PropertyToID("_WeatherStreakSpeed");

		public static readonly int WeatherStreakDensity = Shader.PropertyToID("_WeatherStreakDensity");

		public static readonly int WeatherStreakProbability = Shader.PropertyToID("_WeatherStreakProbability");

		public static readonly int WeatherStreakThickness = Shader.PropertyToID("_WeatherStreakThickness");

		public static readonly int WeatherStreakLength = Shader.PropertyToID("_WeatherStreakLength");

		public static readonly int WeatherStreakBrightness = Shader.PropertyToID("_WeatherStreakBrightness");

		public static readonly int WeatherStreakColor = Shader.PropertyToID("_WeatherStreakColor");

		public static readonly int WeatherStreakCellShiftSpeed = Shader.PropertyToID("_WeatherStreakCellShiftSpeed");

		public static readonly int WeatherSplashScale = Shader.PropertyToID("_WeatherSplashScale");

		public static readonly int WeatherSplashSpeed = Shader.PropertyToID("_WeatherSplashSpeed");

		public static readonly int WeatherSplashIntensity = Shader.PropertyToID("_WeatherSplashIntensity");

		public static readonly int WeatherSplashSize = Shader.PropertyToID("_WeatherSplashSize");

		public static readonly int WeatherSplashCellShiftSpeed = Shader.PropertyToID("_WeatherSplashCellShiftSpeed");

		public static readonly int WeatherHazeSpeed = Shader.PropertyToID("_WeatherHazeSpeed");

		public static readonly int WeatherHazeStrength = Shader.PropertyToID("_WeatherHazeStrength");

		public static readonly int WeatherHazeNoiseScale = Shader.PropertyToID("_WeatherHazeNoiseScale");

		public static readonly int WeatherHazeBlurBlend = Shader.PropertyToID("_WeatherHazeBlurBlend");

		public static readonly int WeatherHazeVerticalBias = Shader.PropertyToID("_WeatherHazeVerticalBias");

		public static readonly int WeatherHazeVignetteStart = Shader.PropertyToID("_WeatherHazeVignetteStart");

		public static readonly int WeatherHazeVignetteEnd = Shader.PropertyToID("_WeatherHazeVignetteEnd");

		public static readonly int WeatherStreakBlendMode = Shader.PropertyToID("_WeatherStreakBlendMode");

		public static readonly int WeatherStreakCameraParallax = Shader.PropertyToID("_WeatherStreakCameraParallax");

		public static readonly int WeatherDropletIntensity = Shader.PropertyToID("_WeatherDropletIntensity");

		public static readonly int WeatherDropletScale = Shader.PropertyToID("_WeatherDropletScale");

		public static readonly int WeatherDropletSpeed = Shader.PropertyToID("_WeatherDropletSpeed");

		public static readonly int WeatherDropletDistortion = Shader.PropertyToID("_WeatherDropletDistortion");

		public static readonly int WeatherDropletSize = Shader.PropertyToID("_WeatherDropletSize");

		public static readonly int WeatherDropletVignetteStart = Shader.PropertyToID("_WeatherDropletVignetteStart");

		public static readonly int WeatherDropletVignetteEnd = Shader.PropertyToID("_WeatherDropletVignetteEnd");

		public static readonly int WeatherSplashColor = Shader.PropertyToID("_WeatherSplashColor");

		public static readonly int WeatherSplashBlendMode = Shader.PropertyToID("_WeatherSplashBlendMode");

		public static readonly int WeatherFogBlendMode = Shader.PropertyToID("_WeatherFogBlendMode");

		public static readonly int WeatherFogIndoorFactor = Shader.PropertyToID("_WeatherFogIndoorFactor");

		public static readonly int WeatherBurnBlendMode = Shader.PropertyToID("_WeatherBurnBlendMode");

		public static readonly int WeatherBurnIntensity = Shader.PropertyToID("_WeatherBurnIntensity");

		public static readonly int WeatherBurnColor = Shader.PropertyToID("_WeatherBurnColor");

		public static readonly int WeatherBurnSpeed = Shader.PropertyToID("_WeatherBurnSpeed");

		public static readonly int WeatherBurnDirection = Shader.PropertyToID("_WeatherBurnDirection");

		public static readonly int WeatherBurnNoiseScale = Shader.PropertyToID("_WeatherBurnNoiseScale");

		public static readonly int WeatherBurnVignetteStart = Shader.PropertyToID("_WeatherBurnVignetteStart");

		public static readonly int WeatherBurnVignetteEnd = Shader.PropertyToID("_WeatherBurnVignetteEnd");

		public static readonly int WeatherFrostOverlayIntensity = Shader.PropertyToID("_WeatherFrostOverlayIntensity");

		public static readonly int WeatherFrostOverlayStart = Shader.PropertyToID("_WeatherFrostOverlayStart");

		public static readonly int WeatherFrostOverlayEnd = Shader.PropertyToID("_WeatherFrostOverlayEnd");

		public static readonly int WeatherFrostOverlayNoiseScale = Shader.PropertyToID("_WeatherFrostOverlayNoiseScale");

		public static readonly int WeatherFrostOverlayColor = Shader.PropertyToID("_WeatherFrostOverlayColor");

		public static readonly int WeatherWetSurfaceIntensity = Shader.PropertyToID("_WeatherWetSurfaceIntensity");

		public static readonly int WeatherRippleStrength = Shader.PropertyToID("_WeatherRippleStrength");

		public static readonly int WeatherBgTintColor = Shader.PropertyToID("_WeatherBgTintColor");

		public static readonly int WeatherBgTintIntensity = Shader.PropertyToID("_WeatherBgTintIntensity");

		public static readonly int WeatherBgDesaturation = Shader.PropertyToID("_WeatherBgDesaturation");

		public static readonly int BackgroundShadowColorProperty = Shader.PropertyToID("_GlobalBackgroundShadowColor");

		public static readonly int BackgroundNormalIntensityProperty = Shader.PropertyToID("_GlobalBackgroundNormalIntensity");

		public static readonly int CharacterShadowColorProperty = Shader.PropertyToID("_GlobalCharacterShadowColor");

		public static readonly int CharacterOutOfColorProperty = Shader.PropertyToID("_GlobalCharacterOutOfColor");
	}

	[Header("DE: Debug Info")]
	[SerializeField]
	private bool _cameraBackgroundColorActive;

	[SerializeField]
	private bool _distanceFogActive;

	[SerializeField]
	private bool _heightFogActive;

	[SerializeField]
	private bool _directionalLightActive;

	[SerializeField]
	private bool _backgroundColorActive;

	[SerializeField]
	private bool _dialogColorActive;

	[SerializeField]
	private bool _shadowColorActive;

	[SerializeField]
	private bool _weatherVisualActive;

	protected override void UpdateVisualEffects()
	{
		base.UpdateVisualEffects();
		UpdateDistanceFog(VolumeStack);
		UpdateHeightFog(VolumeStack);
		UpdateCameraBackgroundColor(VolumeStack);
		UpdateDirectionalLight(VolumeStack);
		UpdateBackgroundColor(VolumeStack);
		UpdateWeatherVisual(VolumeStack);
		UpdateDialogColor(VolumeStack);
		UpdateShadowColor(VolumeStack);
	}

	protected override void SetDisableVisualEffects()
	{
		base.SetDisableVisualEffects();
		Shader.SetGlobalColor(ShaderIDs.BackgroundColor, Color.black);
		Shader.SetGlobalFloat(ShaderIDs.BackgroundColorIntensity, 0f);
		Shader.SetGlobalFloat(ShaderIDs.WeatherIntensity, 0f);
		_weatherVisualActive = false;
		RenderSettings.fog = false;
		Shader.SetGlobalFloat(ShaderIDs.FogHeightEnable, 0f);
		Shader.SetGlobalFloat(ShaderIDs.FogHeightMax, -15f);
		Shader.SetGlobalFloat(ShaderIDs.FogHeightMin, -35f);
		if (TargetCamera != null)
		{
			TargetCamera.backgroundColor = Color.black;
		}
		Shader.SetGlobalFloat(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_OVERRIDE_ENABLE, 0f);
		_distanceFogActive = false;
		_heightFogActive = false;
		_directionalLightActive = false;
		_backgroundColorActive = false;
		_cameraBackgroundColorActive = false;
	}

	private void UpdateBackgroundColor(VolumeStack volumeStack)
	{
		BackgroundColorVolumeComponent component = volumeStack.GetComponent<BackgroundColorVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalColor(ShaderIDs.BackgroundColor, Color.black);
			Shader.SetGlobalFloat(ShaderIDs.BackgroundColorIntensity, 0f);
			_backgroundColorActive = false;
			return;
		}
		if (component.BackgroundColor.overrideState)
		{
			Shader.SetGlobalColor(ShaderIDs.BackgroundColor, component.BackgroundColor.value);
		}
		if (component.BackgroundColorIntensity.overrideState)
		{
			Shader.SetGlobalFloat(ShaderIDs.BackgroundColorIntensity, component.BackgroundColorIntensity.value);
		}
		_backgroundColorActive = true;
	}

	private void UpdateWeatherVisual(VolumeStack volumeStack)
	{
		WeatherVisualVolumeComponent weather = volumeStack.GetComponent<WeatherVisualVolumeComponent>();
		if (weather == null || weather.Intensity.value <= 0.001f)
		{
			Shader.SetGlobalFloat(ShaderIDs.WeatherIntensity, 0f);
			_weatherVisualActive = false;
			return;
		}
		Shader.SetGlobalFloat(ShaderIDs.WeatherIntensity, weather.Intensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherIndoorMask, weather.IndoorMask.value);
		Shader.SetGlobalColor(ShaderIDs.WeatherFogColor, weather.FogColor.value);
		Shader.SetGlobalVector(ShaderIDs.WeatherFogVignetteCenter, weather.FogVignetteCenter.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogVignetteStart, weather.FogVignetteStart.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogVignetteEnd, weather.FogVignetteEnd.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogVignetteIntensity, weather.FogVignetteIntensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogNoiseScale, weather.FogNoiseScale.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogNoiseSpeed, weather.FogNoiseSpeed.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogNoiseIntensity, weather.FogNoiseIntensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogBlendMode, weather.FogBlendMode.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFogIndoorFactor, weather.FogIndoorFactor.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakAngle, weather.StreakAngle.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakSpeed, weather.StreakSpeed.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakDensity, weather.StreakDensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakProbability, weather.StreakProbability.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakThickness, weather.StreakThickness.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakLength, weather.StreakLength.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakBrightness, weather.StreakBrightness.value);
		Shader.SetGlobalColor(ShaderIDs.WeatherStreakColor, weather.StreakColor.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakCellShiftSpeed, weather.StreakCellShiftSpeed.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherStreakBlendMode, weather.StreakBlendMode.value);
		Shader.SetGlobalVector(ShaderIDs.WeatherStreakCameraParallax, weather.StreakCameraParallax.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherSplashScale, weather.SplashScale.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherSplashSpeed, weather.SplashSpeed.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherSplashIntensity, weather.SplashIntensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherSplashSize, weather.SplashSize.value);
		Shader.SetGlobalColor(ShaderIDs.WeatherSplashColor, weather.SplashColor.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherSplashBlendMode, weather.SplashBlendMode.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherSplashCellShiftSpeed, weather.SplashCellShiftSpeed.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherDropletIntensity, weather.DropletIntensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherDropletScale, weather.DropletScale.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherDropletSpeed, weather.DropletSpeed.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherDropletDistortion, weather.DropletDistortion.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherDropletSize, weather.DropletSize.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherDropletVignetteStart, weather.DropletVignetteStart.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherDropletVignetteEnd, weather.DropletVignetteEnd.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherHazeSpeed, weather.HazeSpeed.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherHazeStrength, weather.HazeStrength.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherHazeBlurBlend, weather.HazeBlurBlend.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherHazeVerticalBias, weather.HazeVerticalBias.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherHazeNoiseScale, weather.HazeNoiseScale.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherHazeVignetteStart, weather.HazeVignetteStart.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherHazeVignetteEnd, weather.HazeVignetteEnd.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBurnIntensity, weather.BurnIntensity.value);
		Shader.SetGlobalColor(ShaderIDs.WeatherBurnColor, weather.BurnColor.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBurnBlendMode, weather.BurnBlendMode.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBurnSpeed, weather.BurnSpeed.value);
		Shader.SetGlobalVector(ShaderIDs.WeatherBurnDirection, weather.BurnDirection.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBurnNoiseScale, weather.BurnNoiseScale.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBurnVignetteStart, weather.BurnVignetteStart.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBurnVignetteEnd, weather.BurnVignetteEnd.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFrostOverlayIntensity, weather.FrostOverlayIntensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFrostOverlayStart, weather.FrostOverlayStart.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFrostOverlayEnd, weather.FrostOverlayEnd.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherFrostOverlayNoiseScale, weather.FrostOverlayNoiseScale.value);
		Shader.SetGlobalColor(ShaderIDs.WeatherFrostOverlayColor, weather.FrostOverlayColor.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherWetSurfaceIntensity, weather.WetSurfaceIntensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherRippleStrength, weather.RippleStrength.value);
		Shader.SetGlobalColor(ShaderIDs.WeatherBgTintColor, weather.BgTintColor.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBgTintIntensity, weather.BgTintIntensity.value);
		Shader.SetGlobalFloat(ShaderIDs.WeatherBgDesaturation, weather.BgDesaturation.value);
		float windStrength = weather.WindStrength.value;
		Shader.SetGlobalFloat(WeatherVisualConst.PROPERTY_WIND_ENABLE, (windStrength > 0f) ? 1f : 0f);
		Shader.SetGlobalFloat(WeatherVisualConst.PROPERTY_WIND_INTENSITY, windStrength);
		_weatherVisualActive = true;
	}

	private void UpdateDistanceFog(VolumeStack volumeStack)
	{
		FogVolumeComponent component = volumeStack.GetComponent<FogVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			RenderSettings.fog = false;
			_distanceFogActive = false;
			return;
		}
		RenderSettings.fog = true;
		RenderSettings.fogMode = FogMode.Linear;
		if (component.FogColor.overrideState)
		{
			RenderSettings.fogColor = component.FogColor.value;
		}
		if (component.FogDistanceMin.overrideState)
		{
			RenderSettings.fogStartDistance = component.FogDistanceMin.value;
		}
		if (component.FogDistanceMax.overrideState)
		{
			RenderSettings.fogEndDistance = component.FogDistanceMax.value;
		}
		if (component.FogDistanceMax.overrideState && component.FogDistanceMin.overrideState)
		{
			RenderSettings.fog = component.FogDistanceMin.value < component.FogDistanceMax.value;
		}
		_distanceFogActive = true;
	}

	private void UpdateHeightFog(VolumeStack volumeStack)
	{
		HeightFogVolumeComponent component = volumeStack.GetComponent<HeightFogVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalFloat(ShaderIDs.FogHeightEnable, 0f);
			Shader.SetGlobalFloat(ShaderIDs.FogHeightMax, -15f);
			Shader.SetGlobalFloat(ShaderIDs.FogHeightMin, -35f);
			_heightFogActive = false;
			return;
		}
		Shader.SetGlobalFloat(ShaderIDs.FogHeightEnable, 1f);
		if (component.FogHeightMin.overrideState)
		{
			Shader.SetGlobalFloat(ShaderIDs.FogHeightMin, component.FogHeightMin.value);
		}
		if (component.FogHeightMax.overrideState)
		{
			Shader.SetGlobalFloat(ShaderIDs.FogHeightMax, component.FogHeightMax.value);
		}
		_heightFogActive = true;
	}

	private void UpdateCameraBackgroundColor(VolumeStack volumeStack)
	{
		CameraBackgroundColorVolumeComponent component = volumeStack.GetComponent<CameraBackgroundColorVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			_cameraBackgroundColorActive = false;
			return;
		}
		if (component.BackgroundColor.overrideState && TargetCamera != null)
		{
			TargetCamera.backgroundColor = component.BackgroundColor.value;
		}
		_cameraBackgroundColorActive = true;
	}

	private void UpdateDirectionalLight(VolumeStack volumeStack)
	{
		DirectionalLightVolumeComponent component = volumeStack.GetComponent<DirectionalLightVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			_directionalLightActive = false;
			return;
		}
		if (CachedDirectionalLight == null)
		{
			CachedDirectionalLight = RenderSettings.sun;
		}
		if (CachedDirectionalLight == null)
		{
			return;
		}
		if (component.MainLightColor.overrideState)
		{
			CachedDirectionalLight.color = component.MainLightColor.value;
		}
		if (component.MainLightIntensity.overrideState)
		{
			CachedDirectionalLight.intensity = component.MainLightIntensity.value;
		}
		if (component.MainLightRotationX.overrideState && component.MainLightRotationY.overrideState)
		{
			float x = component.MainLightRotationX.value;
			float y = Mathf.Repeat(component.MainLightRotationY.value + 225f, 360f);
			CachedDirectionalLight.transform.rotation = Quaternion.Euler(x, y, 0f);
		}
		_directionalLightActive = true;
	}

	private void UpdateDialogColor(VolumeStack volumeStack)
	{
		DialogVisualVolumeComponent component = volumeStack.GetComponent<DialogVisualVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalColor(ShaderIDs.DialogBaseColorProperty, Color.white);
			Shader.SetGlobalColor(ShaderIDs.DialogRimLightColorProperty, Color.white);
			Shader.SetGlobalFloat(ShaderIDs.DialogRimLightAngleProperty, 0f);
			Shader.SetGlobalFloat(ShaderIDs.DialogRimLightOffsetProperty, 0f);
			_dialogColorActive = false;
			return;
		}
		Shader.SetGlobalColor(ShaderIDs.DialogBaseColorProperty, component.BaseColor.value);
		Shader.SetGlobalColor(ShaderIDs.DialogRimLightColorProperty, component.RimLightColor.value);
		Shader.SetGlobalFloat(ShaderIDs.DialogRimLightAngleProperty, component.RimLightAngle.value);
		Shader.SetGlobalFloat(ShaderIDs.DialogRimLightOffsetProperty, component.RimLightOffset.value);
		_dialogColorActive = true;
	}

	private void UpdateShadowColor(VolumeStack volumeStack)
	{
		FakeShadowVolumeComponent component = volumeStack.GetComponent<FakeShadowVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalFloat(ShaderIDs.BackgroundNormalIntensityProperty, 1f);
			Shader.SetGlobalColor(ShaderIDs.BackgroundShadowColorProperty, new Color(0.5f, 0.5f, 0.6f, 1f));
			Shader.SetGlobalColor(ShaderIDs.CharacterShadowColorProperty, new Color(0.2f, 0.2f, 0.3f, 1f));
			Shader.SetGlobalColor(ShaderIDs.CharacterOutOfColorProperty, new Color(0.1f, 0.1f, 0.1f, 1f));
			_shadowColorActive = false;
			return;
		}
		Shader.SetGlobalFloat(ShaderIDs.BackgroundNormalIntensityProperty, component.BackgroundNomralIntensity.value);
		Shader.SetGlobalColor(ShaderIDs.BackgroundShadowColorProperty, component.BackgroundShadowColor.value);
		Shader.SetGlobalColor(ShaderIDs.CharacterShadowColorProperty, component.CharacterShadowColor.value);
		Shader.SetGlobalColor(ShaderIDs.CharacterOutOfColorProperty, component.CharacterOutOfSightColor.value);
		_shadowColorActive = true;
	}
}
