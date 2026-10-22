using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class CharacterInteractState : UnitStateBase
	{
		private EInteractionState _interactionState;

		private float _holdingRemainTime;

		private bool _reservedLightOn;

		private IInteractiveObject _target;

		public override EActionState ActionState => EActionState.Interact;

		public override bool IsAimEnable => false;

		private CharacterView CharacterView => UnitView as CharacterView;

		private CharacterModel CharacterModel => UnitModel as CharacterModel;

		public CharacterInteractState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			TryInteractEvent tryInteractEvent = unitEvent as TryInteractEvent;
			_target = CharacterView.GetInteractableObject(tryInteractEvent.NetworkObjectId);
			if (_target is MonsterBody || _target is RewardBox)
			{
				_reservedLightOn = CharacterModel.IsLightOn;
				CharacterModel.IsLightOn = false;
			}
			_interactionState = EInteractionState.Holding;
			GameHost.SendToAll(new NtfStartInteractionHolding
			{
				SourceClientId = CharacterModel.ClientId,
				TargetNetworkObjectId = _target.NetworkObjectId
			});
			_holdingRemainTime = _target.HoldingTime;
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				base.OnExit();
				if (_target != null)
				{
					GameHost.SendToAll(new NtfEndInteraction
					{
						SourceClientId = CharacterModel.ClientId,
						TargetNetworkObjectId = _target.NetworkObjectId
					});
				}
				if (_reservedLightOn)
				{
					CharacterModel.IsLightOn = true;
				}
				_interactionState = EInteractionState.None;
			}
		}

		public override void OnUpdate()
		{
			if (_interactionState == EInteractionState.Completed)
			{
				return;
			}
			if (_interactionState == EInteractionState.Holding)
			{
				_holdingRemainTime -= Time.deltaTime;
				if (0f < _holdingRemainTime)
				{
					return;
				}
				GameHost.SendToAll(new NtfStartInteraction
				{
					SourceClientId = CharacterModel.ClientId,
					TargetNetworkObjectId = _target.NetworkObjectId
				});
				_interactionState = EInteractionState.Interacting;
			}
			if (_interactionState == EInteractionState.Interacting && !CharacterModel.IsWaitingInteractionFinish)
			{
				_interactionState = EInteractionState.Completed;
			}
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
				else if (unitEvent is CancelInteractEvent cancelInteractEvent)
				{
					CharacterView characterView = CharacterView;
					if (characterView != null && characterView.GetInteractableObject(cancelInteractEvent.NetworkObjectId) != null && !CharacterModel.IsWaitingInteractionFinish)
					{
						return EActionState.Idle;
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
				else if (unitEvent is EscapeEvent)
				{
					return EActionState.Escaped;
				}
				else if (unitEvent is StageReadyEvent && GameManager.IsInLounge)
				{
					return EActionState.StageReady;
				}
			}
			if (TriggerActiveSkill(unitEvent) || _interactionState != EInteractionState.Completed)
			{
				return EActionState.None;
			}
			if (!(UnitModel.MoveDir != Vector3.zero))
			{
				return EActionState.Idle;
			}
			return EActionState.Move;
		}

		public override Vector3 GetForward()
		{
			return UnitModel.CalculatedDeltaAimPos;
		}
	}
}
