using UnityEngine;
using UnityEngine.Playables;

public class CutsceneCharacterDeathBehaviour : PlayableBehaviour
{
	public Color DeathColor;

	public float DeathProgress;

	private CutsceneCharacterMaterialController _controller;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		_controller = playerData as CutsceneCharacterMaterialController;
		if (!(_controller == null) && _controller.Initialize())
		{
			_controller.ApplyDeath(DeathColor, info.weight * DeathProgress);
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (_controller != null)
		{
			_controller.ResetDeath();
		}
	}
}
