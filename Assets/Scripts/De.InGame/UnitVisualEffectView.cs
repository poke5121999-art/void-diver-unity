using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using GameCommon.Enum;
using UnityEngine;
using VContainer;

public class UnitVisualEffectView : MonoBase
{
	[SerializeField]
	private UnitModel _unitModel;

	[SerializeField]
	private UnitView _unitView;

	[SerializeField]
	private UnitVisualStatusEffectScriptable _visualScriptable;

	[Inject]
	private GameChannel _gameChannel;

	private Vector3 _startLocalPosition = Vector3.zero;

	private Quaternion _startLocalRotation = Quaternion.identity;

	private Vector3 _startLocalScale = Vector3.one;

	private EStatusEffectTag _currentEffectTag;

	[SerializeField]
	protected SkeletonMaterialOverrider SkeletonMaterialOverrider;

	private readonly int _innerColorProperty = Shader.PropertyToID("_MaskColor0");

	private readonly int _outerColorProperty = Shader.PropertyToID("_MaskColor1");

	private readonly int _intensityProperty = Shader.PropertyToID("_MaskIntensity");

	protected readonly int IsEnemyProperty = Shader.PropertyToID("_IsEnemy");

	private readonly int _propertyHitFlashIntensity = Shader.PropertyToID("_Effect");

	private readonly int _propertyHitFlashColor = Shader.PropertyToID("_EffectColor");

	private Vector3 _transformRight = new Vector3(0.71f, 0f, 0.71f);

	[Header("Hit Visual Effect Settings")]
	[SerializeField]
	private EUnitHitVisualEffectType _hitVisualEffectType = EUnitHitVisualEffectType.Jelly;

	[SerializeField]
	private EUnitHitVisualEffectColor _hitVisualEffectColor = EUnitHitVisualEffectColor.WhiteAlpha;

	private Ease _hitFlashEaseType = (Ease)20;

	private float _hitFlashDuration = 0.125f;

	private Tween _hitFlashTween;

	private Tween _hitShakeTween;

	private Tween _hitShakeScaleTween;

	private readonly int _deathProgressProperty = Shader.PropertyToID("_DissolveProgress");

	private const float _dissolveDelay = 1f;

	private const float _dissolveDuration = 2f;

	private const float _reviveDelay = 0f;

	private const float _reviveDuration = 1f;

	private Tween _stateEffectTween;

	private Tween _stateShadowEffectTween;

	protected override void Awake()
	{
		base.Awake();
		if (base.gameObject != null && base.gameObject.transform != null)
		{
			_startLocalPosition = base.gameObject.transform.localPosition;
			if (base.gameObject != null && base.gameObject.transform != null)
			{
				_startLocalRotation = base.gameObject.transform.localRotation;
				if (base.gameObject != null && base.gameObject.transform != null)
				{
					_startLocalScale = base.gameObject.transform.localScale;
				}
			}
		}
	}

	private void Start()
	{
		Initialize();
		_gameChannel.Receive<NtfHpDamage>().Where((NtfHpDamage _) => _ != null && _unitModel != null && _.NetworkObjectId == _unitModel.NetworkObjectId).Subscribe(delegate
		{
			OnDamage();
		})
			.AddTo(this);
		_unitModel.RxState.Where((EActionState _) => _ == EActionState.ReservedDespawn).Subscribe(delegate
		{
			OnReservedDespawn();
		})
			.AddTo(this);
		_unitModel.RxState.Where((EActionState _) => _ == EActionState.Revive).Subscribe(delegate
		{
			OnRevive();
		})
			.AddTo(this);
	}

	private void OnDestroy()
	{
		ClearHitEffect();
		ClearStateEffect();
	}

	protected virtual void Initialize()
	{
		if (SkeletonMaterialOverrider == null)
		{
			SkeletonMaterialOverrider = GetComponentInChildren<SkeletonMaterialOverrider>();
		}
		if (_unitView != null)
		{
			_unitView.OnStatusEffectTagAdded.Subscribe(OnStatusEffectTagAdded).AddTo(this);
			_unitView.OnStatusEffectTagRemoved.Subscribe(OnStatusEffectTagRemoved).AddTo(this);
		}
	}

	private void InitializeTransform()
	{
		if (base.gameObject != null && base.gameObject.transform != null)
		{
			_startLocalPosition = base.gameObject.transform.localPosition;
			if (base.gameObject != null && base.gameObject.transform != null)
			{
				_startLocalRotation = base.gameObject.transform.localRotation;
				if (base.gameObject != null && base.gameObject.transform != null)
				{
					_startLocalScale = base.gameObject.transform.localScale;
				}
			}
		}
	}

