using UnityEngine;
using UnityEngine.Playables;

public class CutsceneCharacterEffectBehaviour : PlayableBehaviour
{
	public Color EffectColor;

	public float EffectIntensity;

	private CutsceneCharacterMaterialController _controller;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		_controller = playerData as CutsceneCharacterMaterialController;
		if (!(_controller == null) && _controller.Initialize())
		{
			_controller.ApplyEffect(EffectColor, info.weight * EffectIntensity);
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (_controller != null)
		{
			_controller.ResetEffect();
		}
	}
}
