using UnityEngine;
using UnityEngine.Playables;

public class CinematicDissolveBehaviour : PlayableBehaviour
{
	public Texture2D MaskTexture;

	public Texture2D TargetTexture;

	public AnimationCurve ProgressCurve;

	public float EdgeWidth;

	public Vector4 MaskTransform;

	public Vector4 TargetTransform;

	public float EndProgress;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}

	private static Vector4 ComposeFinalParams(Texture2D texture, float screenRatio, Vector4 userTransform)
	{
		return default;
	}

	private static Vector4 ComputeCoverScaleOffset(Texture2D texture, float screenRatio)
	{
		return default;
	}
}
