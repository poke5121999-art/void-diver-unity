using System.Collections.Generic;
using NemoLib.Log;
using VContainer;

namespace De.InGame.Fsm
{
	public class PetStateMachine : NativeBase
	{
		[Inject]
		protected GameManager GameManager;

		[Inject]
		protected SeriLogger Logger;

		private readonly Dictionary<EPetState, PetStateBase> _states = new Dictionary<EPetState, PetStateBase>();

		private PetStateBase _currentState;

		public PetController PetController { get; }

		public PetModel PetModel => PetController.PetModel;

		public PetView PetView => PetController.PetView;

		public EPetState PreviousState { get; private set; }

		public PetStateMachine(PetController controller)
		{
			PetController = controller;
			AddState(new PetIdleState(this));
			AddState(new PetAlertState(this));
			AddState(new PetWaitingToFollowState(this));
			AddState(new PetWalkState(this));
			AddState(new PetRunState(this));
			AddState(new PetBackOffState(this));
			AddState(new PetTeleportState(this));
		}

		private void AddState(PetStateBase state)
		{
			_states[state.PetState] = state;
		}

		public virtual void TriggerEvent(IPetEvent stateEvent)
		{
			if (_currentState != null)
			{
				EPetState nextStateEvent = _currentState.GetNextStateEvent(stateEvent);
				if (nextStateEvent != EPetState.None)
				{
					ChangeState(nextStateEvent, stateEvent);
				}
			}
		}

		protected virtual void ChangeState(EPetState newState, IPetEvent stateEvent)
		{
			if (!GameManager.IsHost)
			{
				Logger.Warning("[PetStateMachine] Not a host.");
				return;
			}
			_currentState?.OnExit();
			PreviousState = PetController.PetModel.State;
			_currentState = _states[newState];
			_currentState.OnEnter(stateEvent);
			PetController.PetModel.State = newState;
		}

		public void OnUpdate()
		{
			if (GameManager.IsHost)
			{
				if (_currentState == null)
				{
					ChangeState(EPetState.Idle, null);
					return;
				}
				TriggerEvent(null);
				_currentState.OnUpdate();
			}
		}
	}
}
