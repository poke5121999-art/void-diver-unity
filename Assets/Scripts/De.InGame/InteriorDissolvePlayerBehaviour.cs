using UnityEngine;
using UnityEngine.Playables;

public class InteriorDissolvePlayerBehaviour : PlayableBehaviour
{
	public DissolveDirection Direction;

	public AnimationCurve ProgressCurve;

	private DecorationVisualManager _manager;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
}
