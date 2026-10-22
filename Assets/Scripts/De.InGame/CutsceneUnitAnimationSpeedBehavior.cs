using UnityEngine.Playables;

public class CutsceneUnitAnimationSpeedBehavior : PlayableBehaviour
{
	public float Speed;

	private CutsceneUnit _targetSpineComponent;

	private bool _firstFrameHappened;

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		_firstFrameHappened = false;
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (!_firstFrameHappened)
		{
			_targetSpineComponent = playerData as CutsceneUnit;
			_targetSpineComponent.SetAnimationSpeed(Speed);
			_firstFrameHappened = true;
		}
	}
}
