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

		private readonly Dictionary<EPetState, PetStateBase> _states;

		private PetStateBase _currentState;

		public PetController PetController { get; }

		public PetModel PetModel => null;

		public PetView PetView => null;

		public EPetState PreviousState { get; private set; }

		public PetStateMachine(PetController controller)
		{
		}

		private void AddState(PetStateBase state)
		{
		}

		public virtual void TriggerEvent(IPetEvent stateEvent)
		{
		}

		protected virtual void ChangeState(EPetState newState, IPetEvent stateEvent)
		{
		}

		public void OnUpdate()
		{
		}
	}
}
