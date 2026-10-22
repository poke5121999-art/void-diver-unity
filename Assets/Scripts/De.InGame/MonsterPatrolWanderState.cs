using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class MonsterPatrolWanderState : MonsterStateBase
{
	private bool _isMoveCompleted;

	private int _wanderPositionIndex;

	private Vector3[] _destinations;

	private int _destinationIndex;

	private Vector3 _currentPosition;

	private float _originAgentRadius;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	private Vector3[] PatrolWanderPositions => null;

	private Vector3 Destination => default;

	public MonsterPatrolWanderState(UnitStateMachine stateMachine)
		: base(null)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
	}

	public override void OnExit()
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
