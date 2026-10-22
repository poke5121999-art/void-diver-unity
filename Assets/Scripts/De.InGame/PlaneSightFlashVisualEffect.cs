using System;
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
	private float _forwardOffset;

	[SerializeField]
	private float _height;

	[SerializeField]
	private float _maxRange;

	[SerializeField]
	private float _spotLightInnerRatio;

	[SerializeField]
	private float _pointLightRangeRatio;

	[SerializeField]
	private float _pointLightIntensityRatio;

	[SerializeField]
	private Vector3 _pointLightWorldOffset;

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

	private float _lightFuelThresholdInverse;

	private float _blinkTimer;

	private float _nextBlinkTime;

	private float _blinkDuration;

	private bool _isBlinking;

	private bool _isRevealing;

	private IDisposable _instantiatedSubscription;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
	}

	private void BindToCore()
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateFuelBlink()
	{
	}

	private void StartBlink()
	{
	}

	private void EndBlinkAndScheduleNext(float fuelRatio)
	{
	}

	private float GetLightIntensityUsingBlink(float minIntensity = 0f)
	{
		return 0f;
	}

	private void UpdateTransform()
	{
	}

	private void UpdateRenderer()
	{
	}

	private void UpdateUnityLights()
	{
	}

	private bool IsValid()
	{
		return false;
	}
}
