using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

[DefaultExecutionOrder(-100)]
[ExecuteAlways]
public class BackgroundBoxAreaEffectManager : MonoBehaviour
{
	private class FadeOutGhost
	{
		public float Intensity;

		public Vector3 Center;

		public Vector3 Extents;

		public Color OverlayColor;

		public float EdgeSmooth;

		public EBackgroundBoxAreaEffectType EffectType;

		public float ShimmerSpeed;

		public float ShimmerStrength;

		public float FogDensity;

		public bool CoverCharacter;

		public bool CoverWorldUI;

		public bool CoverOutline;
	}

	public static BackgroundBoxAreaEffectManager Instance;

	public static Subject<BackgroundBoxAreaEffectManager> OnInstantiated = new Subject<BackgroundBoxAreaEffectManager>();

	private List<BackgroundBoxAreaEffect> _boxAreaEffects = new List<BackgroundBoxAreaEffect>();

	private readonly List<FadeOutGhost> _fadeOutGhosts = new List<FadeOutGhost>();

	private readonly Vector4[] _centers = new Vector4[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private readonly Vector4[] _extents = new Vector4[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private readonly Vector4[] _colors = new Vector4[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private readonly float[] _intensities = new float[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private readonly Vector4[] _shimmerParams = new Vector4[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private readonly float[] _coverCharacters = new float[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private readonly float[] _coverWorldUIs = new float[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private readonly float[] _coverOutlines = new float[BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX];

	private void OnEnable()
	{
		if (Instance != null && Instance != this)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(gameObject);
			}
			else
			{
				Object.DestroyImmediate(gameObject);
			}
			return;
		}
		Instance = this;
		OnInstantiated.OnNext(this);
	}

	private void OnDisable()
	{
		Shader.SetGlobalFloat(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_ENABLE, 0f);
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		Shader.SetGlobalFloat(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_ENABLE, 0f);
	}

	public void RegisterToManager(BackgroundBoxAreaEffect effect)
	{
		if (!_boxAreaEffects.Contains(effect))
		{
			_boxAreaEffects.Add(effect);
		}
	}

	public void UnregisterFromManager(BackgroundBoxAreaEffect effect)
	{
		_boxAreaEffects.Remove(effect);
	}

	public void FadeOutAndUnregister(BackgroundBoxAreaEffect effect, float startIntensity, float delay, float duration, Ease ease)
	{
		_boxAreaEffects.Remove(effect);
		FadeOutGhost ghost = new FadeOutGhost
		{
			Intensity = startIntensity,
			Center = effect.Center,
			Extents = effect.Extents,
			OverlayColor = effect.OverlayColor,
			EdgeSmooth = effect.EdgeSmooth,
			EffectType = effect.EffectType,
			ShimmerSpeed = effect.ShimmerSpeed,
			ShimmerStrength = effect.ShimmerStrength,
			FogDensity = effect.FogDensity,
			CoverCharacter = effect.CoverCharacter,
			CoverWorldUI = effect.CoverWorldUI,
			CoverOutline = effect.CoverOutline
		};
		_fadeOutGhosts.Add(ghost);
		DOTween.To(() => ghost.Intensity, delegate(float v)
		{
			ghost.Intensity = v;
		}, 0f, duration).SetDelay(delay).SetEase(ease).OnComplete(delegate
		{
			_fadeOutGhosts.Remove(ghost);
		});
	}

	public static bool IsInsideCoverWorldUI(Vector3 worldPos)
	{
		if (Instance == null)
		{
			return false;
		}
		foreach (BackgroundBoxAreaEffect boxAreaEffect in Instance._boxAreaEffects)
		{
			if (boxAreaEffect.CoverWorldUI)
			{
				Vector3 center = boxAreaEffect.Center;
				Vector3 extents = boxAreaEffect.Extents;
				if (Mathf.Abs(worldPos.x - center.x) - extents.x <= 0f
					&& Mathf.Abs(worldPos.y - center.y) - extents.y <= 0f
					&& Mathf.Abs(worldPos.z - center.z) - extents.z <= 0f)
				{
					return true;
				}
			}
		}
		foreach (FadeOutGhost fadeOutGhost in Instance._fadeOutGhosts)
		{
			if (fadeOutGhost.CoverWorldUI
				&& Mathf.Abs(worldPos.x - fadeOutGhost.Center.x) - fadeOutGhost.Extents.x <= 0f
				&& Mathf.Abs(worldPos.y - fadeOutGhost.Center.y) - fadeOutGhost.Extents.y <= 0f
				&& Mathf.Abs(worldPos.z - fadeOutGhost.Center.z) - fadeOutGhost.Extents.z <= 0f)
			{
				return true;
			}
		}
		return false;
	}

	private void LateUpdate()
	{
		UpdateGlobalProperties();
	}

	private void UpdateGlobalProperties()
	{
		int num = Mathf.Min(_boxAreaEffects.Count, BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX);
		int num2 = Mathf.Min(_fadeOutGhosts.Count, BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX - num);
		bool flag = false;
		for (int i = 0; i < BackgroundBoxAreaEffectConsts.NUMBER_BOX_AREA_COUNT_MAX; i++)
		{
			if (i < num + num2)
			{
				float intensity;
				Vector3 center;
				Vector3 extents;
				Color overlayColor;
				float edgeSmooth;
				EBackgroundBoxAreaEffectType effectType;
				float shimmerSpeed;
				float shimmerStrength;
				float fogDensity;
				bool coverCharacter;
				bool coverWorldUI;
				bool coverOutline;
				if (i < num)
				{
					BackgroundBoxAreaEffect effect = _boxAreaEffects[i];
					intensity = effect.Intensity;
					center = effect.Center;
					extents = effect.Extents;
					overlayColor = effect.OverlayColor;
					effectType = effect.EffectType;
					edgeSmooth = effect.EdgeSmooth;
					shimmerSpeed = effect.ShimmerSpeed;
					shimmerStrength = effect.ShimmerStrength;
					fogDensity = effect.FogDensity;
					coverCharacter = effect.CoverCharacter;
					coverWorldUI = effect.CoverWorldUI;
					coverOutline = effect.CoverOutline;
				}
				else
				{
					FadeOutGhost ghost = _fadeOutGhosts[i - num];
					overlayColor = ghost.OverlayColor;
					center = ghost.Center;
					intensity = ghost.Intensity;
					extents = ghost.Extents;
					edgeSmooth = ghost.EdgeSmooth;
					effectType = ghost.EffectType;
					shimmerSpeed = ghost.ShimmerSpeed;
					shimmerStrength = ghost.ShimmerStrength;
					fogDensity = ghost.FogDensity;
					coverCharacter = ghost.CoverCharacter;
					coverWorldUI = ghost.CoverWorldUI;
					coverOutline = ghost.CoverOutline;
				}
				_intensities[i] = intensity;
				_coverCharacters[i] = (coverCharacter ? 1f : 0f);
				_coverWorldUIs[i] = (coverWorldUI ? 1f : 0f);
				_coverOutlines[i] = (coverOutline ? 1f : 0f);
				if (intensity >= 0.001f || coverCharacter || coverWorldUI || coverOutline)
				{
					flag = true;
					_centers[i] = new Vector4(center.x, center.y, center.z, (float)effectType);
					_extents[i] = new Vector4(extents.x, extents.y, extents.z, edgeSmooth);
					_colors[i] = new Vector4(overlayColor.r, overlayColor.g, overlayColor.b, overlayColor.a);
					_shimmerParams[i] = new Vector4(shimmerSpeed, shimmerStrength, fogDensity, 0f);
				}
			}
			else
			{
				_intensities[i] = 0f;
				_extents[i] = Vector4.zero;
				_coverCharacters[i] = 0f;
				_coverWorldUIs[i] = 0f;
				_coverOutlines[i] = 0f;
			}
		}
		if (flag)
		{
			Shader.SetGlobalFloat(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_ENABLE, 1f);
			Shader.SetGlobalVectorArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_CENTERS, _centers);
			Shader.SetGlobalVectorArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_EXTENTS, _extents);
			Shader.SetGlobalVectorArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_COLORS, _colors);
			Shader.SetGlobalFloatArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_INTENSITIES, _intensities);
			Shader.SetGlobalVectorArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_SHIMMER_PARAMS, _shimmerParams);
			Shader.SetGlobalFloatArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_COVER_CHARACTERS, _coverCharacters);
			Shader.SetGlobalFloatArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_COVER_WORLD_UIS, _coverWorldUIs);
			Shader.SetGlobalFloatArray(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_COVER_OUTLINES, _coverOutlines);
		}
		else
		{
			Shader.SetGlobalFloat(BackgroundBoxAreaEffectConsts.SHADER_PROPERTY_ENABLE, 0f);
		}
	}
}
