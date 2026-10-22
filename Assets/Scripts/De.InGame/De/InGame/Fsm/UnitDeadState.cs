using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class UnitDeadState : UnitStateBase
	{
		public override EActionState ActionState => EActionState.Dead;

		public override bool IsAimEnable => false;

		public UnitDeadState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent is ReviveEvent)
			{
				return EActionState.Revive;
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return Vector3.zero;
		}
	}
}
