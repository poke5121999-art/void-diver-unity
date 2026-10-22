namespace De.InGame.Fsm
{
	public class PetAlertState : PetStateBase
	{
		private const float _duration = 0.66f;

		private float _timer;

		public override EPetState PetState => EPetState.Idle;

		public PetAlertState(PetStateMachine stateMachine)
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
