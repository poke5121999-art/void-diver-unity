using UnityEngine;

namespace De.InGame.Fsm
{
	public class PetWaitingToFollowState : PetStateBase
	{
		private const float _followDelaySeconds = 0.5f;

		private float _timer;

		public override EPetState PetState => EPetState.WaitingToFollow;

		public PetWaitingToFollowState(PetStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
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
			if (stateEvent is PetCombatEnterEvent)
			{
				return EPetState.Alert;
			}
			if (DistanceToOwner <= DesiredDistance)
			{
				return EPetState.Idle;
			}
			if (_timer < 0.5f)
			{
				return EPetState.None;
			}
			if (!(DistanceToOwner > RunStartDistance))
			{
				return EPetState.Walk;
			}
			return EPetState.Run;
		}
	}
}
