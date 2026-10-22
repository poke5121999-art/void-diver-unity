namespace De.InGame.Fsm
{
	public class PetRunState : PetStateBase
	{
		public override EPetState PetState => EPetState.Run;

		public PetRunState(PetStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
			PetModel.Speed = RunSpeed;
		}

		public override void OnUpdate()
		{
			PetController.MoveToTarget(OwnerPosition, RunSpeed);
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
			if (!(DistanceToOwner > DesiredDistance))
			{
				return EPetState.Idle;
			}
			return EPetState.None;
		}
	}
}
