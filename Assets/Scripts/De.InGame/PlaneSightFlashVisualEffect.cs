using System;
using UniRx;
using UnityEngine;

public class PlaneSightFlashVisualEffect : MonoBehaviour
{
	[Header("Setting: Plane Sight Unit")]
	[SerializeField]
	private PlaneSightUnit _planeSightUnit;

	[Header("Setting: Light")]
	[SerializeField]
	private Light _spotLight;

	[SerializeField]
	private Light _pointLight;

	[Header("Setting: Factors")]
	[SerializeField]
	private float _forwardOffset = 1f;

	[SerializeField]
	private float _height = 1f;

	[SerializeField]
	private float _maxRange = 8f;

	[SerializeField]
	private float _spotLightInnerRatio = 0.75f;

	[SerializeField]
	private float _pointLightRangeRatio = 2f;

	[SerializeField]
	private float _pointLightIntensityRatio = 0.5f;

	[SerializeField]
	private Vector3 _pointLightWorldOffset = new Vector3(-0.15f, 0f, 0.15f);

	[Header("Setting: Shaper")]
	[SerializeField]
	private Renderer _renderer;

	[SerializeField]
	private Transform _rootBone;

	[SerializeField]
	private Transform _offsetBone;

	[SerializeField]
	private Transform _pivotBone;

	[SerializeField]
	private Transform _headBone;

	private PlaneSightCore _planeSightCore;

	private Transform _cachedTransform;

	private PlaneSightDesignScriptableData _planeSightDesignScriptableData;

	private float _lightFuelThresholdInverse = 1f;

	private float _blinkTimer;

	private float _nextBlinkTime;

	private float _blinkDuration = 0.1f;

	private bool _isBlinking;

	private bool _isRevealing;

	private IDisposable _instantiatedSubscription;

	private void Awake()
	{
		_cachedTransform = base.transform;
	}

	private void OnEnable()
	{
		PlaneSightCore.OnRevealingMapChanged += OnRevealingMapChanged;
		BindToCore();
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = PlaneSightCore.OnInstantiated.Subscribe(delegate
		{
			BindToCore();
		});
	}

	private void OnDisable()
	{
		PlaneSightCore.OnRevealingMapChanged -= OnRevealingMapChanged;
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
		_isRevealing = isRevealing;
	}

	private void BindToCore()
	{
		_planeSightCore = PlaneSightCore.Instance;
		if (_planeSightCore == null)
		{
			_planeSightDesignScriptableData = null;
			return;
		}
		_isRevealing = _planeSightCore.IsRevealingMap;
		_planeSightDesignScriptableData = _planeSightCore.DesignScriptableData;
		if (_planeSightDesignScriptableData != null)
		{
			_lightFuelThresholdInverse = 1f / (float)_planeSightDesignScriptableData.FuelBlinkStartThreshold;
		}
	}

	private void LateUpdate()
	{
		if (!IsValid())
		{
			return;
		}
		if (_isRevealing)
		{
			_renderer.enabled = false;
			_spotLight.enabled = false;
			_pointLight.enabled = false;
			return;
		}
		_renderer.enabled = true;
		_spotLight.enabled = true;
		_pointLight.enabled = true;
		UpdateFuelBlink();
		UpdateTransform();
		UpdateRenderer();
		UpdateUnityLights();
	}

	private void UpdateFuelBlink()
	{
		if (_planeSightUnit == null || _planeSightDesignScriptableData == null)
		{
			_isBlinking = false;
			return;
		}
		float currentLightFuel = _planeSightUnit.CurrentLightFuel;
		float lightFuelThresholdInverse = _lightFuelThresholdInverse;
		if (!_isBlinking)
		{
			_nextBlinkTime -= Time.deltaTime;
			if (!(_nextBlinkTime > 0f))
			{
				StartBlink();
			}
		}
		else
		{
			_blinkTimer += Time.deltaTime;
			if (!(_blinkTimer < _blinkDuration))
			{
				EndBlinkAndScheduleNext(lightFuelThresholdInverse * currentLightFuel);
			}
		}
	}

	private void StartBlink()
	{
		_isBlinking = true;
		_blinkTimer = 0f;
		_blinkDuration = UnityEngine.Random.Range(_planeSightDesignScriptableData.FuelBlinkDurationMin, _planeSightDesignScriptableData.FuelBlinkDurationMax);
	}

	private void EndBlinkAndScheduleNext(float fuelRatio)
	{
		_isBlinking = false;
		float fuelBlinkMaxDelayAtZero = _planeSightDesignScriptableData.FuelBlinkMaxDelayAtZero;
		fuelRatio = Mathf.Clamp01(fuelRatio);
		_nextBlinkTime = UnityEngine.Random.Range(0f, (_planeSightDesignScriptableData.FuelBlinkMaxDelayAtThreshold - fuelBlinkMaxDelayAtZero) * fuelRatio + fuelBlinkMaxDelayAtZero);
	}

	private float GetLightIntensityUsingBlink(float minIntensity = 0f)
	{
		if (!(_planeSightDesignScriptableData == null) && _planeSightDesignScriptableData.FuelBlinkEnable && _isBlinking)
		{
			return minIntensity;
		}
		return 1f;
	}

	private void UpdateTransform()
	{
		_cachedTransform.localPosition = Vector3.up * _height;
		Vector3 easedWorldDirection = _planeSightUnit.EasedWorldDirection;
		Vector3 vector = new Vector3(easedWorldDirection.x, 0f, easedWorldDirection.z);
		_cachedTransform.forward = (vector.sqrMagnitude >= 0.01f) ? vector.normalized : Vector3.forward;
		_pivotBone.localScale = Vector3.one * _planeSightUnit.SightToggleFactor * GetLightIntensityUsingBlink();
		_offsetBone.localPosition = Vector3.forward * _forwardOffset;
	}

	private void UpdateRenderer()
	{
		float num = (_planeSightUnit.SightAngle * 0.5f) * ((float)Math.PI / 180f);
		float y = Mathf.Max(0f, Mathf.Cos(num) * _planeSightUnit.SightRange);
		float num2 = Mathf.Min(_maxRange, Mathf.Tan(num) * _planeSightUnit.SightRange * _planeSightUnit.SightToggleFactor);
		float lightIntensityUsingBlink = GetLightIntensityUsingBlink();
		_headBone.localScale = new Vector3(num2 * lightIntensityUsingBlink, y, 1f);
	}

	private void UpdateUnityLights()
	{
		float num = _planeSightUnit.SightToggleFactor * _planeSightUnit.SightAlpha;
		float lightIntensityUsingBlink = GetLightIntensityUsingBlink(0.25f);
		_spotLight.intensity = lightIntensityUsingBlink * num;
		_spotLight.range = _planeSightUnit.SightToggleFactor * _planeSightUnit.SightRange;
		_spotLight.spotAngle = _planeSightUnit.SightToggleFactor * _planeSightUnit.SightAngle;
		_spotLight.innerSpotAngle = _planeSightUnit.SightToggleFactor * _planeSightUnit.SightAngle * _spotLightInnerRatio;
		_pointLight.range = _planeSightUnit.SightBackRange * _pointLightRangeRatio;
		_pointLight.intensity = num * _pointLightIntensityRatio;
		_pointLight.gameObject.transform.position = _cachedTransform.position + _pointLightWorldOffset;
	}

	private bool IsValid()
	{
		if (_planeSightUnit == null || _renderer == null || _spotLight == null || _pointLight == null || _pivotBone == null || _offsetBone == null || _headBone == null)
		{
			return false;
		}
		return true;
	}
}
