using UnityEngine.Playables;

public class CutsceneUnitAnimationBehavior : PlayableBehaviour
{
	public string Animation;

	public bool IsLoop;

	private CutsceneUnit _targetSpineComponent;

	private bool _firstFrameHappened;

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}
}