	private void ResetTransform()
	{
		if (base.gameObject != null && base.gameObject.transform != null)
		{
			base.gameObject.transform.localPosition = _startLocalPosition;
			if (base.gameObject != null && base.gameObject.transform != null)
			{
				base.gameObject.transform.localRotation = _startLocalRotation;
				if (base.gameObject != null && base.gameObject.transform != null)
				{
					base.gameObject.transform.localScale = _startLocalScale;
				}
			}
		}
	}

	private void OnDamage()
	{
		PlayHitEffect();
	}

	private void OnReservedDespawn()
	{
		PlayDissolveEffect();
	}

	public void OnRevive()
	{
		PlayReviveEffect();
	}

	private bool HasEffect(EStatusEffectTag statusEffectTag)
	{
		return _visualScriptable.GetVisualInfoByKeyword(string.Format("{0}", statusEffectTag)) != null;
	}

	protected virtual void OnStatusEffectTagAdded(EStatusEffectTag statusEffectTag)
	{
		if (_visualScriptable.GetIsValidKeyword(string.Format("{0}", statusEffectTag)))
		{
			SetStatusEffect(statusEffectTag);
		}
	}

	protected virtual void OnStatusEffectTagRemoved(EStatusEffectTag statusEffectTag)
	{
		if (_currentEffectTag == statusEffectTag)
		{
			SetStatusEffect(Enum.GetValues(typeof(EStatusEffectTag)).Cast<EStatusEffectTag>().Where((EStatusEffectTag tag) => tag != EStatusEffectTag.None && (_unitModel.GetStatusEffectTags() & tag) == tag).FirstOrDefault(HasEffect));
		}
	}

	private void SetStatusEffect(EStatusEffectTag statusEffectTag)
	{
		_currentEffectTag = statusEffectTag;
		if (_visualScriptable == null)
		{
			return;
		}
		if (statusEffectTag == EStatusEffectTag.None)
		{
			if (SkeletonMaterialOverrider == null || SkeletonMaterialOverrider.InstnancedMaterials == null)
			{
				return;
			}
			foreach (Material item in SkeletonMaterialOverrider.InstnancedMaterials.Where((Material m) => m != null))
			{
				item.SetFloat(_intensityProperty, 0f);
			}
			return;
		}
		UnitVisualStatusEffectInfo visualData = _visualScriptable.GetVisualInfoByKeyword(string.Format("{0}", statusEffectTag));
		if (SkeletonMaterialOverrider == null || SkeletonMaterialOverrider.InstnancedMaterials == null)
		{
			return;
		}
		foreach (Material item2 in SkeletonMaterialOverrider.InstnancedMaterials.Where((Material m) => m != null))
		{
			if (visualData != null && item2 != null)
			{
				item2.SetColor(_innerColorProperty, visualData.InnerColor);
				item2.SetColor(_outerColorProperty, visualData.OuterColor);
				item2.SetFloat(_intensityProperty, visualData.Intensity);
			}
		}
	}

	private void GetHitShakeFactors(in EUnitHitVisualEffectType unitSkeletonHitEffectType, out Ease hitShakeEaseType, out int hitShakeVibrato, out float hitShakeStrength, out float hitShakeDuration, out Ease hitShakeScaleEaseType, out int hitShakeScaleVibrato, out float hitShakeScaleStrength, out float hitShakeScaleDuration)
	{
		hitShakeEaseType = Ease.Linear;
		hitShakeVibrato = 0;
		hitShakeStrength = 0f;
		hitShakeDuration = 0f;
		hitShakeScaleEaseType = Ease.Linear;
		hitShakeScaleVibrato = 0;
		hitShakeScaleStrength = 0f;
		hitShakeScaleDuration = 0f;
		if (unitSkeletonHitEffectType == EUnitHitVisualEffectType.Jelly)
		{
			hitShakeEaseType = Ease.OutQuart;
			hitShakeVibrato = 30;
			hitShakeStrength = 0.2f;
			hitShakeDuration = 0.2f;
			hitShakeScaleEaseType = Ease.OutQuart;
			hitShakeScaleVibrato = 30;
			hitShakeScaleStrength = 0.15f;
		}
		else
		{
			if (unitSkeletonHitEffectType != EUnitHitVisualEffectType.Solid)
			{
				return;
			}
			hitShakeEaseType = Ease.OutQuart;
			hitShakeVibrato = 50;
			hitShakeStrength = 0.1f;
			hitShakeDuration = 0.1f;
			hitShakeScaleEaseType = Ease.OutQuart;
			hitShakeScaleVibrato = 20;
			hitShakeScaleStrength = 0.05f;
		}
		hitShakeScaleDuration = 0.3f;
	}

