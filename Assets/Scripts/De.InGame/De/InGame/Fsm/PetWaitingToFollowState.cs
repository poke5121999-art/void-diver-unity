namespace De.InGame.Fsm
{
	public class PetWaitingToFollowState : PetStateBase
	{
		private const float _followDelaySeconds = 0.5f;

		private float _timer;

		public override EPetState PetState => EPetState.Idle;

		public PetWaitingToFollowState(PetStateMachine stateMachine)
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
