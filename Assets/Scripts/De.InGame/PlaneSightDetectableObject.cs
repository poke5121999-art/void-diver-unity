using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class PlaneSightDetectableObject : MonoBehaviour
{
	[Header("In-Game Debug")]
	[SerializeField]
	private bool _isDetected;

	private bool _prevDetected;

	[SerializeField]
	private ReactiveProperty<int> _rxDetectedCount = new ReactiveProperty<int>();

	[Header("Detection Event Setting")]
	[SerializeField]
	private UnityEvent _detectedEvent;

	[SerializeField]
	private UnityEvent _undectedEvent;

	private readonly ReactiveProperty<bool> _rxIsDetectedByMine = new ReactiveProperty<bool>(false);

	[SerializeField]
	private List<PlaneSightUnit> _detectingPlaneSightUnits = new List<PlaneSightUnit>();

	private readonly Subject<Unit> _onDetectingUnitsChanged = new Subject<Unit>();

	private UnitModel _unitModel;

	private IEnumerator _checkerCoroutine;

	private const float _checkDelay = 0.125f;

	public PlaneSightCore PSCore => PlaneSightCore.Instance;

	public bool IsDetected => _isDetected;

	public IReadOnlyReactiveProperty<int> RxDetectedCount => _rxDetectedCount;

	public IReadOnlyReactiveProperty<bool> RxIsDetectedByMine => _rxIsDetectedByMine;

	public List<PlaneSightUnit> DetectingPlaneSightUnits => _detectingPlaneSightUnits;

	public IObservable<Unit> OnDetectingUnitsChanged => _onDetectingUnitsChanged;

	private void OnEnable()
	{
		if (_checkerCoroutine != null)
		{
			StopCoroutine(_checkerCoroutine);
		}
		_checkerCoroutine = CheckIsUpdateFrame();
		StartCoroutine(_checkerCoroutine);
	}

	private void Start()
	{
		_unitModel = GetComponent<UnitModel>();
		_undectedEvent.Invoke();
		_isDetected = false;
		_prevDetected = false;
	}

	private void OnDisable()
	{
		if (_checkerCoroutine != null)
		{
			StopCoroutine(_checkerCoroutine);
		}
	}

	private void OnDestroy()
	{
		if (_checkerCoroutine != null)
		{
			StopCoroutine(_checkerCoroutine);
		}
	}

	private IEnumerator CheckIsUpdateFrame()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(0.125f);
		while (true)
		{
			UpdateIsDetected();
			yield return waitForSeconds;
		}
	}

	public void UpdateIsDetected()
	{
		if (PSCore == null)
		{
			_rxDetectedCount.Value = 0;
			_isDetected = false;
			_prevDetected = false;
			return;
		}
		int num = 0;
		bool flag = false;
		foreach (IPlaneSightDetector planeSightDetector in PSCore.PlaneSightDetectors)
		{
			bool flag2 = planeSightDetector.GetIsThisDetected(base.gameObject);
			if (flag2)
			{
				num++;
			}
			PlaneSightUnit component = planeSightDetector.GetTransform().GetComponent<PlaneSightUnit>();
			if (!component)
			{
				continue;
			}
			bool flag3 = _detectingPlaneSightUnits.Contains(component);
			if (flag2 && !flag3)
			{
				_detectingPlaneSightUnits.Add(component);
				_onDetectingUnitsChanged.OnNext(Unit.Default);
			}
			else if (flag3 && !flag2)
			{
				_detectingPlaneSightUnits.Remove(component);
				_onDetectingUnitsChanged.OnNext(Unit.Default);
			}
			if (component.SightInfo != null && component.SightInfo.IsMine)
			{
				flag |= component.GetIsThisDetected(base.gameObject, true);
			}
		}
		_rxIsDetectedByMine.Value = flag;
		_rxDetectedCount.Value = num;
		_isDetected = num > 0;
		if (_prevDetected != _isDetected)
		{
			(_isDetected ? _detectedEvent : _undectedEvent).Invoke();
			_prevDetected = _isDetected;
		}
	}

	public bool IsDetectedByMineSight()
	{
		return _detectingPlaneSightUnits.Any((PlaneSightUnit unit) => unit.SightInfo.IsMine);
	}
}
