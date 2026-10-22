using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class MonsterGroggyState : MonsterStateBase
{
	private float _groggyEndTime;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	public MonsterGroggyState(UnitStateMachine stateMachine)
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

	public override void OnExit()
	{
	}

	public override Vector3 GetForward()
	{
		return default;
	}
}
