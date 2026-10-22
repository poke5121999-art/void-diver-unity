using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CutsceneUnitAnimationClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("재생할 스파인 애니메이션을 선택합니다.")]
	public string Animation;

	[Tooltip("애니메이션을 루프할지 여부를 선택합니다.")]
	public bool IsLoop;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CutsceneUnitAnimationBehavior> playable = ScriptPlayable<CutsceneUnitAnimationBehavior>.Create(graph);
		CutsceneUnitAnimationBehavior behaviour = playable.GetBehaviour();
		behaviour.Animation = Animation;
		behaviour.IsLoop = IsLoop;
		return playable;
	}
}
