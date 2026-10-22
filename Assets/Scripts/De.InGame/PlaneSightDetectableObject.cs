using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class PlaneSightDetectableObject : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CCheckIsUpdateFrame_003Ed__27 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlaneSightDetectableObject _003C_003E4__this;

		private WaitForSeconds _003CwaitForSeconds_003E5__2;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CCheckIsUpdateFrame_003Ed__27(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Header("In-Game Debug")]
	[SerializeField]
	private bool _isDetected;

	private bool _prevDetected;

	[SerializeField]
	private ReactiveProperty<int> _rxDetectedCount;

	[Header("Detection Event Setting")]
	[SerializeField]
	private UnityEvent _detectedEvent;

	[SerializeField]
	private UnityEvent _undectedEvent;

	private readonly ReactiveProperty<bool> _rxIsDetectedByMine;

	[SerializeField]
	private List<PlaneSightUnit> _detectingPlaneSightUnits;

	private readonly Subject<Unit> _onDetectingUnitsChanged;

	private UnitModel _unitModel;

	private IEnumerator _checkerCoroutine;

	private const float _checkDelay = 0.125f;

	public PlaneSightCore PSCore => null;

	public bool IsDetected => false;

	public IReadOnlyReactiveProperty<int> RxDetectedCount => null;

	public IReadOnlyReactiveProperty<bool> RxIsDetectedByMine => null;

	public List<PlaneSightUnit> DetectingPlaneSightUnits => null;

	public IObservable<Unit> OnDetectingUnitsChanged => null;

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	[IteratorStateMachine(typeof(_003CCheckIsUpdateFrame_003Ed__27))]
	private IEnumerator CheckIsUpdateFrame()
	{
		return null;
	}

	public void UpdateIsDetected()
	{
	}

	public bool IsDetectedByMineSight()
	{
		return false;
	}
}
