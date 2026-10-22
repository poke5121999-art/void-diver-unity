namespace De.InGame.Fsm
{
	public class PetRunState : PetStateBase
	{
		public override EPetState PetState => EPetState.Idle;

		public PetRunState(PetStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
		}

		public override void OnUpdate()
		{
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			return EPetState.Idle;
		}
	}
}
