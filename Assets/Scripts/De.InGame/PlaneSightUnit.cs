using System;
using UniRx;
using UnityEngine;

public class PlaneSightUnit : MonoBehaviour, IPlaneSightDetector
{
	[Header("Plane Sight Unit Setting")]
	[SerializeField]
	private UnitModel _unitModel;

	[SerializeField]
	private Vector3 _pivotOffset;

	[SerializeField]
	private bool _isUsingTransform;

	private Vector3 _prevWorldDirection = Vector3.forward;

	private Vector3 _currentWorldDirection = Vector3.forward;

	private Vector3 _inputWorldDirection = Vector3.forward;

	private const float _defaultRange = 9f;

	private const float _defaultBackRange = 3f;

	private const float _defaultViewAngle = 120f;

	private float _sightAlpha = 1f;

	private float _sightAngle = 120f;

	private float _sightRange = 9f;

	private float _sightBackRange = 3f;

	private float _prevSightToggle;

	private float _currentSightToggle;

	private float _prevBlindness;

	private float _currentBlindness;

	private float _prevSharingSight;

	private float _currentSharingSight;

	private float _currentLightFuel;

	[Header("Plane Sight Unit Info")]
	[SerializeField]
	private SightInfo _sightInfo = new SightInfo(9f, 3f, 120f);

	private IDisposable _instantiatedSubscription;

	public UnitModel UnitModel => _unitModel;

	public Vector3 PivotOffset => _pivotOffset;

	public bool IsUsingTransform => _isUsingTransform;

	public Vector3 EasedWorldDirection => _currentWorldDirection;

	public Vector3 WorldDirection => _isUsingTransform ? base.gameObject.transform.forward : _inputWorldDirection;

	public float SightAlpha => _sightAlpha;

	public float SightAngle => _sightAngle;

	public float SightRange => _sightRange;

	public float SightBackRange => _sightBackRange;

	public float SightToggleFactor => _currentSightToggle;

	public float CurrentLightFuel => _currentLightFuel;

	public SightInfo SightInfo => _sightInfo;

	private PlaneSightCore PSCore => PlaneSightCore.Instance;

	private void OnPlaneSightInstantiated(PlaneSightCore fow)
	{
		if (base.enabled)
		{
			RegisterToManager();
			((IPlaneSightDetector)this).RegisterToManagerAsDetector();
		}
	}

	private void Start()
	{
		if (_sightInfo != null)
		{
			bool toggleOnOff = _sightInfo.ToggleOnOff;
			_sightInfo.SetOnOffToggle(in toggleOnOff);
			float num = toggleOnOff ? 1f : 0f;
			_prevSightToggle = num;
			_currentSightToggle = num;
		}
	}

	private void OnEnable()
	{
		RegisterToManager();
		((IPlaneSightDetector)this).RegisterToManagerAsDetector();
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = PlaneSightCore.OnInstantiated.Subscribe(OnPlaneSightInstantiated);
		_currentWorldDirection = _inputWorldDirection;
		_prevWorldDirection = _inputWorldDirection;
		if (_sightInfo == null)
		{
			_sightInfo = new SightInfo(9f, 3f, 120f);
		}
		_sightInfo.SetTransform(base.transform);
	}

