using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class CharacterCutsceneState : UnitStateBase
	{
		public override EActionState ActionState => EActionState.Cutscene;

		public override bool IsAimEnable => false;

		public CharacterCutsceneState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			Logger.Debug("Cutscene State Entered");
			UnitModel.AddBuff(TConst.CutsceneStateBuffId.IntValue, UnitModel);
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				UnitModel.RemoveBuff(TConst.CutsceneStateBuffId.IntValue);
				base.OnExit();
			}
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent is CutsceneFinishEvent)
			{
				return EActionState.Idle;
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return UnitModel.CalculatedDeltaAimPos;
		}
	}
}
