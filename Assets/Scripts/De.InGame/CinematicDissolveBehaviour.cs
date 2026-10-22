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
		double duration = playable.GetDuration();
		double time = playable.GetTime();
		float progress = 0f;
		if (duration > 0.0)
		{
			progress = (float)(time / duration);
			if (progress >= 0f)
			{
				if (progress > 1f)
				{
					progress = 1f;
				}
			}
			else
			{
				progress = 0f;
			}
		}
		if (ProgressCurve != null)
		{
			progress = ProgressCurve.Evaluate(progress);
		}
		Shader.SetGlobalFloat(CinematicEffectConsts.DISSOLVE_PROGRESS, info.weight * progress);
		Shader.SetGlobalFloat(CinematicEffectConsts.DISSOLVE_EDGE_WIDTH, EdgeWidth);
		float screenRatio = 1f;
		if (Screen.height > 0)
		{
			screenRatio = (float)Screen.width / (float)Screen.height;
		}
		Vector4 targetParams;
		if (TargetTexture != null)
		{
			Shader.SetGlobalTexture(CinematicEffectConsts.DISSOLVE_TARGET, TargetTexture);
			targetParams = ComposeFinalParams(TargetTexture, screenRatio, TargetTransform);
		}
		else
		{
			targetParams = new Vector4(1f, 1f, 0f, 0f);
		}
		Shader.SetGlobalVector(CinematicEffectConsts.DISSOLVE_TARGET_PARAMS, targetParams);
		Vector4 maskParams;
		if (MaskTexture != null)
		{
			Shader.SetGlobalTexture(CinematicEffectConsts.DISSOLVE_MASK, MaskTexture);
			Texture2D reference = (TargetTexture != null) ? TargetTexture : MaskTexture;
			maskParams = ComposeFinalParams(reference, screenRatio, MaskTransform);
		}
		else
		{
			maskParams = new Vector4(1f, 1f, 0f, 0f);
		}
		Shader.SetGlobalVector(CinematicEffectConsts.DISSOLVE_MASK_PARAMS, maskParams);
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Shader.SetGlobalFloat(CinematicEffectConsts.DISSOLVE_PROGRESS, EndProgress);
	}

	private static Vector4 ComposeFinalParams(Texture2D texture, float screenRatio, Vector4 userTransform)
	{
		float textureRatio = (texture.height > 0) ? ((float)texture.width / (float)texture.height) : 1f;
		float scaleX = 1f;
		float scaleY = 1f;
		float offsetX = 0f;
		float offsetY = 0f;
		if (screenRatio < textureRatio)
		{
			scaleX = screenRatio / textureRatio;
			offsetX = (1f - scaleX) * 0.5f;
		}
		else if (textureRatio < screenRatio)
		{
			scaleY = textureRatio / screenRatio;
			offsetY = (1f - scaleY) * 0.5f;
		}
		float userScaleX = (Mathf.Abs(userTransform.x) >= 0.0001f) ? userTransform.x : 0.0001f;
		float userScaleY = (Mathf.Abs(userTransform.y) >= 0.0001f) ? userTransform.y : 0.0001f;
		Vector4 result = default(Vector4);
		result.x = scaleX / userScaleX;
		result.y = scaleY / userScaleY;
		result.z = (offsetX - 0.5f) / userScaleX + 0.5f - userTransform.z;
		result.w = (offsetY - 0.5f) / userScaleY + 0.5f - userTransform.w;
		return result;
	}

	private static Vector4 ComputeCoverScaleOffset(Texture2D texture, float screenRatio)
	{
		Vector4 result = default(Vector4);
		float textureRatio = (texture.height > 0) ? ((float)texture.width / (float)texture.height) : 1f;
		result = new Vector4(1f, 1f, 0f, 0f);
		if (screenRatio < textureRatio)
		{
			result.x = screenRatio / textureRatio;
			result.z = (1f - screenRatio / textureRatio) * 0.5f;
		}
		else if (textureRatio < screenRatio)
		{
			result.y = textureRatio / screenRatio;
			result.w = (1f - textureRatio / screenRatio) * 0.5f;
		}
		return result;
	}
}
