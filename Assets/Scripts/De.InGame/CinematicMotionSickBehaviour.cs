using UnityEngine.Playables;

public class CinematicMotionSickBehaviour : PlayableBehaviour
{
	public float Intensity;

	public float RotationDuration;

	public float RotationAngle;

	public float PulseDuration;

	public float PulseDegree;

	public float ShakeDegree;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
}
