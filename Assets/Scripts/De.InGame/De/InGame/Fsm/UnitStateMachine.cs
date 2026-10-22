using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.Log;
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

		public bool IsAimEnable => false;

		public EActionState PreviousState { get; private set; }

		public UnitModel UnitModel { get; }

		public UnitView UnitView { get; }

		public UnitController UnitController { get; }

		protected UnitStateMachine(UnitController controller)
		{
		}

		protected void AddState(UnitStateBase unitState)
		{
		}

		public virtual void TriggerEvent(IUnitEvent stateEvent)
		{
		}

		protected virtual void ChangeState(EActionState actionState, IUnitEvent unitEvent)
		{
		}

		public void SetInitialState(EActionState defaultState)
		{
		}

		public virtual void OnUpdate()
		{
		}

		private void DrainPendingEvents()
		{
		}

		public Vector3 GetForward()
		{
			return default;
		}

		public void Clear()
		{
		}
	}
}
