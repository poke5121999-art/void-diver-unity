using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPointWanderState : MonsterStateBase
{
	private static readonly int WalkableAreaMask;

	private float _stateEndTime;

	private bool _isMoveCompleted;

	private NavMeshPath _navMeshPath;

	private int _currentCornerIndex;

	private Vector3 _prevDirection;

	private bool _pathInitialized;

	public override EActionState ActionState => EActionState.None;

	public override bool IsAimEnable => false;

	private bool HasPath => false;

	public MonsterPointWanderState(UnitStateMachine stateMachine)
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

	private void UpdateMove()
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

	private void InitializeSpecificPath(Vector3 position)
	{
	}

	private void InitializeRandomPath()
	{
	}
}
