using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CutsceneCharacterEffectClip : PlayableAsset, ITimelineClipAsset
{
	[ColorUsage(false, true)]
	[Tooltip("이펙트 컬러 (HDR)")]
	public Color EffectColor;

	[Range(0f, 1f)]
	[Tooltip("이펙트 강도 (0 = 비활성)")]
	public float EffectIntensity;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
