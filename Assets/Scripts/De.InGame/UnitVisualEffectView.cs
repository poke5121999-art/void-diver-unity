using DG.Tweening;
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

	private Vector3 _startLocalPosition;

	private Quaternion _startLocalRotation;

	private Vector3 _startLocalScale;

	private EStatusEffectTag _currentEffectTag;

	[SerializeField]
	protected SkeletonMaterialOverrider SkeletonMaterialOverrider;

	private readonly int _innerColorProperty;

	private readonly int _outerColorProperty;

	private readonly int _intensityProperty;

	protected readonly int IsEnemyProperty;

	private readonly int _propertyHitFlashIntensity;

	private readonly int _propertyHitFlashColor;

	private Vector3 _transformRight;

	[Header("Hit Visual Effect Settings")]
	[SerializeField]
	private EUnitHitVisualEffectType _hitVisualEffectType;

	[SerializeField]
	private EUnitHitVisualEffectColor _hitVisualEffectColor;

	private Ease _hitFlashEaseType;

	private float _hitFlashDuration;

	private Tween _hitFlashTween;

	private Tween _hitShakeTween;

	private Tween _hitShakeScaleTween;

	private readonly int _deathProgressProperty;

	private const float _dissolveDelay = 1f;

	private const float _dissolveDuration = 2f;

	private const float _reviveDelay = 0f;

	private const float _reviveDuration = 1f;

	private Tween _stateEffectTween;

	private Tween _stateShadowEffectTween;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	protected virtual void Initialize()
	{
	}

	private void InitializeTransform()
	{
	}

	private void ResetTransform()
	{
	}

	private void OnDamage()
	{
	}

	private void OnReservedDespawn()
	{
	}

	public void OnRevive()
	{
	}

	private bool HasEffect(EStatusEffectTag statusEffectTag)
	{
		return false;
	}

	protected virtual void OnStatusEffectTagAdded(EStatusEffectTag statusEffectTag)
	{
	}

	protected virtual void OnStatusEffectTagRemoved(EStatusEffectTag statusEffectTag)
	{
	}

	private void SetStatusEffect(EStatusEffectTag statusEffectTag)
	{
	}

	private void GetHitShakeFactors(in EUnitHitVisualEffectType unitSkeletonHitEffectType, out Ease hitShakeEaseType, out int hitShakeVibrato, out float hitShakeStrength, out float hitShakeDuration, out Ease hitShakeScaleEaseType, out int hitShakeScaleVibrato, out float hitShakeScaleStrength, out float hitShakeScaleDuration)
	{
		hitShakeEaseType = default;
		hitShakeVibrato = default;
		hitShakeStrength = default;
		hitShakeDuration = default;
		hitShakeScaleEaseType = default;
		hitShakeScaleVibrato = default;
		hitShakeScaleStrength = default;
		hitShakeScaleDuration = default;
	}

	private Color GetHitVisualEffectColor(in EUnitHitVisualEffectColor unitHitColorType)
	{
		return default;
	}

	private void ClearHitEffect()
	{
	}

	private void PlayHitEffect()
	{
	}

	private void ClearStateEffect()
	{
	}

	private void PlayDissolveEffect()
	{
	}

	private void PlayReviveEffect()
	{
	}
}
