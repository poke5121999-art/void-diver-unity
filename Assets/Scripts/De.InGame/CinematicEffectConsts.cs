using UnityEngine;

public static class CinematicEffectConsts
{
	public static readonly int IRIS_PROGRESS = Shader.PropertyToID("_CinematicIrisProgress");

	public static readonly int IRIS_SOFTNESS = Shader.PropertyToID("_CinematicIrisSoftness");

	public static readonly int IRIS_CENTER = Shader.PropertyToID("_CinematicIrisCenter");

	public static readonly int IRIS_COLOR = Shader.PropertyToID("_CinematicIrisColor");

	public static readonly int MOTION_SICK_INTENSITY = Shader.PropertyToID("_MotionSickIntensity");

	public static readonly int MOTION_SICK_ROTATION_DURATION = Shader.PropertyToID("_MotionSickRotationDuration");

	public static readonly int MOTION_SICK_ROTATION_ANGLE = Shader.PropertyToID("_MotionSickRotationAngle");

	public static readonly int MOTION_SICK_PULSE_DURATION = Shader.PropertyToID("_MotionSickPulseDuration");

	public static readonly int MOTION_SICK_PULSE_DEGREE = Shader.PropertyToID("_MotionSickPulseDegree");

	public static readonly int MOTION_SICK_SHAKE_DEGREE = Shader.PropertyToID("_MotionSickShakeDegree");

	public static readonly int GLITCH_PULSE_INTENSITY = Shader.PropertyToID("_GlitchPulseIntensity");

	public static readonly int GLITCH_PULSE_DEGREE = Shader.PropertyToID("_GlitchPulseDegree");

	public static readonly int GLITCH_PULSE_SCALE = Shader.PropertyToID("_GlitchPulseScale");

	public static readonly int GLITCH_PULSE_STEP_CUTOFF = Shader.PropertyToID("_GlitchPulseStepCutoff");

	public static readonly int GLITCH_PULSE_FLICKER_STEP_CUTOFF = Shader.PropertyToID("_GlitchPulseFlickerStepCutoff");

	public static readonly int PSYCHE_DIVE_INTENSITY = Shader.PropertyToID("_PsycheDiveIntensity");

	public static readonly int PSYCHE_DIVE_OFFSET = Shader.PropertyToID("_PsycheDiveOffset");

	public static readonly int DISSOLVE_PROGRESS = Shader.PropertyToID("_CinematicDissolveProgress");

	public static readonly int DISSOLVE_EDGE_WIDTH = Shader.PropertyToID("_CinematicDissolveEdgeWidth");

	public static readonly int DISSOLVE_MASK = Shader.PropertyToID("_CinematicDissolveMask");

	public static readonly int DISSOLVE_TARGET = Shader.PropertyToID("_CinematicDissolveTarget");

	public static readonly int DISSOLVE_MASK_PARAMS = Shader.PropertyToID("_CinematicDissolveMaskParams");

	public static readonly int DISSOLVE_TARGET_PARAMS = Shader.PropertyToID("_CinematicDissolveTargetParams");
}
