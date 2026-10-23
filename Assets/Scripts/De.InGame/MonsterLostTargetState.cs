using De.InGame.Fsm;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public class MonsterLostTargetState : MonsterStateBase
{
	private bool _isCompleted;

	private float _remainTime;

	public override EActionState ActionState => EActionState.LostTarget;

	public override bool IsAimEnable => false;

	public MonsterLostTargetState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		SetTarget(null);
		_isCompleted = false;
		_remainTime = TConst.MonsterLostTargetStateTime.FloatValue;
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
		UnitView.SyncPosition();
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
		return _isCompleted ? EActionState.Idle : EActionState.None;
	}

	public override Vector3 GetForward()
	{
		return MonsterModel.CalculatedDeltaAimPos;
	}
}
