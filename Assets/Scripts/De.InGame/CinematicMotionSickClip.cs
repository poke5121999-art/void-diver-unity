using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicMotionSickClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("멀미 효과 강도")]
	[Range(0f, 1f)]
	public float Intensity = 0.5f;

	[Tooltip("회전 주기 (초)")]
	public float RotationDuration = 8f;

	[Tooltip("회전 각도")]
	public float RotationAngle = 1f;

	[Tooltip("펄스 주기 (초)")]
	public float PulseDuration = 5f;

	[Tooltip("펄스 정도")]
	public float PulseDegree = 0.01f;

	[Tooltip("흔들림 정도")]
	public float ShakeDegree = 0.0005f;

	public ClipCaps clipCaps => ClipCaps.Blending;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CinematicMotionSickBehaviour> scriptPlayable = ScriptPlayable<CinematicMotionSickBehaviour>.Create(graph);
		CinematicMotionSickBehaviour behaviour = scriptPlayable.GetBehaviour();
		behaviour.Intensity = Intensity;
		behaviour.RotationDuration = RotationDuration;
		behaviour.RotationAngle = RotationAngle;
		behaviour.PulseDuration = PulseDuration;
		behaviour.PulseDegree = PulseDegree;
		behaviour.ShakeDegree = ShakeDegree;
		return scriptPlayable;
	}
}
