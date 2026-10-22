using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CutsceneUnitAnimationSpeedClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("재생 속도")]
	public float Speed;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
