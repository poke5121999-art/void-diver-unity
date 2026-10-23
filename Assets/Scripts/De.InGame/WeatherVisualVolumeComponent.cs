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
	public ClampedFloatParameter Intensity = new ClampedFloatParameter(0f, 0f, 1f);

	[Tooltip("실내 마스크 (0=실외, 1=실내)")]
	public ClampedFloatParameter IndoorMask = new ClampedFloatParameter(0f, 0f, 1f);

	[Header("안개")]
	public ColorParameter FogColor = new ColorParameter(new Color(0.4f, 0.4f, 0.45f, 1f), hdr: false, showAlpha: false, showEyeDropper: true);

	public Vector2Parameter FogVignetteCenter = new Vector2Parameter(new Vector2(0.5f, 0.5f));

	public FloatParameter FogVignetteStart = new FloatParameter(0.3f);

	public FloatParameter FogVignetteEnd = new FloatParameter(0.8f);

	public ClampedFloatParameter FogVignetteIntensity = new ClampedFloatParameter(0f, 0f, 1f);

	public FloatParameter FogNoiseScale = new FloatParameter(2f);

	public FloatParameter FogNoiseSpeed = new FloatParameter(0.05f);

	public ClampedFloatParameter FogNoiseIntensity = new ClampedFloatParameter(0.3f, 0f, 1f);

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter FogBlendMode = new ClampedFloatParameter(0f, 0f, 1f);

	[Tooltip("0=실내 안개 없음, 1=실내도 동일")]
	public ClampedFloatParameter FogIndoorFactor = new ClampedFloatParameter(0.5f, 0f, 1f);

	[Header("빗줄기/입자")]
	public FloatParameter StreakAngle = new FloatParameter(65f);

	public FloatParameter StreakSpeed = new FloatParameter(1.5f);

	public FloatParameter StreakDensity = new FloatParameter(40f);

	public ClampedFloatParameter StreakProbability = new ClampedFloatParameter(0.25f, 0f, 1f);

	public FloatParameter StreakThickness = new FloatParameter(0.02f);

	public FloatParameter StreakLength = new FloatParameter(0.6f);

	public FloatParameter StreakBrightness = new FloatParameter(0f);

	public ColorParameter StreakColor = new ColorParameter(new Color(0.7f, 0.8f, 1f, 1f), hdr: false, showAlpha: false, showEyeDropper: true);

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter StreakBlendMode = new ClampedFloatParameter(0f, 0f, 1f);

	public FloatParameter StreakCellShiftSpeed = new FloatParameter(0.2f);

	public Vector2Parameter StreakCameraParallax = new Vector2Parameter(new Vector2(0.1f, 0.1f));

	[Header("화면 물방울")]
	public ClampedFloatParameter DropletIntensity = new ClampedFloatParameter(0f, 0f, 1f);

	public FloatParameter DropletScale = new FloatParameter(3f);

	public FloatParameter DropletSpeed = new FloatParameter(0.3f);

	public FloatParameter DropletDistortion = new FloatParameter(0.05f);

	public FloatParameter DropletSize = new FloatParameter(0.12f);

	public FloatParameter DropletVignetteStart = new FloatParameter(0.3f);

	public FloatParameter DropletVignetteEnd = new FloatParameter(0.8f);

	[Header("스플래시")]
	public FloatParameter SplashScale = new FloatParameter(5f);

	public FloatParameter SplashSpeed = new FloatParameter(3f);

	public ClampedFloatParameter SplashIntensity = new ClampedFloatParameter(0f, 0f, 1f);

	public ColorParameter SplashColor = new ColorParameter(new Color(1f, 1f, 1f, 1f), hdr: false, showAlpha: false, showEyeDropper: true);

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter SplashBlendMode = new ClampedFloatParameter(0f, 0f, 1f);

	public FloatParameter SplashSize = new FloatParameter(0.15f);

	public FloatParameter SplashCellShiftSpeed = new FloatParameter(0.3f);

	[Header("UV 왜곡 (아지랑이)")]
	public FloatParameter HazeSpeed = new FloatParameter(1f);

	public FloatParameter HazeStrength = new FloatParameter(0f);

	public FloatParameter HazeNoiseScale = new FloatParameter(3f);

	public ClampedFloatParameter HazeBlurBlend = new ClampedFloatParameter(0.5f, 0f, 1f);

	public FloatParameter HazeVerticalBias = new FloatParameter(1.5f);

	public FloatParameter HazeVignetteStart = new FloatParameter(0.2f);

	public FloatParameter HazeVignetteEnd = new FloatParameter(0.8f);

	[Header("타는 효과")]
	public ClampedFloatParameter BurnIntensity = new ClampedFloatParameter(0f, 0f, 1f);

	public ColorParameter BurnColor = new ColorParameter(new Color(1f, 0.5f, 0.1f, 1f), hdr: false, showAlpha: false, showEyeDropper: true);

	[Tooltip("0=합(Additive), 1=곱(Multiply)")]
	public ClampedFloatParameter BurnBlendMode = new ClampedFloatParameter(1f, 0f, 1f);

	public FloatParameter BurnSpeed = new FloatParameter(0.4f);

	public Vector2Parameter BurnDirection = new Vector2Parameter(new Vector2(0f, -1f));

	public FloatParameter BurnNoiseScale = new FloatParameter(4f);

	public FloatParameter BurnVignetteStart = new FloatParameter(0.3f);

	public FloatParameter BurnVignetteEnd = new FloatParameter(0.8f);

	[Header("서리 오버레이")]
	public ClampedFloatParameter FrostOverlayIntensity = new ClampedFloatParameter(0f, 0f, 1f);

	public FloatParameter FrostOverlayStart = new FloatParameter(0.5f);

	public FloatParameter FrostOverlayEnd = new FloatParameter(1f);

	public FloatParameter FrostOverlayNoiseScale = new FloatParameter(5f);

	public ColorParameter FrostOverlayColor = new ColorParameter(new Color(0.9f, 0.95f, 1f, 1f), hdr: false, showAlpha: false, showEyeDropper: true);

	[Header("바람")]
	[Tooltip("바람 강도 (Plant_Util에서 수신)")]
	public FloatParameter WindStrength = new FloatParameter(0f);

	[Header("BG: 젖은 표면")]
	public ClampedFloatParameter WetSurfaceIntensity = new ClampedFloatParameter(0f, 0f, 1f);

	public FloatParameter RippleStrength = new FloatParameter(0.02f);

	[Header("BG: 색조 오버레이")]
	public ColorParameter BgTintColor = new ColorParameter(new Color(0.85f, 0.88f, 0.93f, 1f), hdr: false, showAlpha: false, showEyeDropper: true);

	public ClampedFloatParameter BgTintIntensity = new ClampedFloatParameter(0f, 0f, 1f);

	public ClampedFloatParameter BgDesaturation = new ClampedFloatParameter(0.4f, 0f, 1f);

	public bool IsActive()
	{
		return Intensity.value > 0.001f;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
