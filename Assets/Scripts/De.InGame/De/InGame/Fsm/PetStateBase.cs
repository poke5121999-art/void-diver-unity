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

		protected PetController PetController => StateMachine.PetController;

		protected PetModel PetModel => StateMachine.PetController.PetModel;

		protected PetView PetView => StateMachine.PetController.PetView;

		protected EPetState PreviousState => StateMachine.PreviousState;

		protected bool IsInCombat => PetController.IsInCombat;

		protected float DistanceToOwner => PetController.DistanceToOwner;

		protected Vector3 OwnerPosition => PetController.OwnerPosition;

		protected float WalkSpeed => PetController.WalkSpeed;

		protected float RunSpeed => PetController.RunSpeed;

		protected float WalkStartDistance => PetController.WalkStartDistance;

		protected float RunStartDistance => PetController.RunStartDistance;

		protected float DesiredDistance => PetController.DesiredDistance;

		protected float BackOffDistance => PetController.BackOffDistance;

		public abstract EPetState PetState { get; }

		protected PetStateBase(PetStateMachine stateMachine)
		{
			StateMachine = stateMachine;
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
