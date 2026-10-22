using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CutsceneCharacterEffectClip : PlayableAsset, ITimelineClipAsset
{
	[ColorUsage(false, true)]
	[Tooltip("이펙트 컬러 (HDR)")]
	public Color EffectColor = Color.white;

	[Range(0f, 1f)]
	[Tooltip("이펙트 강도 (0 = 비활성)")]
	public float EffectIntensity;

	public ClipCaps clipCaps => ClipCaps.Blending;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CutsceneCharacterEffectBehaviour> playable = ScriptPlayable<CutsceneCharacterEffectBehaviour>.Create(graph);
		CutsceneCharacterEffectBehaviour behaviour = playable.GetBehaviour();
		behaviour.EffectColor = EffectColor;
		behaviour.EffectIntensity = EffectIntensity;
		return playable;
	}
}
