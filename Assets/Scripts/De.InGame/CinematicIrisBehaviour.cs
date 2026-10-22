using UnityEngine;
using UnityEngine.Playables;

public class CinematicIrisBehaviour : PlayableBehaviour
{
	public float Progress;

	public float Softness;

	public Vector2 Center;

	public Color IrisColor;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
}
