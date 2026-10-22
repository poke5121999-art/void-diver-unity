using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class MonsterPhaseUpState : MonsterStateBase
{
	private float _phaseUpTime;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	public MonsterPhaseUpState(UnitStateMachine stateMachine)
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
