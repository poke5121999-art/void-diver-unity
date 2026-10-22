using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class CharacterStageReadyState : UnitStateBase
	{
		public override EActionState ActionState => EActionState.StageReady;

		public override bool IsAimEnable => false;

		public CharacterStageReadyState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			Logger.Debug("Ready State Entered");
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent is CutsceneStartEvent)
			{
				GameManager.OnCancelStartStage();
				return EActionState.Cutscene;
			}
			if (unitEvent is TryInteractEvent)
			{
				GameManager.OnCancelStartStage();
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
