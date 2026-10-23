using De.InGame.Fsm;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public class MonsterAlertState : MonsterStateBase
{
	private float _alertTargetStartTime;

	private float _lostTargetStartTime;

	private float _lastStateFinishAsLostTargetTime = float.MaxValue;

	public override EActionState ActionState => EActionState.Alert;

	public override bool IsAimEnable => true;

	private float RequiredTimeToAggro
	{
		get
		{
			if (TimeNow > _lastStateFinishAsLostTargetTime && TimeNow - _lastStateFinishAsLostTargetTime <= TConst.MonsterAlertLostTargetRecentTime.FloatValue)
			{
				return TConst.MonsterAlertTrackTargetTime.FloatValue * TConst.MonsterAlertTrackTargetTimeMultiplier.FloatValue;
			}
			return TConst.MonsterAlertTrackTargetTime.FloatValue;
		}
	}

	private float TargetTrackingTime => TimeNow - _alertTargetStartTime;

	private float TargetLostTime
	{
		get
		{
			if (_lostTargetStartTime >= 0f)
			{
				return TimeNow - _lostTargetStartTime;
			}
			return 0f;
		}
	}

	public MonsterAlertState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		_alertTargetStartTime = TimeNow;
		_lostTargetStartTime = -1f;
	}

	public override void OnUpdate()
	{
		UnitView.SyncPosition(-1f);
		if (Target == null)
		{
			if (_lostTargetStartTime < 0f)
			{
				_lostTargetStartTime = TimeNow;
			}
		}
		else
		{
			_lostTargetStartTime = -1f;
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
				if (unitEvent is UseSkillEvent)
				{
					return EActionState.Skill;
				}
			}
		}
		if (!IsTargetInSight(Target))
		{
			ITarget target = FindEnemyInSight();
			if (target == null)
			{
				SetTarget(null);
			}
			else
			{
				SetTarget(target);
				_alertTargetStartTime = TimeNow;
			}
		}
		if (Target != null)
		{
			UnitModel.SetCalculatedDeltaAimPos(TargetPosition - OwnerPosition);
			if (IsTargetInDirectAggroSight(Target) || TargetTrackingTime > RequiredTimeToAggro)
			{
				return EActionState.Aggro;
			}
		}
		if (Target == null && TargetLostTime >= TConst.MonsterAlertLostTargetTime.FloatValue)
		{
			_lastStateFinishAsLostTargetTime = TimeNow;
			return EActionState.Idle;
		}
		return EActionState.None;
	}

	public override Vector3 GetForward()
	{
		return UnitModel.CalculatedDeltaAimPos;
	}
}
