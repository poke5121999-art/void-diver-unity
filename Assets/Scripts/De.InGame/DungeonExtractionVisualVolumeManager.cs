using UnityEngine;
using UnityEngine.Rendering;

[DefaultExecutionOrder(-100)]
[ExecuteAlways]
public class DungeonExtractionVisualVolumeManager : CustomVisualVolumeManager
{
	private static class ShaderIDs
	{
		public static readonly int BackgroundColor;

		public static readonly int BackgroundColorIntensity;

		public static readonly int FogHeightEnable;

		public static readonly int FogHeightMax;

		public static readonly int FogHeightMin;

		public static readonly int DialogBaseColorProperty;

		public static readonly int DialogRimLightColorProperty;

		public static readonly int DialogRimLightAngleProperty;

		public static readonly int DialogRimLightOffsetProperty;

		public static readonly int WeatherIntensity;

		public static readonly int WeatherIndoorMask;

		public static readonly int WeatherFogColor;

		public static readonly int WeatherFogVignetteCenter;

		public static readonly int WeatherFogVignetteStart;

		public static readonly int WeatherFogVignetteEnd;

		public static readonly int WeatherFogVignetteIntensity;

		public static readonly int WeatherFogNoiseScale;

		public static readonly int WeatherFogNoiseSpeed;

		public static readonly int WeatherFogNoiseIntensity;

		public static readonly int WeatherStreakAngle;

		public static readonly int WeatherStreakSpeed;

		public static readonly int WeatherStreakDensity;

		public static readonly int WeatherStreakProbability;

		public static readonly int WeatherStreakThickness;

		public static readonly int WeatherStreakLength;

		public static readonly int WeatherStreakBrightness;

		public static readonly int WeatherStreakColor;

		public static readonly int WeatherStreakCellShiftSpeed;

		public static readonly int WeatherSplashScale;

		public static readonly int WeatherSplashSpeed;

		public static readonly int WeatherSplashIntensity;

		public static readonly int WeatherSplashSize;

		public static readonly int WeatherSplashCellShiftSpeed;

		public static readonly int WeatherHazeSpeed;

		public static readonly int WeatherHazeStrength;

		public static readonly int WeatherHazeNoiseScale;

		public static readonly int WeatherHazeBlurBlend;

		public static readonly int WeatherHazeVerticalBias;

		public static readonly int WeatherHazeVignetteStart;

		public static readonly int WeatherHazeVignetteEnd;

		public static readonly int WeatherStreakBlendMode;

		public static readonly int WeatherStreakCameraParallax;

		public static readonly int WeatherDropletIntensity;

		public static readonly int WeatherDropletScale;

		public static readonly int WeatherDropletSpeed;

		public static readonly int WeatherDropletDistortion;

		public static readonly int WeatherDropletSize;

		public static readonly int WeatherDropletVignetteStart;

		public static readonly int WeatherDropletVignetteEnd;

		public static readonly int WeatherSplashColor;

		public static readonly int WeatherSplashBlendMode;

		public static readonly int WeatherFogBlendMode;

		public static readonly int WeatherFogIndoorFactor;

		public static readonly int WeatherBurnBlendMode;

		public static readonly int WeatherBurnIntensity;

		public static readonly int WeatherBurnColor;

		public static readonly int WeatherBurnSpeed;

		public static readonly int WeatherBurnDirection;

		public static readonly int WeatherBurnNoiseScale;

		public static readonly int WeatherBurnVignetteStart;

		public static readonly int WeatherBurnVignetteEnd;

		public static readonly int WeatherFrostOverlayIntensity;

		public static readonly int WeatherFrostOverlayStart;

		public static readonly int WeatherFrostOverlayEnd;

		public static readonly int WeatherFrostOverlayNoiseScale;

		public static readonly int WeatherFrostOverlayColor;

		public static readonly int WeatherWetSurfaceIntensity;

		public static readonly int WeatherRippleStrength;

		public static readonly int WeatherBgTintColor;

		public static readonly int WeatherBgTintIntensity;

		public static readonly int WeatherBgDesaturation;

		public static readonly int BackgroundShadowColorProperty;

		public static readonly int BackgroundNormalIntensityProperty;

		public static readonly int CharacterShadowColorProperty;

		public static readonly int CharacterOutOfColorProperty;
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
	}

	protected override void SetDisableVisualEffects()
	{
	}

	private void UpdateBackgroundColor(VolumeStack volumeStack)
	{
	}

	private void UpdateWeatherVisual(VolumeStack volumeStack)
	{
	}

	private void UpdateDistanceFog(VolumeStack volumeStack)
	{
	}

	private void UpdateHeightFog(VolumeStack volumeStack)
	{
	}

	private void UpdateCameraBackgroundColor(VolumeStack volumeStack)
	{
	}

	private void UpdateDirectionalLight(VolumeStack volumeStack)
	{
	}

	private void UpdateDialogColor(VolumeStack volumeStack)
	{
	}

	private void UpdateShadowColor(VolumeStack volumeStack)
	{
	}
}
