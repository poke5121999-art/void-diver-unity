using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class UnitReviveState : UnitStateBase
	{
		private float _reviveTime;

		private float _customHpPercent;

		public override EActionState ActionState => EActionState.Revive;

		public override bool IsAimEnable => false;

		private int ReviveDebuffId => TConst.ReviveDebuffId.IntValue;

		private float ReviveHpPercent => TConst.ReviveHpPercent.FloatValue;

		public UnitReviveState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_customHpPercent = ((unitEvent is ReviveEvent reviveEvent && reviveEvent.IsEquipmentRevive) ? reviveEvent.EquipmentReviveHpPercent : 0f);
			GameHost.SendToAll(new NtfDisplayFx
			{
				Vfx = "Common/Heal",
				VfxDuration = 1f,
				Sfx = string.Empty,
				Position = StateMachine.UnitView.transform.position.ToProtoVector3()
			});
			_reviveTime = GameManager.TimeNow + 1f;
		}

		public override void OnExit()
		{
			if (!IsActivated)
			{
				return;
			}
			base.OnExit();
			if (ReviveDebuffId > 0)
			{
				StateMachine.UnitModel.AddBuff(ReviveDebuffId, StateMachine.UnitModel, 1, 0uL, Vector3.zero);
			}
			StateMachine.UnitModel.AddBuff(3000001, StateMachine.UnitModel, 1, 0uL, Vector3.zero);
			float num = _customHpPercent;
			if (num <= 0f)
			{
				num = ReviveHpPercent;
			}
			UnitModel unitModel = StateMachine.UnitModel;
			unitModel.Hp = (int)(unitModel.HpMax * num / 100f);
			unitModel.Stress = Mathf.Min(unitModel.Stress, (float)TConst.ReviveStress.IntValue);
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent is CutsceneStartEvent)
			{
				return EActionState.Cutscene;
			}
			if (!(_reviveTime < GameManager.TimeNow))
			{
				return EActionState.None;
			}
			return EActionState.Idle;
		}

		public override Vector3 GetForward()
		{
			return Vector3.zero;
		}
	}
}
