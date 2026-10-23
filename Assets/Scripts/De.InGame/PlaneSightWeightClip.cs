using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PlaneSightWeightClip : PlayableAsset, ITimelineClipAsset
{
	public ClipCaps clipCaps => ClipCaps.Blending;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<PlaneSightWeightBehaviour>.Create(graph);
	}
}
