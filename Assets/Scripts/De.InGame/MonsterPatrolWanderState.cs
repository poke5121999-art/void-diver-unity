using De.InGame.Fsm;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrolWanderState : MonsterStateBase
{
	private bool _isMoveCompleted;

	private int _wanderPositionIndex;

	private Vector3[] _destinations;

	private int _destinationIndex;

	private Vector3 _currentPosition;

	private float _originAgentRadius;

	public override EActionState ActionState => EActionState.Wander;

	public override bool IsAimEnable => true;

	private Vector3[] PatrolWanderPositions => MonsterModel.PatrolWanderPositions;

	private Vector3 Destination => _destinations[_destinationIndex];

	public MonsterPatrolWanderState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		_currentPosition = OwnerPosition;
		if ((float)System.Math.Sqrt((_currentPosition - MonsterModel.PatrolWanderPositions[_wanderPositionIndex]).sqrMagnitude) < 1f)
		{
			_isMoveCompleted = true;
		}
		if (_isMoveCompleted)
		{
			int wanderPositionIndex = _wanderPositionIndex;
			_isMoveCompleted = false;
			_wanderPositionIndex = (wanderPositionIndex + 1) % MonsterModel.PatrolWanderPositions.Length;
		}
		int wanderPositionIndex2 = _wanderPositionIndex;
		Vector3[] patrolWanderPositions = MonsterModel.PatrolWanderPositions;
		int num = (patrolWanderPositions.Length - 1 + wanderPositionIndex2) % MonsterModel.PatrolWanderPositions.Length;
		Vector3 a = MonsterModel.PatrolWanderPositions[num];
		Vector3 vector = MonsterModel.PatrolWanderPositions[_wanderPositionIndex];
		Vector3 closestPointOnSegment = MathUtility.GetClosestPointOnSegment(a, vector, OwnerPosition);
		_destinationIndex = 0;
		if ((float)System.Math.Sqrt((closestPointOnSegment - OwnerPosition).sqrMagnitude) <= 1f)
		{
			_destinations = new Vector3[1] { vector };
		}
		else
		{
			_destinations = new Vector3[2] { closestPointOnSegment, vector };
		}
		_originAgentRadius = NavMeshAgent.radius;
		NavMeshAgent.radius = 0.1f;
		UnitView.UpdateAgentPath(_destinations[_destinationIndex]);
	}

	public override void OnExit()
	{
		if (IsActivated)
		{
			base.OnExit();
			NavMeshAgent.radius = _originAgentRadius;
			UnitView.ResetAgentPath();
		}
	}

	public override void OnUpdate()
	{
		UnitModel.MoveDir = (OwnerPosition - _currentPosition).normalized;
		_currentPosition = OwnerPosition;
		NavMeshAgent agent = UnitView.Agent;
		if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
		{
			if (_destinationIndex == _destinations.Length - 1)
			{
				_isMoveCompleted = true;
			}
			else
			{
				_destinationIndex++;
				UnitView.UpdateAgentPath(_destinations[_destinationIndex]);
			}
		}
		if (GetForward() != Vector3.zero)
		{
			UnitModel.SetCalculatedDeltaAimPos(GetForward());
		}
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
					UnitModel.SetCalculatedDeltaAimPos(TargetPosition - OwnerPosition);
					return MonsterModel.IsInCombat ? EActionState.MoveToTarget : EActionState.Aggro;
				}
			}
			else if (unitEvent is CrowdControlEvent crowdControlEvent)
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
				if (unitEvent is UseSkillEvent)
				{
					return EActionState.Skill;
				}
				if (unitEvent is ReceiveAggroSpreadEvent receiveAggroSpreadEvent)
				{
					if (MonsterModel.AggroChainDepth == 0 && receiveAggroSpreadEvent.Target != null)
					{
						SetTarget(receiveAggroSpreadEvent.Target);
						MonsterModel.AggroChainDepth = receiveAggroSpreadEvent.ChainDepth + 1;
						return EActionState.ReceiveAggroSpread;
					}
				}
				else if (unitEvent is TryMonsterCuriousStateEvent && Target == null)
				{
					return EActionState.Curious;
				}
			}
		}
		if (_isMoveCompleted)
		{
			return EActionState.Idle;
		}
		if (MonsterModel.IsPendingFleeState)
		{
			MonsterModel.IsPendingFleeState = false;
			return EActionState.Flee;
		}
		if (MonsterModel.IsPendingAggroState)
		{
			MonsterModel.IsPendingAggroState = false;
			if (Target != null)
			{
				return MonsterModel.IsInCombat ? EActionState.MoveToTarget : EActionState.Aggro;
			}
		}
		if (Target is UnitModel unitModel && !unitModel.IsHitable)
		{
			SetTarget(null);
		}
		if (Target == null)
		{
			SetTarget(FindEnemyInSight());
		}
		if (Target == null)
		{
			return EActionState.None;
		}
		return EActionState.Idle;
	}

	public override Vector3 GetForward()
	{
		return UnitModel.RxMoveDir.Value;
	}
}
