using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class MonsterCapturedState : MonsterStateBase
{
	private float _despawnTime;

	private bool _isDespawned;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	public MonsterCapturedState(UnitStateMachine stateMachine)
		: base(null)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
	}

	public override void OnUpdate()
	{
	}

	private void DropCapturedGoods()
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
