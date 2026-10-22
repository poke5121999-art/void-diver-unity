using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CutsceneLuaSignalClip : PlayableAsset
{
	public int SignalId;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CutsceneLuaSignalBehaviour> playable = ScriptPlayable<CutsceneLuaSignalBehaviour>.Create(graph);
		playable.GetBehaviour().SignalId = SignalId;
		return playable;
	}
}
