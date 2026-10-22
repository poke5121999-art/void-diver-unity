using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicPsycheDiveClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("사이키다이브 효과 강도")]
	[Range(0f, 1f)]
	public float Intensity;

	[Tooltip("포스터라이즈 임계값 오프셋")]
	public float Offset;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
