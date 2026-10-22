using NemoLib.Log;
using UnityEngine;
using VContainer;

namespace De.InGame.Fsm
{
	public abstract class PetStateBase : NativeBase
	{
		[Inject]
		protected GameManager GameManager;

		[Inject]
		protected SeriLogger Logger;

		protected PetStateMachine StateMachine { get; }

		protected PetController PetController => null;

		protected PetModel PetModel => null;

		protected PetView PetView => null;

		protected EPetState PreviousState => EPetState.Idle;

		protected bool IsInCombat => false;

		protected float DistanceToOwner => 0f;

		protected Vector3 OwnerPosition => default;

		protected float WalkSpeed => 0f;

		protected float RunSpeed => 0f;

		protected float WalkStartDistance => 0f;

		protected float RunStartDistance => 0f;

		protected float DesiredDistance => 0f;

		protected float BackOffDistance => 0f;

		public abstract EPetState PetState { get; }

		protected PetStateBase(PetStateMachine stateMachine)
		{
		}

		public virtual void OnEnter(IPetEvent stateEvent)
		{
		}

		public virtual void OnExit()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public abstract EPetState GetNextStateEvent(IPetEvent stateEvent);
	}
}
