using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SFXPlayableAsset : PlayableAsset
{
	public string Sfx;

	public Transform Sfx3DTransform;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<SfxPlayableBehaviour> scriptPlayable = ScriptPlayable<SfxPlayableBehaviour>.Create(graph);
		SfxPlayableBehaviour behaviour = scriptPlayable.GetBehaviour();
		behaviour.Sfx = Sfx;
		behaviour.Transform = Sfx3DTransform;
		return scriptPlayable;
	}
}
