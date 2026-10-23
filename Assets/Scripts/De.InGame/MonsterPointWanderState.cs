using De.InGame.Fsm;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPointWanderState : MonsterStateBase
{
	private static readonly int WalkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");

	private float _stateEndTime;

	private bool _isMoveCompleted;

	private NavMeshPath _navMeshPath;

	private int _currentCornerIndex;

	private Vector3 _prevDirection;

	private bool _pathInitialized;

	public override EActionState ActionState => EActionState.Wander;

	public override bool IsAimEnable => false;

	private bool HasPath => _navMeshPath != null;

	public MonsterPointWanderState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		_isMoveCompleted = false;
		_prevDirection = UnitModel.CalculatedDeltaAimPos;
		_pathInitialized = false;
	}

	public override void OnExit()
	{
		if (IsActivated)
		{
			base.OnExit();
			MonsterModel.IsWanderSpecificPosition = false;
			MonsterModel.WanderSpecificPosition = Vector3.zero;
		}
	}

	public override void OnUpdate()
	{
		if (_pathInitialized)
		{
			UpdateMove();
			return;
		}
		if (!MonsterModel.IsWanderSpecificPosition)
		{
			float timeNow = TimeNow;
			_stateEndTime = timeNow + RandomUtility.GetRandomFloat(TConst.WanderMinTime.FloatValue, TConst.WanderMaxTime.FloatValue);
			InitializeRandomPath();
		}
		else
		{
			_stateEndTime = TimeNow + TConst.WanderSpecificMaxTime.FloatValue;
			InitializeSpecificPath(MonsterModel.WanderSpecificPosition);
		}
		_pathInitialized = true;
	}

	private void UpdateMove()
	{
		if (_stateEndTime < TimeNow)
		{
			_isMoveCompleted = true;
		}
		else
		{
			if (_navMeshPath == null)
			{
				return;
			}
			if ((float)System.Math.Sqrt((OwnerPosition - _navMeshPath.corners[_currentCornerIndex]).sqrMagnitude) < 0.25f)
			{
				if (_currentCornerIndex < _navMeshPath.corners.Length - 1)
				{
					_currentCornerIndex++;
				}
				else
				{
					_isMoveCompleted = true;
				}
				if (GetForward() != Vector3.zero)
				{
					UnitModel.SetCalculatedDeltaAimPos(GetForward());
				}
			}
			Vector3 vector = _navMeshPath.corners[_currentCornerIndex] - OwnerPosition;
			if (vector != Vector3.zero)
			{
				_prevDirection = vector;
			}
			UnitModel.MoveDir = GetForward().normalized;
			float num = (float)System.Math.Sqrt((OwnerPosition - _navMeshPath.corners[_currentCornerIndex]).sqrMagnitude);
			float num2 = Mathf.Min(UnitModel.GetStat(EStatType.MoveSpeed), num / Time.fixedDeltaTime);
			UnitView.Move(UnitModel.RxMoveDir.Value, num2, Time.fixedDeltaTime);
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
					if (MonsterModel.AggroChainDepth == 0)
					{
						SetTarget(receiveAggroSpreadEvent.Target);
						MonsterModel.AggroChainDepth = receiveAggroSpreadEvent.ChainDepth + 1;
						return EActionState.ReceiveAggroSpread;
					}
				}
				else if (unitEvent is TryMonsterCuriousStateEvent tryMonsterCuriousStateEvent && Target == null)
				{
					MonsterModel.WanderSpecificPosition = tryMonsterCuriousStateEvent.TargetPosition;
					if (!MonsterModel.IsWanderSpecificPosition)
					{
						return EActionState.Curious;
					}
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
			return MonsterModel.IsInCombat ? EActionState.MoveToTarget : EActionState.Aggro;
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
		if (_navMeshPath == null)
		{
			return UnitModel.CalculatedDeltaAimPos;
		}
		Vector3 vector = _navMeshPath.corners[_currentCornerIndex] - OwnerPosition;
		if (vector == Vector3.zero)
		{
			return _prevDirection;
		}
		return vector;
	}

	private void InitializeSpecificPath(Vector3 position)
	{
		_navMeshPath = null;
		if (NavMesh.SamplePosition(position, out var hit, 1f, WalkableAreaMask))
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			if (NavMeshAgent.CalculatePath(hit.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
			{
				_navMeshPath = navMeshPath;
				_currentCornerIndex = 0;
			}
		}
	}

	private void InitializeRandomPath()
	{
		List<int> list = Enumerable.Range(0, 8).OrderBy((int _) => Random.value).ToList();
		float randomFloat = RandomUtility.GetRandomFloat(0f, 45f);
		_navMeshPath = null;
		for (int i = 0; i < 8; i++)
		{
			Vector3 vector = Quaternion.Euler(0f, (float)list[i] * 45f + randomFloat, 0f) * Vector3.forward;
			float wanderRange = MonsterData.WanderRange;
			Vector3 sourcePosition = SpawnPosition + vector * wanderRange;
			if (NavMesh.SamplePosition(sourcePosition, out var hit, 1f, WalkableAreaMask))
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				if (NavMeshAgent.CalculatePath(hit.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
				{
					_navMeshPath = navMeshPath;
					_currentCornerIndex = 0;
					return;
				}
			}
		}
	}
}
