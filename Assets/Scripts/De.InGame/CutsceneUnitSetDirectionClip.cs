using System;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CutsceneUnitSetDirectionClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("방향")]
	public ECutsceneUnitDirection Direction;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CutsceneUnitSetDirectionBehavior> playable = ScriptPlayable<CutsceneUnitSetDirectionBehavior>.Create(graph);
		CutsceneUnitSetDirectionBehavior behaviour = playable.GetBehaviour();
		behaviour.Direction = Direction;
		return playable;
	}
}
