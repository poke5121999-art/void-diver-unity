using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicGlitchPulseClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("글리치 효과 강도")]
	[Range(0f, 1f)]
	public float Intensity = 0.5f;

	[Tooltip("글리치 UV 오프셋 정도")]
	public float Degree = 0.005f;

	[Tooltip("글리치 노이즈 스케일")]
	public float Scale = 50f;

	[Tooltip("글리치 발생 임계값 (낮을수록 자주 발생)")]
	[Range(0f, 1f)]
	public float StepCutoff = 0.5f;

	[Tooltip("깜빡임 발생 임계값")]
	[Range(0f, 0.1f)]
	public float FlickerStepCutoff = 0.02f;

	public ClipCaps clipCaps => ClipCaps.Blending;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CinematicGlitchPulseBehaviour> scriptPlayable = ScriptPlayable<CinematicGlitchPulseBehaviour>.Create(graph);
		CinematicGlitchPulseBehaviour behaviour = scriptPlayable.GetBehaviour();
		behaviour.Intensity = Intensity;
		behaviour.Degree = Degree;
		behaviour.Scale = Scale;
		behaviour.StepCutoff = StepCutoff;
		behaviour.FlickerStepCutoff = FlickerStepCutoff;
		return scriptPlayable;
	}
}
