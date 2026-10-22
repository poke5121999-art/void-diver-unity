using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class InteriorDissolveClip : PlayableAsset, ITimelineClipAsset
{
	public DissolveDirection Direction;

	[Tooltip("클립 진행도(0~1) → dissolve progress(0~0.5) 커브")]
	public AnimationCurve ProgressCurve;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
