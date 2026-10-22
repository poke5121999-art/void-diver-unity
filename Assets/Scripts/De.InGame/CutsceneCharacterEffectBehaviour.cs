using UnityEngine;
using UnityEngine.Playables;

public class CutsceneCharacterEffectBehaviour : PlayableBehaviour
{
	public Color EffectColor;

	public float EffectIntensity;

	private CutsceneCharacterMaterialController _controller;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
}
