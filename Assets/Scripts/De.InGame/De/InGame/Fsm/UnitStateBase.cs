using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

namespace De.InGame.Fsm
{
	public abstract class UnitStateBase : NativeBase
	{
		[Inject]
		protected GameManager GameManager;

		[Inject]
		protected GameChannel GameChannel;

		[Inject]
		protected GameHost GameHost;

		[Inject]
		protected GameObjectManager ObjectManager;

		[Inject]
		protected SeriLogger Logger;

		[Inject]
		protected GameContext GameContext;

		private CompositeDisposable _disposable;

		protected bool IsActivated;

		protected float TimeNow => GameManager.TimeNow;

		protected UnitStateMachine StateMachine { get; }

		protected EActionState PreviousState => StateMachine.PreviousState;

		protected UnitModel UnitModel => StateMachine.UnitModel;

		protected UnitView UnitView => StateMachine.UnitView;

		protected UnitController UnitController => StateMachine.UnitController;

		public abstract EActionState ActionState { get; }

		public abstract bool IsAimEnable { get; }

		protected UnitStateBase(UnitStateMachine stateMachine)
		{
			StateMachine = stateMachine;
		}

		public virtual void OnEnter(IUnitEvent unitEvent)
		{
			_disposable = new CompositeDisposable();
			IsActivated = true;
		}

		public virtual void OnExit()
		{
			if (IsActivated)
			{
				if (_disposable != null)
				{
					_disposable.Dispose();
				}
				IsActivated = false;
			}
		}

		public abstract void OnUpdate();

		public abstract EActionState GetNextStateEvent(IUnitEvent unitEvent);

		public abstract Vector3 GetForward();

		public virtual void Clear()
		{
			if (_disposable != null)
			{
				_disposable.Dispose();
			}
			_disposable = null;
			IsActivated = false;
		}

		protected virtual bool TriggerActiveSkill(IUnitEvent unitEvent)
		{
			if (unitEvent == null)
			{
				return false;
			}
			foreach (int activeSkillId in StateMachine.UnitModel.GetActiveSkillIds())
			{
				if (CheckActiveSkillTriggered(activeSkillId, unitEvent))
				{
					UnitController controller = StateMachine.UnitController;
					UseSkillEvent useSkillEvent = new UseSkillEvent();
					useSkillEvent.SkillId = activeSkillId;
					useSkillEvent.TriggerTarget = StateMachine.UnitModel;
					controller.HandleUnitEvent(useSkillEvent);
					return true;
				}
			}
			if (StateMachine.UnitModel is CharacterModel && unitEvent is UseItemEvent useItemEvent)
			{
				int skillId = TItem.Table[useItemEvent.ItemId].SkillId;
				if (!TSkill.Table.TryGetValue(skillId, out var skill))
				{
					return false;
				}
				UnitModel unitModel = StateMachine.UnitModel;
				SkillTrigger trigger = skill.RootActionNode.SkillTrigger;
				if (!useItemEvent.CheckSkillTriggered(unitModel, trigger, out var _))
				{
					return false;
				}
				UnitController controller2 = StateMachine.UnitController;
				UseSkillEvent useSkillEvent2 = new UseSkillEvent();
				useSkillEvent2.SkillId = skillId;
				useSkillEvent2.TriggerTarget = StateMachine.UnitModel;
				controller2.HandleUnitEvent(useSkillEvent2);
				return true;
			}
			return false;
		}

		private bool CheckActiveItemSkillTriggere(int skillId, UseItemEvent useItemEvent)
		{
			if (!TSkill.Table.TryGetValue(skillId, out var skill))
			{
				return false;
			}
			UnitModel unitModel = StateMachine.UnitModel;
			SkillTrigger trigger = skill.RootActionNode.SkillTrigger;
			return useItemEvent.CheckSkillTriggered(unitModel, trigger, out var _);
		}

