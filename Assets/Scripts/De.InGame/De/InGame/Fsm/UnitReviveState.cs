using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class UnitReviveState : UnitStateBase
	{
		private float _reviveTime;

		private float _customHpPercent;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		private int ReviveDebuffId => 0;

		private float ReviveHpPercent => 0f;

		public UnitReviveState(UnitStateMachine stateMachine)
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
