using System;
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

	private Vector3 _prevWorldDirection;

	private Vector3 _currentWorldDirection;

	private Vector3 _inputWorldDirection;

	private const float _defaultRange = 9f;

	private const float _defaultBackRange = 3f;

	private const float _defaultViewAngle = 120f;

	private float _sightAlpha;

	private float _sightAngle;

	private float _sightRange;

	private float _sightBackRange;

	private float _prevSightToggle;

	private float _currentSightToggle;

	private float _prevBlindness;

	private float _currentBlindness;

	private float _prevSharingSight;

	private float _currentSharingSight;

	private float _currentLightFuel;

	[Header("Plane Sight Unit Info")]
	[SerializeField]
	private SightInfo _sightInfo;

	private IDisposable _instantiatedSubscription;

	public UnitModel UnitModel => null;

	public Vector3 PivotOffset => default;

	public bool IsUsingTransform => false;

	public Vector3 EasedWorldDirection => default;

	public Vector3 WorldDirection => default;

	public float SightAlpha => 0f;

	public float SightAngle => 0f;

	public float SightRange => 0f;

	public float SightBackRange => 0f;

	public float SightToggleFactor => 0f;

	public float CurrentLightFuel => 0f;

	public SightInfo SightInfo => null;

	private PlaneSightCore PSCore => null;

	private void OnPlaneSightInstantiated(PlaneSightCore fow)
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
	}

	public void SetOptions(float range, float backRange, float viewAngle)
	{
	}

	private void RegisterToManager()
	{
	}

	private void UnRegisterFromManager()
	{
	}

	void IPlaneSightDetector.RegisterToManagerAsDetector()
	{
	}

	void IPlaneSightDetector.UnRegisterFromManagerAsDetector()
	{
	}

	public bool GetIsThisDetected(GameObject detectableObject, bool ignoreToggle = false)
	{
		return false;
	}

	public Transform GetTransform()
	{
		return null;
	}

	private void UpdateEasedDirection()
	{
	}

	private void UpdateSight()
	{
	}

	private void UpdateBlindedIntensity()
	{
	}

	private void UpdateSharingSight()
	{
	}

	private void UpdateSightValues()
	{
	}

	public void SetDirection(in Vector3 targetDirection)
	{
	}

	public void SetOnOffToggle(in bool targetOnOff)
	{
	}

	public void SetOnOffToggleRaw(in bool targetOnOff)
	{
	}

	public void SetIsBlinded(in bool targetIsBlinded)
	{
	}

	public void SetIsMine(in bool targetIsMine)
	{
	}

	public void SetIsShared(in bool targetIsShared)
	{
	}

	public void SetLightFuel(in int targetLightFuel)
	{
	}
}
