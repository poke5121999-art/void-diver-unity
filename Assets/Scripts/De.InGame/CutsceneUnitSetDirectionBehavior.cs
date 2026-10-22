using GameCommon.Enum;
using UnityEngine.Playables;

public class CutsceneUnitSetDirectionBehavior : PlayableBehaviour
{
	public ECutsceneUnitDirection Direction;

	private CutsceneUnit _targetSpineComponent;

	private bool _firstFrameHappened;

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}
}
