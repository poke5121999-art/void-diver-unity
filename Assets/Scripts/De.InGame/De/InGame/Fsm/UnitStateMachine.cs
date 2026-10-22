using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.Log;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;

namespace De.InGame.Fsm
{
	public abstract class UnitStateMachine : NativeBase
	{
		[Inject]
		protected GameManager GameManager;

		[Inject]
		protected GameObjectManager ObjectManager;

		[Inject]
		protected SeriLogger Logger;

		private readonly Dictionary<EActionState, UnitStateBase> _states;

		private UnitStateBase _currentState;

		private EActionState _initialState;

		private bool _isTransitioning;

		private readonly Queue<IUnitEvent> _pendingEvents;

		public bool IsAimEnable => _currentState != null && _currentState.IsAimEnable;

		public EActionState PreviousState { get; private set; }

		public UnitModel UnitModel { get; }

		public UnitView UnitView { get; }

		public UnitController UnitController { get; }

		protected UnitStateMachine(UnitController controller)
		{
			_states = new Dictionary<EActionState, UnitStateBase>();
			_pendingEvents = new Queue<IUnitEvent>();
			UnitController = controller;
			UnitModel = controller.GetComponent<UnitModel>();
			UnitView = controller.GetComponent<UnitView>();
		}

		protected void AddState(UnitStateBase unitState)
		{
			_states.Add(unitState.ActionState, unitState);
		}

		public virtual void TriggerEvent(IUnitEvent stateEvent)
		{
			if (_currentState == null)
			{
				return;
			}
			if (!_isTransitioning)
			{
				EActionState next = _currentState.GetNextStateEvent(stateEvent);
				if (next != EActionState.None)
				{
					ChangeState(next, stateEvent);
				}
			}
			else
			{
				_pendingEvents.Enqueue(stateEvent);
			}
		}

		protected virtual void ChangeState(EActionState actionState, IUnitEvent unitEvent)
		{
			if (!GameManager.IsHost)
			{
				Logger.Warning("IsHost is false");
				return;
			}
			if (_isTransitioning)
			{
				_pendingEvents.Enqueue(unitEvent);
				return;
			}
			_isTransitioning = true;
			if (_currentState != null)
			{
				_currentState.OnExit();
			}
			PreviousState = UnitModel.State;
			_currentState = _states[actionState];
			_currentState.OnEnter(unitEvent);
			UnitModel.State = actionState;
			_isTransitioning = false;
		}

		public void SetInitialState(EActionState defaultState)
		{
			if (GameManager.IsHost)
			{
				_initialState = defaultState;
			}
			else
			{
				Logger.Warning("IsHost is false");
			}
		}

		public virtual void OnUpdate()
		{
			if (_states.Count == 0)
			{
				return;
			}
			if (_currentState == null)
			{
				if (_initialState == EActionState.None)
				{
					ChangeState(EActionState.Idle, null);
				}
				else
				{
					ChangeState(_initialState, null);
					_initialState = EActionState.None;
				}
			}
			DrainPendingEvents();
			TriggerEvent(null);
			if (_currentState != null)
			{
				_currentState.OnUpdate();
			}
		}

		private void DrainPendingEvents()
		{
			int count = _pendingEvents.Count;
			for (int i = 0; i < count; i++)
			{
				TriggerEvent(_pendingEvents.Dequeue());
			}
		}

		public Vector3 GetForward()
		{
			return (_currentState != null) ? _currentState.GetForward() : Vector3.zero;
		}

		public void Clear()
		{
			PreviousState = EActionState.None;
			if (_currentState != null)
			{
				_currentState.OnExit();
				_currentState = null;
			}
			_pendingEvents.Clear();
			_isTransitioning = false;
			_states.Values.ForEach(delegate(UnitStateBase s)
			{
				s.Clear();
			});
			_states.Clear();
		}
	}
}