	private Color GetHitVisualEffectColor(in EUnitHitVisualEffectColor unitHitColorType)
	{
		switch (unitHitColorType)
		{
		case EUnitHitVisualEffectColor.WhiteAlpha:
			return new Color(1f, 1f, 1f, 0.4f);
		case EUnitHitVisualEffectColor.Red:
			return new Color(1f, 0f, 0f, 1f);
		case EUnitHitVisualEffectColor.RedAlpha:
			return new Color(1f, 0f, 0f, 0.4f);
		default:
			return new Color(1f, 1f, 1f, 1f);
		}
	}

	private void ClearHitEffect()
	{
		if (_hitFlashTween != null && _hitFlashTween.IsActive())
		{
			_hitFlashTween.Kill();
		}
		if (_hitShakeTween != null && _hitShakeTween.IsActive())
		{
			_hitShakeTween.Kill();
		}
		if (_hitShakeScaleTween != null && _hitShakeScaleTween.IsActive())
		{
			_hitShakeScaleTween.Kill();
		}
		_hitFlashTween = null;
	}

	private void PlayHitEffect()
	{
		ClearHitEffect();
		if (base.gameObject == null || base.gameObject.transform == null)
		{
			return;
		}
		base.gameObject.transform.localPosition = _startLocalPosition;
		if (base.gameObject == null || base.gameObject.transform == null)
		{
			return;
		}
		base.gameObject.transform.localRotation = _startLocalRotation;
		if (base.gameObject == null || base.gameObject.transform == null)
		{
			return;
		}
		base.gameObject.transform.localScale = _startLocalScale;
		float hitFlashDuration = _hitFlashDuration;
		_hitFlashTween = DOVirtual.Float(1f, 0f, hitFlashDuration, delegate(float value)
		{
			if (SkeletonMaterialOverrider != null && SkeletonMaterialOverrider.InstnancedMaterials != null)
			{
				foreach (Material item in SkeletonMaterialOverrider.InstnancedMaterials.Where((Material m) => m != null))
				{
					item.SetFloat(_propertyHitFlashIntensity, value);
				}
			}
		}).SetEase(_hitFlashEaseType).OnStart(delegate
		{
			if (SkeletonMaterialOverrider != null && SkeletonMaterialOverrider.InstnancedMaterials != null)
			{
				foreach (Material item2 in SkeletonMaterialOverrider.InstnancedMaterials)
				{
					if (item2 != null)
					{
						item2.SetColor(_propertyHitFlashColor, GetHitVisualEffectColor(in _hitVisualEffectColor));
						item2.SetFloat(_propertyHitFlashIntensity, 1f);
					}
				}
			}
		}).Play();
	}

	private void ClearStateEffect()
	{
		if (_stateEffectTween != null)
		{
			_stateEffectTween.Kill();
		}
		_stateEffectTween = null;
		if (_stateShadowEffectTween != null)
		{
			_stateShadowEffectTween.Kill();
		}
		_stateShadowEffectTween = null;
		if (SkeletonMaterialOverrider != null && SkeletonMaterialOverrider.InstnancedMaterials != null)
		{
			foreach (Material item in SkeletonMaterialOverrider.InstnancedMaterials.Where((Material m) => m != null))
			{
				item.SetFloat(_deathProgressProperty, 0f);
			}
		}
	}

	private void PlayDissolveEffect()
	{
		ClearStateEffect();
		_stateEffectTween = DOVirtual.Float(0f, 1f, 2f, delegate(float value)
		{
			if (SkeletonMaterialOverrider != null && SkeletonMaterialOverrider.InstnancedMaterials != null)
			{
				foreach (Material item in SkeletonMaterialOverrider.InstnancedMaterials.Where((Material m) => m != null))
				{
					item.SetFloat(_deathProgressProperty, value);
				}
			}
		}).SetDelay(1f).SetEase(Ease.Linear).Play();
	}

	private void PlayReviveEffect()
	{
		ClearStateEffect();
		_stateEffectTween = DOVirtual.Float(1f, 0f, 1f, delegate(float value)
		{
			if (SkeletonMaterialOverrider != null && SkeletonMaterialOverrider.InstnancedMaterials != null)
			{
				foreach (Material item in SkeletonMaterialOverrider.InstnancedMaterials.Where((Material m) => m != null))
				{
					item.SetFloat(_deathProgressProperty, value);
				}
			}
		}).SetDelay(0f).SetEase(Ease.Linear).Play();
	}
}
