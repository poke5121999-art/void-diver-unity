using UnityEngine;

namespace De.InGame.Fsm
{
	public class PetAlertState : PetStateBase
	{
		private const float _duration = 0.66f;

		private float _timer;

		public override EPetState PetState => EPetState.Alert;

		public PetAlertState(PetStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
			PetModel.Speed = -1f;
			_timer = 0f;
		}

		public override void OnUpdate()
		{
			_timer += Time.fixedDeltaTime;
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			if (stateEvent is PetTeleportEvent)
			{
				return EPetState.Teleport;
			}
			if (!(_timer < 0.66f))
			{
				return EPetState.Idle;
			}
			return EPetState.None;
		}
	}
}
