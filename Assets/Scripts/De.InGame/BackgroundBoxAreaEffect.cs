using System;
using DG.Tweening;
using UnityEngine;

[ExecuteAlways]
public class BackgroundBoxAreaEffect : MonoBehaviour
{
	[Header("Box Setting")]
	[SerializeField]
	private bool _useTransform;

	[SerializeField]
	private Vector3 _center;

	[SerializeField]
	private Vector3 _extents;

	[Header("Effect Setting")]
	[SerializeField]
	private EBackgroundBoxAreaEffectType _effectType;

	[SerializeField]
	private Color _overlayColor;

	[SerializeField]
	private float _targetIntensity;

	[SerializeField]
	private float _edgeSmooth;

	[Header("Shimmer Setting")]
	[SerializeField]
	private float _shimmerSpeed;

	[SerializeField]
	private float _shimmerStrength;

	[Header("Fog Setting")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _fogDensity;

	[Header("Character Cover")]
	[SerializeField]
	private bool _coverCharacter;

	[Header("World UI Cover")]
	[SerializeField]
	private bool _coverWorldUI;

	[Header("Outline Cover")]
	[SerializeField]
	private bool _coverOutline;

	[Header("Fade Out")]
	[SerializeField]
	private float _fadeOutDelay;

	[SerializeField]
	private float _fadeOutDuration;

	[SerializeField]
	private Ease _fadeOutEase;

	private float _intensity;

	private Tween _fadeTween;

	private IDisposable _instantiatedSubscription;

	public float Intensity => 0f;

	public Vector3 Center => default;

	public Vector3 Extents => default;

	public Color OverlayColor => default;

	public EBackgroundBoxAreaEffectType EffectType => EBackgroundBoxAreaEffectType.Darken;

	public float EdgeSmooth => 0f;

	public float ShimmerSpeed => 0f;

	public float ShimmerStrength => 0f;

	public float FogDensity => 0f;

	public bool CoverCharacter => false;

	public bool CoverWorldUI => false;

	public bool CoverOutline => false;

	private void OnEnable()
	{
	}

	private void RegisterSelf()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void UnregisterSelf()
	{
	}

	public Tween FadeIn(float duration, Ease ease = Ease.Linear)
	{
		return null;
	}

	public Tween FadeOut(float duration, Ease ease = Ease.Linear, bool disableOnComplete = false)
	{
		return null;
	}

	public void SetBox(Vector3 center, Vector3 extents)
	{
	}

	public void ApplyOverlay(EBackgroundBoxAreaEffectType effectType, Color overlayColor, float edgeSmooth, float shimmerSpeed, float shimmerStrength, float fogDensity, float targetIntensity)
	{
	}

	public void SetImmediate(float intensity)
	{
	}
}
