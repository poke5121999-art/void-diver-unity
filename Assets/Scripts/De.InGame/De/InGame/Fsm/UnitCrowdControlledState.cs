using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class UnitCrowdControlledState : UnitStateBase
	{
		private float _moveRemainTime;

		private float _moveSpeed;

		private Vector3 _moveDir;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		public UnitCrowdControlledState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent stateEvent)
		{
		}

		public override void OnExit()
		{
		}

		private void UpdateCcInfo(CrowdControlEvent crowdControlEvent)
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
