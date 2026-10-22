using UnityEngine;
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
		Shader.SetGlobalFloat(CinematicEffectConsts.MOTION_SICK_INTENSITY, info.weight * Intensity);
		Shader.SetGlobalFloat(CinematicEffectConsts.MOTION_SICK_ROTATION_DURATION, RotationDuration);
		Shader.SetGlobalFloat(CinematicEffectConsts.MOTION_SICK_ROTATION_ANGLE, RotationAngle);
		Shader.SetGlobalFloat(CinematicEffectConsts.MOTION_SICK_PULSE_DURATION, PulseDuration);
		Shader.SetGlobalFloat(CinematicEffectConsts.MOTION_SICK_PULSE_DEGREE, PulseDegree);
		Shader.SetGlobalFloat(CinematicEffectConsts.MOTION_SICK_SHAKE_DEGREE, ShakeDegree);
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Shader.SetGlobalFloat(CinematicEffectConsts.MOTION_SICK_INTENSITY, 0f);
	}
}
