using GameCommon.Enum;
using SomeNet.Utility;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterAvoidState : MonsterStateBase
	{
		private bool _isCompleted;

		private float _remainTime;

		private bool _isClockwise;

		public override EActionState ActionState => EActionState.Avoid;

		public override bool IsAimEnable => false;

		public MonsterAvoidState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_remainTime = MonsterData.AvoidStateDuration;
			_isClockwise = RandomUtility.GetRandomBool(0.5f);
		}

		public override void OnUpdate()
		{
			if (MonsterModel.Target == null || _remainTime <= 0f)
			{
				_isCompleted = true;
				return;
			}
			UnitModel.MoveDir = GetForward().normalized;
			UnitView.Move(UnitModel.MoveDir, UnitModel.MoveSpeed, Time.fixedDeltaTime);
			_remainTime -= Time.fixedDeltaTime;
			if (!(0f < _remainTime))
			{
				_isCompleted = true;
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
			if (_isCompleted)
			{
				return EActionState.Idle;
			}
			if (MonsterModel.Target is IHitable hitable && hitable.IsHitable)
			{
				float num = Vector3.Distance(UnitView.transform.position, TargetPosition);
				if (MonsterData.AvoidRange < num)
				{
					return EActionState.Idle;
				}
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			Vector3 position = UnitView.transform.position;
			Vector3 diff = position - TargetPosition;
			float distance = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);
			if (distance <= 0f)
			{
				return UnitModel.MoveDir;
			}
			float angle = Time.deltaTime * (UnitModel.MoveSpeed / distance) * 57.29578f;
			if (!_isClockwise)
			{
				angle = -angle;
			}
			return MathUtility.RotateAround(UnitView.transform.position, TargetPosition, angle) - UnitView.transform.position;
		}
	}
}
