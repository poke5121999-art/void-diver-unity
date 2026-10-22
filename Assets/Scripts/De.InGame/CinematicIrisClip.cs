using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CinematicIrisClip : PlayableAsset, ITimelineClipAsset
{
	[Tooltip("Iris 닫힘 정도 (0 = 열림, 1 = 완전 닫힘)")]
	[Range(0f, 1f)]
	public float Progress = 1f;

	[Tooltip("경계 부드러움")]
	[Range(0.001f, 0.3f)]
	public float Softness = 0.05f;

	[Tooltip("Iris 중심점 (UV 좌표)")]
	public Vector2 Center = new Vector2(0.5f, 0.5f);

	[Tooltip("Iris 마스크 색상")]
	public Color IrisColor = new Color(0f, 0f, 0f, 1f);

	public ClipCaps clipCaps => ClipCaps.Blending;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CinematicIrisBehaviour> scriptPlayable = ScriptPlayable<CinematicIrisBehaviour>.Create(graph);
		CinematicIrisBehaviour behaviour = scriptPlayable.GetBehaviour();
		behaviour.Progress = Progress;
		behaviour.Softness = Softness;
		behaviour.Center = Center;
		behaviour.IrisColor = IrisColor;
		return scriptPlayable;
	}
}
