using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterPauseState : MonsterStateBase
	{
		private bool _isCompleted;

		private float _remainTime;

		public override EActionState ActionState => EActionState.Pause;

		public override bool IsAimEnable => false;

		public MonsterPauseState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_remainTime = MonsterData.PauseStateDuration;
		}

		public override void OnUpdate()
		{
			if (_remainTime > 0f)
			{
				_remainTime -= Time.fixedDeltaTime;
				if (_remainTime <= 0f)
				{
					_isCompleted = true;
				}
			}
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				base.OnExit();
				_isCompleted = false;
			}
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent != null)
			{
				if (unitEvent is CrowdControlEvent crowdControlEvent)
				{
					if (crowdControlEvent.CcInfo.ccType != ECrowdControlType.None)
					{
						return EActionState.CrowdControlled;
					}
				}
				else
				{
					if (unitEvent is DieEvent)
					{
						return EActionState.Dying;
					}
					if (unitEvent is CaptureEvent)
					{
						return EActionState.Captured;
					}
				}
			}
			if (!_isCompleted)
			{
				return EActionState.None;
			}
			return EActionState.Idle;
		}

		public override Vector3 GetForward()
		{
			return StateMachine.UnitModel.MoveDir;
		}
	}
}
