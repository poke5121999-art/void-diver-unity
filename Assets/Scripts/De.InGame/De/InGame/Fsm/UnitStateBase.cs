using GameCommon.Data;
using GameCommon.Enum;
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

		protected float TimeNow => 0f;

		protected UnitStateMachine StateMachine { get; }

		protected EActionState PreviousState => EActionState.None;

		protected UnitModel UnitModel => null;

		protected UnitView UnitView => null;

		protected UnitController UnitController => null;

		public abstract EActionState ActionState { get; }

		public abstract bool IsAimEnable { get; }

		protected UnitStateBase(UnitStateMachine stateMachine)
		{
		}

		public virtual void OnEnter(IUnitEvent unitEvent)
		{
		}

		public virtual void OnExit()
		{
		}

		public abstract void OnUpdate();

		public abstract EActionState GetNextStateEvent(IUnitEvent unitEvent);

		public abstract Vector3 GetForward();

		public virtual void Clear()
		{
		}

		protected virtual bool TriggerActiveSkill(IUnitEvent unitEvent)
		{
			return false;
		}

		private bool CheckActiveItemSkillTriggere(int skillId, UseItemEvent useItemEvent)
		{
			return false;
		}

		private bool CheckActiveSkillTriggered(int skillId, IUnitEvent unitEvent)
		{
			return false;
		}

		public bool CheckCurrentSkillInputTriggered(int skillId, SkillTrigger skillTrigger, SkillInputEvent skillInputEvent)
		{
			return false;
		}

		private ESkillInputEvent GetSkillInputType(int skillId, bool isUp = false)
		{
			return ESkillInputEvent.None;
		}

		protected int GetSkillId(ESkillInputEvent inputEvent)
		{
			return 0;
		}
	}
}