	private void OnDisable()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		UnRegisterFromManager();
		((IPlaneSightDetector)this).UnRegisterFromManagerAsDetector();
	}

	private void Update()
	{
		UpdateEasedDirection();
		UpdateSight();
		UpdateBlindedIntensity();
		UpdateSharingSight();
		UpdateSightValues();
	}

	public void SetOptions(float range, float backRange, float viewAngle)
	{
		if (_sightInfo == null)
		{
			_sightInfo = new SightInfo();
			_sightInfo.SetTransform(base.transform);
		}
		_sightInfo?.SetSightInfo(in range, in backRange, in viewAngle);
	}

	private void RegisterToManager()
	{
		if (!(PlaneSightCore.Instance == null))
		{
			PlaneSightUnit planeSightUnit = this;
			PlaneSightCore.Instance.RegisterPlaneSightUnit(in planeSightUnit);
		}
	}

	private void UnRegisterFromManager()
	{
		if (!(PlaneSightCore.Instance == null))
		{
			PlaneSightUnit planeSightUnit = this;
			PlaneSightCore.Instance.UnRegisterPlaneSightUnit(in planeSightUnit);
		}
	}

	void IPlaneSightDetector.RegisterToManagerAsDetector()
	{
		if (!(PlaneSightCore.Instance == null))
		{
			PlaneSightCore.Instance.RegisterDetector(this);
		}
	}

	void IPlaneSightDetector.UnRegisterFromManagerAsDetector()
	{
		if (!(PlaneSightCore.Instance == null))
		{
			PlaneSightCore.Instance.UnRegisterDetector(this);
		}
	}

	public bool GetIsThisDetected(GameObject detectableObject, bool ignoreToggle = false)
	{
		if (detectableObject == null)
		{
			return false;
		}
		if (PlaneSightCore.Instance == null)
		{
			return false;
		}
		if (!ignoreToggle && !_sightInfo.ToggleOnOff)
		{
			return false;
		}
		Vector3 position = base.gameObject.transform.position;
		Vector3 position2 = detectableObject.transform.position;
		if (Physics.Linecast(position, position2, out var _, PlaneSightCore.Instance.WallLayerMask))
		{
			return false;
		}
		Vector3 worldDirection = WorldDirection;
		Vector3 vector = detectableObject.transform.position - base.gameObject.transform.position;
		Vector3 normalized = vector.normalized;
		float sqrMagnitude = (detectableObject.transform.position - base.gameObject.transform.position).sqrMagnitude;
		float num = Mathf.Cos(_sightAngle * 0.5f * ((float)Math.PI / 180f));
		if (num < Vector3.Dot(worldDirection, normalized))
		{
			return sqrMagnitude < _sightRange * _sightRange;
		}
		return sqrMagnitude < _sightBackRange * _sightBackRange;
	}

	public Transform GetTransform()
	{
		return base.transform;
	}

	private void UpdateEasedDirection()
	{
		if (Vector3.Dot(WorldDirection, _prevWorldDirection) > 1f - Mathf.Epsilon)
		{
			_currentWorldDirection = WorldDirection;
			_prevWorldDirection = WorldDirection;
			return;
		}
		if ((bool)PSCore && (bool)PSCore.DesignScriptableData)
		{
			float sightDirectionStiffness = PSCore.DesignScriptableData.SightDirectionStiffness;
			if (sightDirectionStiffness >= Mathf.Epsilon)
			{
				float t = Mathf.Clamp01(Time.deltaTime * PSCore.DesignScriptableData.SightDirectionStiffness);
				_currentWorldDirection = Vector3.Lerp(_prevWorldDirection, WorldDirection, t);
				_prevWorldDirection = _currentWorldDirection;
				return;
			}
		}
		_currentWorldDirection = WorldDirection;
		_prevWorldDirection = _currentWorldDirection;
	}

	private void UpdateSight()
	{
		if (_sightInfo == null)
		{
			return;
		}
		float num = _sightInfo.ToggleOnOff ? 1f : 0f;
		if (Mathf.Abs(num - _prevSightToggle) < Mathf.Epsilon)
		{
			_currentSightToggle = num;
			_prevSightToggle = num;
			return;
		}
		if ((bool)PSCore && (bool)PSCore.DesignScriptableData)
		{
			float num2 = (num <= _currentSightToggle) ? PSCore.DesignScriptableData.SightMorphNegativeDelta : PSCore.DesignScriptableData.SightMorphDelta;
			if (num2 >= Mathf.Epsilon)
			{
				num = Mathf.Lerp(_prevSightToggle, num, Time.deltaTime * num2);
			}
		}
		_currentSightToggle = num;
		_prevSightToggle = _currentSightToggle;
	}

	private void UpdateBlindedIntensity()
	{
		if (_sightInfo == null)
		{
			return;
		}
		float num = _sightInfo.IsBlinded ? 1f : 0f;
		if (Mathf.Abs(num - _prevBlindness) < Mathf.Epsilon)
		{
			_currentBlindness = num;
			_prevBlindness = num;
			return;
		}
		if ((bool)PSCore && (bool)PSCore.DesignScriptableData)
		{
			float num2 = (num <= _currentBlindness) ? PSCore.DesignScriptableData.BlindedSightDelta : PSCore.DesignScriptableData.BlindedSightNegativeDelta;
			if (num2 >= Mathf.Epsilon)
			{
				num = Mathf.Lerp(_prevBlindness, num, Time.deltaTime * num2);
			}
		}
		_currentBlindness = num;
		_prevBlindness = _currentBlindness;
	}

	private void UpdateSharingSight()
	{
		if (_sightInfo == null)
		{
			return;
		}
		float num = (_sightInfo.IsMine || _sightInfo.IsShared) ? 1f : 0f;
		if (Mathf.Abs(num - _prevSharingSight) < Mathf.Epsilon)
		{
			_currentSharingSight = num;
			_prevSharingSight = num;
			return;
		}
		if ((bool)PSCore && (bool)PSCore.DesignScriptableData)
		{
			float num2 = (num <= _currentSharingSight) ? PSCore.DesignScriptableData.SharingSightNegativeDelta : PSCore.DesignScriptableData.SharingSightDelta;
			if (num2 >= Mathf.Epsilon)
			{
				num = Mathf.Lerp(_prevSharingSight, num, Time.deltaTime * num2);
			}
		}
		_currentSharingSight = num;
		_prevSharingSight = _currentSharingSight;
	}

	private void UpdateSightValues()
	{
		if (_sightInfo == null)
		{
			return;
		}
		PlaneSightDesignScriptableData designScriptableData = (PlaneSightCore.Instance == null) ? null : PlaneSightCore.Instance.DesignScriptableData;
		if (designScriptableData == null)
		{
			return;
		}
		float viewAngle = _sightInfo.ViewAngle;
		float frontRange = _sightInfo.FrontRange;
		float backRange = _sightInfo.BackRange;
		float t = Mathf.Clamp01(_currentSightToggle);
		float a = Mathf.Lerp(designScriptableData.SightOffAlphaFactor, designScriptableData.SightOnAlphaFactor, t);
		float angle = viewAngle * Mathf.Lerp(designScriptableData.SightOffAngleFactor, designScriptableData.SightOnAngleFactor, t);
		float front = frontRange * Mathf.Lerp(designScriptableData.SightOffFrontRangeFactor, designScriptableData.SightOnFrontRangeFactor, t);
		float back = backRange * Mathf.Lerp(designScriptableData.SightOffBackRangeFactor, designScriptableData.SightOnBackRangeFactor, t);
		float blindness = Mathf.Clamp01(_currentBlindness);
		_sightAngle = Mathf.Lerp(angle, designScriptableData.BlindedSightAngle, blindness);
		_sightAlpha = Mathf.Lerp(a, designScriptableData.BlindedSightAlpha, blindness) * _currentSharingSight;
		_sightRange = Mathf.Lerp(front, designScriptableData.BlindedSightRange, blindness);
		_sightBackRange = Mathf.Lerp(back, designScriptableData.BlindedSightBackRange, blindness);
	}

	public void SetDirection(in Vector3 targetDirection)
	{
		_inputWorldDirection = targetDirection;
	}

	public void SetOnOffToggle(in bool targetOnOff)
	{
		_sightInfo?.SetOnOffToggle(in targetOnOff);
	}

	public void SetOnOffToggleRaw(in bool targetOnOff)
	{
		_sightInfo?.SetOnOffToggle(in targetOnOff);
		float num = targetOnOff ? 1f : 0f;
		_prevSightToggle = num;
		_currentSightToggle = num;
	}

	public void SetIsBlinded(in bool targetIsBlinded)
	{
		_sightInfo?.SetIsBlinded(in targetIsBlinded);
	}

	public void SetIsMine(in bool targetIsMine)
	{
		_sightInfo?.SetIsMine(in targetIsMine);
	}

	public void SetIsShared(in bool targetIsShared)
	{
		_sightInfo?.SetIsShared(in targetIsShared);
	}

	public void SetLightFuel(in int targetLightFuel)
	{
		_currentLightFuel = targetLightFuel;
	}
}
