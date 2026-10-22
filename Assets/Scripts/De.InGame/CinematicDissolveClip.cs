using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicDissolveClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("디졸브 임계값 텍스처 (그레이스케일 0~1). Linear, sRGB OFF 권장. 화면 비율과 달라도 cover 자동 보정")]
	public Texture2D MaskTexture;

	[Tooltip("디졸브 결과 텍스처 (배경 이미지). 화면 비율과 달라도 cover 자동 보정")]
	public Texture2D TargetTexture;

	[Tooltip("0 = 원본, 1 = 완전 대체. 클립 진행도 → progress 곡선")]
	public AnimationCurve ProgressCurve;

	[Tooltip("디졸브 경계 부드러움 (0 = 하드 컷, 0.5 = 매우 부드러움)")]
	[Range(0f, 0.5f)]
	public float EdgeWidth;

	[Tooltip("클립 종료 후 고정될 progress 값 (0 = 원본, 1 = 완전 대체)")]
	[Range(0f, 1f)]
	public float EndProgress;

	[Tooltip("Mask 추가 Scale(xy)/Offset(zw). cover 결과 위에 합성. Scale>1 = 줌인, Offset = UV 패닝")]
	public Vector4 MaskTransform;

	[Tooltip("Target 추가 Scale(xy)/Offset(zw). cover 결과 위에 합성. Scale>1 = 줌인, Offset = UV 패닝")]
	public Vector4 TargetTransform;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
