using De.InGame.Fsm;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public class MonsterReceiveAggroSpreadState : MonsterStateBase
{
	private bool _isTimeCompleted;

	private float _remainTime;

	public override EActionState ActionState => EActionState.ReceiveAggroSpread;

	public override bool IsAimEnable => true;

	public MonsterReceiveAggroSpreadState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		_isTimeCompleted = false;
		_remainTime = TConst.MonsterReceiveAggroSpreadStateTime.FloatValue;
	}

	public override void OnUpdate()
	{
		if (_remainTime > 0f)
		{
			_remainTime -= Time.fixedDeltaTime;
			if (_remainTime <= 0f)
			{
				_isTimeCompleted = true;
			}
		}
		UnitView.SyncPosition(-1f);
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
		if (!_isTimeCompleted)
		{
			return EActionState.None;
		}
		if (Target == null)
		{
			return EActionState.Idle;
		}
		return EActionState.Aggro;
	}

	public override Vector3 GetForward()
	{
		return MonsterModel.CalculatedDeltaAimPos;
	}
}
