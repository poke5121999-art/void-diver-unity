using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

[ExecuteAlways]
public class BackgroundBoxAreaEffect : MonoBehaviour
{
	[Header("Box Setting")]
	[SerializeField]
	private bool _useTransform = true;

	[SerializeField]
	private Vector3 _center;

	[SerializeField]
	private Vector3 _extents = Vector3.one;

	[Header("Effect Setting")]
	[SerializeField]
	private EBackgroundBoxAreaEffectType _effectType;

	[SerializeField]
	private Color _overlayColor = new Color(0f, 0f, 0f, 0.8f);

	[SerializeField]
	private float _targetIntensity = 1f;

	[SerializeField]
	private float _edgeSmooth = 1f;

	[Header("Shimmer Setting")]
	[SerializeField]
	private float _shimmerSpeed = 1f;

	[SerializeField]
	private float _shimmerStrength = 0.01f;

	[Header("Fog Setting")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _fogDensity = 1f;

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
	private float _fadeOutDuration = 0.3f;

	[SerializeField]
	private Ease _fadeOutEase = Ease.Linear;

	private float _intensity;

	private Tween _fadeTween;

	private IDisposable _instantiatedSubscription;

	public float Intensity => _intensity;

	public Vector3 Center => _useTransform ? base.transform.position : _center;

	public Vector3 Extents => _useTransform ? (base.transform.localScale * 0.5f) : _extents;

	public Color OverlayColor => _overlayColor;

	public EBackgroundBoxAreaEffectType EffectType => _effectType;

	public float EdgeSmooth => _edgeSmooth;

	public float ShimmerSpeed => _shimmerSpeed;

	public float ShimmerStrength => _shimmerStrength;

	public float FogDensity => _fogDensity;

	public bool CoverCharacter => _coverCharacter;

	public bool CoverWorldUI => _coverWorldUI;

	public bool CoverOutline => _coverOutline;

	private void OnEnable()
	{
		_intensity = _targetIntensity;
		RegisterSelf();
		if (_instantiatedSubscription != null)
		{
			_instantiatedSubscription.Dispose();
		}
		_instantiatedSubscription = BackgroundBoxAreaEffectManager.OnInstantiated.Subscribe(delegate
		{
			RegisterSelf();
		});
	}

	private void RegisterSelf()
	{
		if (BackgroundBoxAreaEffectManager.Instance != null)
		{
			BackgroundBoxAreaEffectManager.Instance.RegisterToManager(this);
		}
	}

	private void OnDisable()
	{
		if (_instantiatedSubscription != null)
		{
			_instantiatedSubscription.Dispose();
		}
		_instantiatedSubscription = null;
		if (Application.isPlaying && _fadeOutDuration > 0f && BackgroundBoxAreaEffectManager.Instance != null)
		{
			if (_fadeTween != null)
			{
				_fadeTween.Kill();
			}
			_fadeTween = DOTween.To(() => _intensity, delegate(float v)
			{
				_intensity = v;
			}, 0f, _fadeOutDuration).SetDelay(_fadeOutDelay).SetEase(_fadeOutEase).OnComplete(UnregisterSelf);
		}
		else
		{
			if (_fadeTween != null)
			{
				_fadeTween.Kill();
			}
			UnregisterSelf();
		}
	}

	private void OnDestroy()
	{
		if (_fadeTween != null)
		{
			_fadeTween.Kill();
		}
		if (_instantiatedSubscription != null)
		{
			_instantiatedSubscription.Dispose();
		}
		_instantiatedSubscription = null;
		if (Application.isPlaying && _fadeOutDuration > 0f && BackgroundBoxAreaEffectManager.Instance != null)
		{
			BackgroundBoxAreaEffectManager.Instance.FadeOutAndUnregister(this, _intensity, _fadeOutDelay, _fadeOutDuration, _fadeOutEase);
		}
		else
		{
			UnregisterSelf();
		}
	}

	private void UnregisterSelf()
	{
		if (BackgroundBoxAreaEffectManager.Instance != null)
		{
			BackgroundBoxAreaEffectManager.Instance.UnregisterFromManager(this);
		}
	}

	public Tween FadeIn(float duration, Ease ease = Ease.Linear)
	{
		if (_fadeTween != null)
		{
			_fadeTween.Kill();
		}
		_fadeTween = DOTween.To(() => _intensity, delegate(float v)
		{
			_intensity = v;
		}, _targetIntensity, duration).SetEase(ease);
		return _fadeTween;
	}

	public Tween FadeOut(float duration, Ease ease = Ease.Linear, bool disableOnComplete = false)
	{
		if (_fadeTween != null)
		{
			_fadeTween.Kill();
		}
		_fadeTween = DOTween.To(() => _intensity, delegate(float v)
		{
			_intensity = v;
		}, 0f, duration).SetEase(ease);
		if (disableOnComplete)
		{
			_fadeTween.OnComplete(delegate
			{
				base.gameObject.SetActive(false);
			});
		}
		return _fadeTween;
	}

	public void SetBox(Vector3 center, Vector3 extents)
	{
		_center = center;
		_extents = extents;
		_useTransform = false;
	}

	public void ApplyOverlay(EBackgroundBoxAreaEffectType effectType, Color overlayColor, float edgeSmooth, float shimmerSpeed, float shimmerStrength, float fogDensity, float targetIntensity)
	{
		_effectType = effectType;
		_overlayColor = overlayColor;
		_shimmerSpeed = shimmerSpeed;
		_shimmerStrength = shimmerStrength;
		_edgeSmooth = edgeSmooth;
		_fogDensity = fogDensity;
		_targetIntensity = targetIntensity;
		if (_fadeTween != null)
		{
			_fadeTween.Kill();
		}
		_intensity = targetIntensity;
	}

	public void SetImmediate(float intensity)
	{
		if (_fadeTween != null)
		{
			_fadeTween.Kill();
		}
		_intensity = intensity;
	}
}