		private bool CheckActiveSkillTriggered(int skillId, IUnitEvent unitEvent)
		{
			if (!TSkill.Table.TryGetValue(skillId, out var skill) || unitEvent == null)
			{
				return false;
			}
			if (!(unitEvent is SkillInputEvent skillInputEvent))
			{
				return false;
			}
			if (skill.SkillType == ESkillType.Passive || skill.SkillType == ESkillType.Preview)
			{
				return false;
			}
			UnitModel unitModel = StateMachine.UnitModel;
			SkillTrigger trigger = skill.RootActionNode.SkillTrigger;
			if (!skillInputEvent.CheckSkillTriggered(unitModel, trigger, out var _) && !CheckCurrentSkillInputTriggered(skillId, skill.RootActionNode.SkillTrigger, skillInputEvent))
			{
				return false;
			}
			if (StateMachine.UnitModel.CheckSkill(skillId))
			{
				return true;
			}
			if (!skill.RequireWeapon)
			{
				return false;
			}
			// vtable+0x3d8 is slot 42 = UnitModel.get_WeaponId (abstract int property, metadata slot table checked) @0x1806cf673
			if (StateMachine.UnitModel.WeaponId > 0)
			{
				return false;
			}
			StateMachine.UnitController.OnShowMonologueText.OnNext(TLocalizedText.CannotUseSkillWithoutWeapon.LocalText);
			return false;
		}

		public bool CheckCurrentSkillInputTriggered(int skillId, SkillTrigger skillTrigger, SkillInputEvent skillInputEvent)
		{
			if (skillTrigger is SkillInputTrigger skillInputTrigger && (skillInputTrigger.SkillInputType == ESkillInputEvent.CurrentInputDown || skillInputTrigger.SkillInputType == ESkillInputEvent.CurrentInputUp))
			{
				ESkillInputEvent type = skillInputTrigger.SkillInputType;
				int up = (type == ESkillInputEvent.CurrentInputUp) ? 1 : 0;
				int expected;
				switch (StateMachine.UnitModel.GetActiveSkillIds().IndexOf(skillId))
				{
				case 0:
					expected = 1 + up;
					break;
				case 1:
					expected = 6 + up;
					break;
				case 2:
					expected = 4 + up;
					break;
				case 3:
					expected = 8 + up;
					break;
				case 4:
					expected = 10 + up;
					break;
				case 5:
					expected = 12 + up;
					break;
				case 6:
					expected = 14 + up;
					break;
				default:
					expected = 0;
					break;
				}
				return (int)skillInputEvent.SkillInputType == expected;
			}
			return false;
		}

		private ESkillInputEvent GetSkillInputType(int skillId, bool isUp = false)
		{
			int up = isUp ? 1 : 0;
			switch (StateMachine.UnitModel.GetActiveSkillIds().IndexOf(skillId))
			{
			case 0:
				return ESkillInputEvent.AttackDown + up;
			case 1:
				return ESkillInputEvent.Skill1Down + up;
			case 2:
				return ESkillInputEvent.DashDown + up;
			case 3:
				return ESkillInputEvent.Skill2Down + up;
			case 4:
				return ESkillInputEvent.Skill3Down + up;
			case 5:
				return ESkillInputEvent.Skill4Down + up;
			case 6:
				return ESkillInputEvent.EquipmentSkillDown + up;
			default:
				return ESkillInputEvent.None;
			}
		}

		protected int GetSkillId(ESkillInputEvent inputEvent)
		{
			int index;
			switch (inputEvent)
			{
			case ESkillInputEvent.AttackDown:
				index = 0;
				break;
			case ESkillInputEvent.Skill1Down:
				index = 1;
				break;
			case ESkillInputEvent.DashDown:
				index = 2;
				break;
			case ESkillInputEvent.Skill2Down:
				index = 3;
				break;
			case ESkillInputEvent.Skill3Down:
				index = 4;
				break;
			case ESkillInputEvent.Skill4Down:
				index = 5;
				break;
			case ESkillInputEvent.EquipmentSkillDown:
				index = 6;
				break;
			default:
				return -1;
			}
			List<int> activeSkillIds = StateMachine.UnitModel.GetActiveSkillIds();
			if (index >= activeSkillIds.Count)
			{
				return -1;
			}
			return activeSkillIds[index];
		}
	}
}
