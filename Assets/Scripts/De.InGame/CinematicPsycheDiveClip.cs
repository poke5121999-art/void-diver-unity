using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicPsycheDiveClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("사이키다이브 효과 강도")]
	[Range(0f, 1f)]
	public float Intensity = 0.5f;

	[Tooltip("포스터라이즈 임계값 오프셋")]
	public float Offset = 0.04f;

	public ClipCaps clipCaps => ClipCaps.Blending;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CinematicPsycheDiveBehaviour> playable = ScriptPlayable<CinematicPsycheDiveBehaviour>.Create(graph);
		CinematicPsycheDiveBehaviour behaviour = playable.GetBehaviour();
		behaviour.Intensity = Intensity;
		behaviour.Offset = Offset;
		return playable;
	}
}
