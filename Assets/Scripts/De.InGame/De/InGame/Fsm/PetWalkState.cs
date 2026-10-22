namespace De.InGame.Fsm
{
	public class PetWalkState : PetStateBase
	{
		public override EPetState PetState => EPetState.Walk;

		public PetWalkState(PetStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
			PetModel.Speed = WalkSpeed;
		}

		public override void OnUpdate()
		{
			PetController.MoveToTarget(OwnerPosition, WalkSpeed);
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
			if (DistanceToOwner <= DesiredDistance)
			{
				return EPetState.Idle;
			}
			if (!(DistanceToOwner > RunStartDistance))
			{
				return EPetState.None;
			}
			return EPetState.Run;
		}
	}
}
