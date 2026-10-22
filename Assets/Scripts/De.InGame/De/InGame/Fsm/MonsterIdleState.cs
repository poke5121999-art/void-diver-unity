using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterIdleState : MonsterStateBase
	{
		private float _stateStartTime;

		private float _wanderStartDelay;

		public override EActionState ActionState => EActionState.Idle;

		public override bool IsAimEnable => true;

		private float StateElapsedTime => GameManager.TimeNow - _stateStartTime;

		public MonsterIdleState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_stateStartTime = GameManager.TimeNow;
			if (MonsterController.IsPatrolMonster)
			{
				_wanderStartDelay = TConst.PatrolWanderWaitTime.FloatValue;
			}
			else
			{
				_wanderStartDelay = Random.Range(TConst.WanderWaitTimeMin.FloatValue, TConst.WanderWaitTimeMax.FloatValue);
			}
		}

		public override void OnUpdate()
		{
			StateMachine.UnitView.SyncPosition();
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent != null)
			{
				if (unitEvent is HpDamageEvent hpDamageEvent)
				{
					if (hpDamageEvent.SourceModel != null && hpDamageEvent.SourceModel.IsHitable && IsEnemy(hpDamageEvent.SourceModel))
					{
						SetTarget(hpDamageEvent.SourceModel);
						StateMachine.UnitModel.SetCalculatedDeltaAimPos(TargetPosition - StateMachine.UnitView.transform.position);
						if (!MonsterModel.IsInCombat)
						{
							return EActionState.Aggro;
						}
						return EActionState.MoveToTarget;
					}
				}
				else if (unitEvent is CrowdControlEvent crowdControlEvent)
				{
					if (crowdControlEvent.CcInfo.ccType != 0)
					{
						MonsterModel.IsPendingAggroState = true;
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
					if (unitEvent is ReceiveAggroSpreadEvent receiveAggroSpreadEvent)
					{
						if (MonsterModel.AggroChainDepth == 0)
						{
							SetTarget(receiveAggroSpreadEvent.Target);
							MonsterModel.AggroChainDepth = receiveAggroSpreadEvent.ChainDepth + 1;
							return EActionState.ReceiveAggroSpread;
						}
					}
					else if (unitEvent is TryMonsterCuriousStateEvent tryMonsterCuriousStateEvent && MonsterModel.Target == null)
					{
						MonsterModel.WanderSpecificPosition = tryMonsterCuriousStateEvent.TargetPosition;
						return EActionState.Curious;
					}
				}
			}
			if ((StateMachine.UnitModel as MonsterModel).IsPendingFleeState)
			{
				MonsterModel.IsPendingFleeState = false;
				return EActionState.Flee;
			}
			if (MonsterModel.IsPendingAggroState)
			{
				MonsterModel.IsPendingAggroState = false;
				if (MonsterModel.Target != null)
				{
					return MonsterModel.IsInCombat ? EActionState.MoveToTarget : EActionState.Aggro;
				}
			}
			if (MonsterModel.Target is UnitModel unitModel && !unitModel.IsHitable)
			{
				SetTarget(null);
			}
			if (MonsterModel.Target == null)
			{
				SetTarget(FindEnemyInSight());
			}
			if (MonsterModel.Target == null)
			{
				return (GameManager.TimeNow - _stateStartTime > _wanderStartDelay) ? EActionState.Wander : EActionState.None;
			}
			StateMachine.UnitModel.SetCalculatedDeltaAimPos(TargetPosition - StateMachine.UnitView.transform.position);
			if (!IsTargetInSight(MonsterModel.Target))
			{
				if (GameManager.TimeNow - _stateStartTime > TConst.TargetLostInSightTime.FloatValue)
				{
					SetTarget(FindEnemyInSight());
					return EActionState.None;
				}
			}
			if (MonsterModel.IsInCombat)
			{
				if (CheckSkill(out var aiSkillDecision))
				{
					HandleUseSkillEvent(aiSkillDecision);
					return EActionState.None;
				}
				if (CheckTargetInKiteRange())
				{
					return EActionState.KiteFromTarget;
				}
				return IsTargetInRange(MonsterData.StartMoveToTargetRange) ? EActionState.None : EActionState.MoveToTarget;
			}
			if (IsTargetInSight(MonsterModel.Target, TConst.MonsterDirectAggroSightRangeMultiplier.FloatValue, TConst.MonsterDirectAggroSightBackRangeMultiplier.FloatValue))
			{
				return EActionState.Aggro;
			}
			return IsTargetInSight(MonsterModel.Target) ? EActionState.Alert : EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return StateMachine.UnitModel.CalculatedDeltaAimPos;
		}
	}
}
