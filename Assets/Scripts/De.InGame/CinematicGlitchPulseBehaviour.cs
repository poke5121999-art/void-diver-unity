using UnityEngine;
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
		Shader.SetGlobalFloat(CinematicEffectConsts.GLITCH_PULSE_INTENSITY, info.weight * Intensity);
		Shader.SetGlobalFloat(CinematicEffectConsts.GLITCH_PULSE_DEGREE, Degree);
		Shader.SetGlobalFloat(CinematicEffectConsts.GLITCH_PULSE_SCALE, Scale);
		Shader.SetGlobalFloat(CinematicEffectConsts.GLITCH_PULSE_STEP_CUTOFF, StepCutoff);
		Shader.SetGlobalFloat(CinematicEffectConsts.GLITCH_PULSE_FLICKER_STEP_CUTOFF, FlickerStepCutoff);
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Shader.SetGlobalFloat(CinematicEffectConsts.GLITCH_PULSE_INTENSITY, 0f);
	}
}
