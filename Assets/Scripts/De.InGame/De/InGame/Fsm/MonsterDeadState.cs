using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterDeadState : MonsterStateBase
	{
		private Inventory[] _lootingInventories;

		private MonsterBody _monsterBody;

		private int _nextPhaseMonsterId;

		private bool _isCompleted;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		public MonsterDeadState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		private void OnDeadDurationElapsed()
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
