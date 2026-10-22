using UnityEngine;
using UnityEngine.Playables;

public class CinematicPsycheDiveBehaviour : PlayableBehaviour
{
	public float Intensity;

	public float Offset;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		Shader.SetGlobalFloat(CinematicEffectConsts.PSYCHE_DIVE_INTENSITY, info.weight * Intensity);
		Shader.SetGlobalFloat(CinematicEffectConsts.PSYCHE_DIVE_OFFSET, Offset);
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Shader.SetGlobalFloat(CinematicEffectConsts.PSYCHE_DIVE_INTENSITY, 0f);
	}
}
