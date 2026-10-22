using GameCommon.Enum;
using UnityEngine;
using UnityEngine.AI;

namespace De.InGame.Fsm
{
	public class MonsterFleeState : MonsterStateBase
	{
		private bool _isCompleted;

		private float _remainTime;

		private Vector3 _forward;

		private Vector3 _targetPosition;

		public override EActionState ActionState => EActionState.Flee;

		public override bool IsAimEnable => false;

		public MonsterFleeState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			Vector3 normalized;
			if (MonsterModel.Target == null)
			{
				normalized = (MonsterView.SpawnPosition - StateMachine.UnitView.transform.position).normalized;
			}
			else
			{
				normalized = (StateMachine.UnitView.transform.position - MonsterModel.Target.Position).normalized;
			}
			float num = StateMachine.UnitModel.MoveSpeed * MonsterData.FleeDuration;
			Vector3 sourcePosition = StateMachine.UnitView.transform.position + normalized * num;
			if (NavMesh.SamplePosition(sourcePosition, out var hit, 5f, 1))
			{
				_targetPosition = hit.position;
				_remainTime = MonsterData.FleeDuration;
				_forward = (hit.position - StateMachine.UnitView.transform.position).normalized;
				StateMachine.UnitModel.SetCalculatedDeltaAimPos(GetForward());
				SetTarget(null);
			}
		}

		public override void OnUpdate()
		{
			if (!(_remainTime <= 0f))
			{
				StateMachine.UnitModel.MoveDir = GetForward().normalized;
				StateMachine.UnitView.Move(StateMachine.UnitModel.MoveDir, StateMachine.UnitModel.MoveSpeed, Time.fixedDeltaTime);
				_remainTime -= Time.fixedDeltaTime;
				if (!(_remainTime > 0f))
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
					if (crowdControlEvent.CcInfo.ccType != 0)
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
			if (Vector3.Distance(StateMachine.UnitView.transform.position, _targetPosition) < 0.5f)
			{
				return EActionState.Idle;
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return _forward;
		}
	}
}
