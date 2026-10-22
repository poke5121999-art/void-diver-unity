using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicIrisClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("Iris 닫힘 정도 (0 = 열림, 1 = 완전 닫힘)")]
	[Range(0f, 1f)]
	public float Progress;

	[Tooltip("경계 부드러움")]
	[Range(0.001f, 0.3f)]
	public float Softness;

	[Tooltip("Iris 중심점 (UV 좌표)")]
	public Vector2 Center;

	[Tooltip("Iris 마스크 색상")]
	public Color IrisColor;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
