using UnityEngine;
using UnityEngine.Playables;

public class InteriorDissolvePlayerBehaviour : PlayableBehaviour
{
	public DissolveDirection Direction;

	public AnimationCurve ProgressCurve;

	private DecorationVisualManager _manager;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (_manager == null)
		{
			_manager = playerData as DecorationVisualManager;
		}
		if (_manager == null)
		{
			return;
		}
		double duration = playable.GetDuration();
		float num = ((!(duration > 0.0)) ? 0f : ((float)(playable.GetTime() / duration)));
		if (num < 0f)
		{
			num = 0f;
		}
		else if (num > 1f)
		{
			num = 1f;
		}
		if (ProgressCurve != null)
		{
			num = ProgressCurve.Evaluate(num);
		}
		if (Direction != DissolveDirection.Out)
		{
			num = 1f - num;
		}
		_manager.SetAllProgress(num * 0.5f);
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (_manager != null)
		{
			_manager.SetAllProgress((Direction == DissolveDirection.Out) ? 0.5f : 0f);
		}
	}
}
