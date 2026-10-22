using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CutsceneLuaSignalClip : PlayableAsset
{
	public int SignalId;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default;
	}
}
