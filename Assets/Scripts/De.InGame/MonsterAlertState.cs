using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class MonsterAlertState : MonsterStateBase
{
	private float _alertTargetStartTime;

	private float _lostTargetStartTime;

	private float _lastStateFinishAsLostTargetTime;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	private float RequiredTimeToAggro => 0f;

	private float TargetTrackingTime => 0f;

	private float TargetLostTime => 0f;

	public MonsterAlertState(UnitStateMachine stateMachine)
		: base(null)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
	}

	public override void OnUpdate()
	{
	}

	public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
	{
		return EActionState.None;
	}

	public override Vector3 GetForward()
	{
		return default;
	}
}
