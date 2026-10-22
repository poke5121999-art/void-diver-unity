using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class CharacterIdleState : UnitStateBase
	{
		public override EActionState ActionState => EActionState.Idle;

		public override bool IsAimEnable => true;

		private CharacterView CharacterView => UnitView as CharacterView;

		public CharacterIdleState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnUpdate()
		{
			UnitView.SyncPosition();
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent != null)
			{
				if (unitEvent is CutsceneStartEvent)
				{
					return EActionState.Cutscene;
				}
				if (unitEvent is CrowdControlEvent crowdControlEvent)
				{
					if (crowdControlEvent.CcInfo.ccType != ECrowdControlType.None)
					{
						return EActionState.CrowdControlled;
					}
				}
				else if (unitEvent is TryInteractEvent tryInteractEvent)
				{
					if (CharacterView.GetInteractableObject(tryInteractEvent.NetworkObjectId) != null)
					{
						return EActionState.Interact;
					}
				}
				else if (unitEvent is UseSkillEvent)
				{
					return EActionState.Skill;
				}
				else if (unitEvent is DieEvent)
				{
					return EActionState.Dying;
				}
				else if (unitEvent is StageReadyEvent)
				{
					if (GameManager.IsInLounge)
					{
						return EActionState.StageReady;
					}
				}
				else if (unitEvent is EscapeEvent)
				{
					return EActionState.Escaped;
				}
			}
			if (TriggerActiveSkill(unitEvent))
			{
				return EActionState.None;
			}
			if (!(UnitModel.MoveDir != Vector3.zero))
			{
				return EActionState.None;
			}
			return EActionState.Move;
		}

		public override Vector3 GetForward()
		{
			return UnitModel.CalculatedDeltaAimPos;
		}
	}
}
