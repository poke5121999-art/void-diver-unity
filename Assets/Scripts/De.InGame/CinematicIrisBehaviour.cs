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
		Shader.SetGlobalFloat(CinematicEffectConsts.IRIS_PROGRESS, info.weight * Progress);
		Shader.SetGlobalFloat(CinematicEffectConsts.IRIS_SOFTNESS, Softness);
		Shader.SetGlobalVector(CinematicEffectConsts.IRIS_CENTER, new Vector4(Center.x, Center.y, 0f, 0f));
		Shader.SetGlobalColor(CinematicEffectConsts.IRIS_COLOR, IrisColor);
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Shader.SetGlobalFloat(CinematicEffectConsts.IRIS_PROGRESS, 0f);
	}
}
