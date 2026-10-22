using GameCommon.Enum;
using SomeNet.Utility;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterMoveState : MonsterStateBase
	{
		public override EActionState ActionState => EActionState.Move;

		public override bool IsAimEnable => true;

		public MonsterMoveState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (StateMachine.PreviousState != EActionState.Skill)
			{
				return EActionState.Idle;
			}
			float num = RandomUtility.GetRandomFloat() * 100f;
			if (num < MonsterData.PauseStatePercent)
			{
				return EActionState.Pause;
			}
			if (!(num - MonsterData.PauseStatePercent < MonsterData.AvoidStatePercent))
			{
				return EActionState.Idle;
			}
			return EActionState.Avoid;
		}

		public override Vector3 GetForward()
		{
			return StateMachine.UnitModel.CalculatedDeltaAimPos;
		}
	}
}
