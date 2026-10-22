using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class CharacterMoveState : UnitStateBase
	{
		private CompositeDisposable _disposable;

		public override EActionState ActionState => EActionState.Move;

		public override bool IsAimEnable => true;

		private CharacterView CharacterView => UnitView as CharacterView;

		private CharacterModel CharacterModel => UnitModel as CharacterModel;

		private CharacterController CharacterController => UnitController as CharacterController;

		public CharacterMoveState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_disposable?.Dispose();
			_disposable = new CompositeDisposable();
			UnitModel.RxMoveSpeed.Subscribe(delegate
			{
				UnitView.SetAnimationSpeed(UnitView.GetMoveAnimationSpeed());
			}).AddTo(_disposable);
		}

		public override void OnUpdate()
		{
			if (CharacterModel.TryRun)
			{
				float runStaminaCost = UnitModel.RunStaminaCost;
				float deltaTime = Time.deltaTime;
				if (deltaTime * runStaminaCost <= UnitModel.Stamina)
				{
					CharacterController.HandleUnitEvent(new StaminaDamageEvent
					{
						Amount = deltaTime * runStaminaCost
					});
					CharacterModel.TryRunExpireTime = GameManager.TimeNow + TConst.ToggleRunExpireDelay.FloatValue;
				}
				else
				{
					CharacterModel.TryRun = false;
				}
			}
			CharacterModel.IsMovingBackward = IsBackward(UnitModel.Forward, UnitModel.MoveDir);
			UnitView.Move(UnitModel.MoveDir, UnitModel.MoveSpeed, Time.fixedDeltaTime);
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				if (CharacterModel.TryRun)
				{
					CharacterModel.TryRunExpireTime = GameManager.TimeNow + TConst.ToggleRunExpireDelay.FloatValue;
				}
				CharacterModel.IsMovingBackward = false;
				_disposable?.Dispose();
				UnitView.SetAnimationSpeed(1f);
			}
		}

		private static bool IsBackward(Vector3 moveDir, Vector3 aimDir)
		{
			return Vector3.Dot(moveDir.normalized, aimDir.normalized) < Mathf.Cos(2.0943952f);
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
			if (!(UnitModel.MoveDir == Vector3.zero))
			{
				return EActionState.None;
			}
			return EActionState.Idle;
		}

		public override Vector3 GetForward()
		{
			return UnitModel.CalculatedDeltaAimPos;
		}
	}
}
