using UnityEngine;
using UnityEngine.Playables;

public class CutsceneUnitAnimationBehavior : PlayableBehaviour
{
	public string Animation;

	public bool IsLoop;

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
			if (_targetSpineComponent != null)
			{
				_targetSpineComponent.PlayAnimation(Animation, IsLoop);
			}
			_firstFrameHappened = true;
		}
	}
}
