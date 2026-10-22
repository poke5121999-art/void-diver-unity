using UnityEngine.Playables;

public class CinematicGlitchPulseBehaviour : PlayableBehaviour
{
	public float Intensity;

	public float Degree;

	public float Scale;

	public float StepCutoff;

	public float FlickerStepCutoff;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
}
