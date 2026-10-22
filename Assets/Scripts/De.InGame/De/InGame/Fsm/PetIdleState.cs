namespace De.InGame.Fsm
{
	public class PetIdleState : PetStateBase
	{
		public override EPetState PetState => EPetState.Idle;

		public PetIdleState(PetStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			return EPetState.Idle;
		}
	}
}
