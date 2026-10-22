using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Project DE/Weather Visual")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class WeatherVisualVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("공통")]
	[Tooltip("전체 날씨 효과 강도")]
	public ClampedFloatParameter Intensity;

	[Tooltip("실내 마스크 (0=실외, 1=실내)")]
	public ClampedFloatParameter IndoorMask;

	[Header("안개")]
	public ColorParameter FogColor;

	public Vector2Parameter FogVignetteCenter;

	public FloatParameter FogVignetteStart;

	public FloatParameter FogVignetteEnd;

	public ClampedFloatParameter FogVignetteIntensity;

	public FloatParameter FogNoiseScale;

	public FloatParameter FogNoiseSpeed;

	public ClampedFloatParameter FogNoiseIntensity;

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter FogBlendMode;

	[Tooltip("0=실내 안개 없음, 1=실내도 동일")]
	public ClampedFloatParameter FogIndoorFactor;

	[Header("빗줄기/입자")]
	public FloatParameter StreakAngle;

	public FloatParameter StreakSpeed;

	public FloatParameter StreakDensity;

	public ClampedFloatParameter StreakProbability;

	public FloatParameter StreakThickness;

	public FloatParameter StreakLength;

	public FloatParameter StreakBrightness;

	public ColorParameter StreakColor;

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter StreakBlendMode;

	public FloatParameter StreakCellShiftSpeed;

	public Vector2Parameter StreakCameraParallax;

	[Header("화면 물방울")]
	public ClampedFloatParameter DropletIntensity;

	public FloatParameter DropletScale;

	public FloatParameter DropletSpeed;

	public FloatParameter DropletDistortion;

	public FloatParameter DropletSize;

	public FloatParameter DropletVignetteStart;

	public FloatParameter DropletVignetteEnd;

	[Header("스플래시")]
	public FloatParameter SplashScale;

	public FloatParameter SplashSpeed;

	public ClampedFloatParameter SplashIntensity;

	public ColorParameter SplashColor;

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter SplashBlendMode;

	public FloatParameter SplashSize;

	public FloatParameter SplashCellShiftSpeed;

	[Header("UV 왜곡 (아지랑이)")]
	public FloatParameter HazeSpeed;

	public FloatParameter HazeStrength;

	public FloatParameter HazeNoiseScale;

	public ClampedFloatParameter HazeBlurBlend;

	public FloatParameter HazeVerticalBias;

	public FloatParameter HazeVignetteStart;

	public FloatParameter HazeVignetteEnd;

	[Header("타는 효과")]
	public ClampedFloatParameter BurnIntensity;

	public ColorParameter BurnColor;

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter BurnBlendMode;

	public FloatParameter BurnSpeed;

	public Vector2Parameter BurnDirection;

	public FloatParameter BurnNoiseScale;

	public FloatParameter BurnVignetteStart;

	public FloatParameter BurnVignetteEnd;

	[Header("서리 오버레이")]
	public ClampedFloatParameter FrostOverlayIntensity;

	public FloatParameter FrostOverlayStart;

	public FloatParameter FrostOverlayEnd;

	public FloatParameter FrostOverlayNoiseScale;

	public ColorParameter FrostOverlayColor;

	[Header("바람")]
	[Tooltip("바람 강도 (Plant_Util에서 수신)")]
	public FloatParameter WindStrength;

	[Header("BG: 젖은 표면")]
	public ClampedFloatParameter WetSurfaceIntensity;

	public FloatParameter RippleStrength;

	[Header("BG: 색조 오버레이")]
	public ColorParameter BgTintColor;

	public ClampedFloatParameter BgTintIntensity;

	public ClampedFloatParameter BgDesaturation;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
