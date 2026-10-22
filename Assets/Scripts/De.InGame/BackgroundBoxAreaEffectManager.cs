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

	public static Subject<BackgroundBoxAreaEffectManager> OnInstantiated;

	private List<BackgroundBoxAreaEffect> _boxAreaEffects;

	private readonly List<FadeOutGhost> _fadeOutGhosts;

	private readonly Vector4[] _centers;

	private readonly Vector4[] _extents;

	private readonly Vector4[] _colors;

	private readonly float[] _intensities;

	private readonly Vector4[] _shimmerParams;

	private readonly float[] _coverCharacters;

	private readonly float[] _coverWorldUIs;

	private readonly float[] _coverOutlines;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public void RegisterToManager(BackgroundBoxAreaEffect effect)
	{
	}

	public void UnregisterFromManager(BackgroundBoxAreaEffect effect)
	{
	}

	public void FadeOutAndUnregister(BackgroundBoxAreaEffect effect, float startIntensity, float delay, float duration, Ease ease)
	{
	}

	public static bool IsInsideCoverWorldUI(Vector3 worldPos)
	{
		return false;
	}

	private void LateUpdate()
	{
	}

	private void UpdateGlobalProperties()
	{
	}
}
