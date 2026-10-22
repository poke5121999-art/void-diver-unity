using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicMotionSickClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("멀미 효과 강도")]
	[Range(0f, 1f)]
	public float Intensity;

	[Tooltip("회전 주기 (초)")]
	public float RotationDuration;

	[Tooltip("회전 각도")]
	public float RotationAngle;

	[Tooltip("펄스 주기 (초)")]
	public float PulseDuration;

	[Tooltip("펄스 정도")]
	public float PulseDegree;

	[Tooltip("흔들림 정도")]
	public float ShakeDegree;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
