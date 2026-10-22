using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CutsceneCharacterDeathClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("디졸브 컬러")]
	public Color DeathColor = new Color(0f, 0f, 0f, 1f);

	[Range(0f, 1f)]
	[Tooltip("디졸브 진행도 (0 = 비활성, 1 = 완전 디졸브)")]
	public float DeathProgress;

	public ClipCaps clipCaps => ClipCaps.Blending;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CutsceneCharacterDeathBehaviour> playable = ScriptPlayable<CutsceneCharacterDeathBehaviour>.Create(graph);
		CutsceneCharacterDeathBehaviour behaviour = playable.GetBehaviour();
		behaviour.DeathColor = DeathColor;
		behaviour.DeathProgress = DeathProgress;
		return playable;
	}
}
