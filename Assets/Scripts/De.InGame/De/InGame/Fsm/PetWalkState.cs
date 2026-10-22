namespace De.InGame.Fsm
{
	public class PetWalkState : PetStateBase
	{
		public override EPetState PetState => EPetState.Idle;

		public PetWalkState(PetStateMachine stateMachine)
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
