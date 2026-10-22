using UnityEngine.Playables;

public class CutsceneCharacterGhostBehaviour : PlayableBehaviour
{
	private CutsceneCharacterMaterialController _controller;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		_controller = playerData as CutsceneCharacterMaterialController;
		if (!(_controller == null) && _controller.Initialize())
		{
			_controller.ApplyGhost((info.weight < 0.5f) ? 0f : 1f);
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (_controller != null)
		{
			_controller.ResetGhost();
		}
	}
}
