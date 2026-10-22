using UnityEngine.Playables;

public class CutsceneCharacterGhostBehaviour : PlayableBehaviour
{
	private CutsceneCharacterMaterialController _controller;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
}
