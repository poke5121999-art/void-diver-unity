using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicGlitchPulseClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("글리치 효과 강도")]
	[Range(0f, 1f)]
	public float Intensity;

	[Tooltip("글리치 UV 오프셋 정도")]
	public float Degree;

	[Tooltip("글리치 노이즈 스케일")]
	public float Scale;

	[Tooltip("글리치 발생 임계값 (낮을수록 자주 발생)")]
	[Range(0f, 1f)]
	public float StepCutoff;

	[Tooltip("깜빡임 발생 임계값")]
	[Range(0f, 0.1f)]
	public float FlickerStepCutoff;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
