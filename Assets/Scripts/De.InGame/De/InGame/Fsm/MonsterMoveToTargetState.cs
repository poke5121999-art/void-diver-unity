using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterMoveToTargetState : MonsterStateBase
	{
		private Vector3 _currentPosition;

		private float _targetLostInSightTime;

		private float _targetOutsideFollowRangeElapsedTime;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		public MonsterMoveToTargetState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		public override void OnExit()
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
