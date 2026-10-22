namespace De.InGame.Fsm
{
	public class PetIdleState : PetStateBase
	{
		public override EPetState PetState => EPetState.Idle;

		public PetIdleState(PetStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
			PetModel.Speed = -1f;
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			if (stateEvent is PetTeleportEvent)
			{
				return EPetState.Teleport;
			}
			if (stateEvent is PetCombatEnterEvent)
			{
				return EPetState.Alert;
			}
			if (DistanceToOwner > WalkStartDistance)
			{
				return EPetState.WaitingToFollow;
			}
			if (DistanceToOwner < BackOffDistance)
			{
				return EPetState.BackOff;
			}
			return EPetState.None;
		}
	}
}
