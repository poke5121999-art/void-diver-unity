using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class MonsterAggroState : MonsterStateBase
{
	private float _stateStartTime;

	private float _lastAggroSfxTime;

	private bool _shouldPlayAggroSfx;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	private float StateTime => 0f;

	private float StateElapsedTime => 0f;

	public MonsterAggroState(UnitStateMachine stateMachine)
		: base(null)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
	}

	public override void OnUpdate()
	{
	}

	public override void OnExit()
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
