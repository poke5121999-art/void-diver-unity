using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterMoveToTargetState : MonsterStateBase
	{
		private Vector3 _currentPosition;

		private float _targetLostInSightTime;

		private float _targetOutsideFollowRangeElapsedTime;

		public override EActionState ActionState => EActionState.MoveToTarget;

		public override bool IsAimEnable => true;

		public MonsterMoveToTargetState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			if (MonsterModel.Target == null)
			{
				Logger.Warning(string.Format("NoneTarget MoveToTarget {0}, previous : {1}", StateMachine.UnitModel.TableId, StateMachine.PreviousState));
				return;
			}
			_currentPosition = StateMachine.UnitView.transform.position;
			_targetLostInSightTime = 0f;
			_targetOutsideFollowRangeElapsedTime = 0f;
			StateMachine.UnitModel.SetCalculatedDeltaAimPos(TargetPosition - StateMachine.UnitView.transform.position);
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				base.OnExit();
				StateMachine.UnitView.ResetAgentPath();
			}
		}

		public override void OnUpdate()
		{
			if (MonsterModel.Target != null)
			{
				StateMachine.UnitView.UpdateAgentPath(MonsterModel.Target.Position);
				StateMachine.UnitModel.MoveDir = (StateMachine.UnitView.transform.position - _currentPosition).normalized;
				_currentPosition = StateMachine.UnitView.transform.position;
				StateMachine.UnitModel.SetCalculatedDeltaAimPos(TargetPosition - StateMachine.UnitView.transform.position);
				if (IsTargetInSight(MonsterModel.Target))
				{
					_targetLostInSightTime = 0f;
				}
				else
				{
					_targetLostInSightTime += Time.fixedDeltaTime;
				}
				if (CheckTargetOutsideFollowRange())
				{
					_targetOutsideFollowRangeElapsedTime += Time.fixedDeltaTime;
				}
				else
				{
					_targetOutsideFollowRangeElapsedTime = 0f;
				}
			}
			StateMachine.UnitView.SyncPosition(StateMachine.UnitModel.MoveSpeed);
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (MonsterModel.Target == null)
			{
				return EActionState.Idle;
			}
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
					if (unitEvent is UseSkillEvent)
					{
						return EActionState.Skill;
					}
				}
			}
			if (MonsterModel.IsPendingFleeState)
			{
				MonsterModel.IsPendingFleeState = false;
				return EActionState.Flee;
			}
			if (MonsterModel.IsPendingAggroState)
			{
				MonsterModel.IsPendingAggroState = false;
				return MonsterModel.IsInCombat ? EActionState.MoveToTarget : EActionState.Aggro;
			}
			if (_targetLostInSightTime > TConst.TargetLostInSightTime.FloatValue)
			{
				return EActionState.LostTarget;
			}
			if (CheckHighStressTarget(out var target))
			{
				SetTarget(target);
				return EActionState.None;
			}
			if (CheckTargetOutsideBlindRange())
			{
				return EActionState.LostTarget;
			}
			if (CheckSkill(out var aiSkillDecision))
			{
				HandleUseSkillEvent(aiSkillDecision);
				return EActionState.None;
			}
			if (CheckTargetInKiteRange())
			{
				return EActionState.KiteFromTarget;
			}
			if (_targetOutsideFollowRangeElapsedTime > TConst.TargetFollowRangeGiveUpTime.FloatValue)
			{
				return EActionState.LostTarget;
			}
			if (IsTargetInRange(MonsterData.EndMoveToTargetRange))
			{
				return EActionState.Idle;
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return StateMachine.UnitModel.CalculatedDeltaAimPos;
		}
	}
}
