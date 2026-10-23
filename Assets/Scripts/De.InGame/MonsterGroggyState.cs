using De.InGame.Fsm;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public class MonsterGroggyState : MonsterStateBase
{
	private float _groggyEndTime;

	public override EActionState ActionState => EActionState.Groggy;

	public override bool IsAimEnable => false;

	public MonsterGroggyState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		MonsterModel.GroggyCount++;
		_groggyEndTime = TimeNow + TConst.GroggyDuration.FloatValue;
	}

	public override void OnUpdate()
	{
		UnitView.SyncPosition();
	}

	public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
	{
		if (unitEvent != null)
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
		return (_groggyEndTime <= TimeNow) ? EActionState.Idle : EActionState.None;
	}

	public override void OnExit()
	{
		base.OnExit();
		MonsterModel monsterModel = MonsterModel;
		float timeNow = TimeNow;
		monsterModel.SuppressionImmunityEndTime = timeNow + TConst.SuppressionImmunityDuration.FloatValue;
		MonsterModel.ResetSuppressionGauge();
	}

	public override Vector3 GetForward()
	{
		return UnitModel.Forward;
	}
}
