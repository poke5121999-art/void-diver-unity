using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class StationaryUnitDeadState : ExtraUnitStateBase
	{
		public override EActionState ActionState => EActionState.Dead;

		public override bool IsAimEnable => false;

		public StationaryUnitDeadState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			return EActionState.ReservedDespawn;
		}

		public override Vector3 GetForward()
		{
			return Vector3.zero;
		}
	}
}
