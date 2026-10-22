using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterIdleState : MonsterStateBase
	{
		private float _stateStartTime;

		private float _wanderStartDelay;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		private float StateElapsedTime => 0f;

		public MonsterIdleState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return default;
		}
	}
}
