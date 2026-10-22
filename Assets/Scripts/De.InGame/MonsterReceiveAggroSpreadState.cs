using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class MonsterReceiveAggroSpreadState : MonsterStateBase
{
	private bool _isTimeCompleted;

	private float _remainTime;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	public MonsterReceiveAggroSpreadState(UnitStateMachine stateMachine)
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
