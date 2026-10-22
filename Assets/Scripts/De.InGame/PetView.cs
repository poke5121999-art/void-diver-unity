using GameCommon.Enum;
using NemoLib.Log;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

public class PetView : MonoBase
{
	[SerializeField]
	private NavMeshAgent _agent;

	[SerializeField]
	private SkeletonAnimation[] _skeletonAnimations;

	[SerializeField]
	private PetModel _petModel;

	[SerializeField]
	private GameObject _visualGo;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	private const float _navMeshSampleRadius = 1.8f;

	private const float _repathThreshold = 0.3f;

	private int _currentSkeletonIndex;

	private Vector3 _lastDestination;

	private bool IsHost => false;

	private Vector3 Forward => default;

	private bool IsPlaying => false;

	private SkeletonAnimation CurrentSkeletonAnimation => null;

	public void Init()
	{
	}

	private void OnStateChanged(EPetState state)
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
	}

	private void LateUpdate()
	{
	}

	private void OnStageStateChanged(EStageState stageState)
	{
	}

	private void SyncPositionOnClient()
	{
	}

	public void UpdateAgentPath(Vector3 destination, float speed)
	{
	}

	private bool TryRecoverOntoNavMesh(Vector3 walkableTarget)
	{
		return false;
	}

	private void StopFollow()
	{
	}

	public void Teleport(Vector3 position)
	{
	}

	private void PlayIdle()
	{
	}

	private void PlayWalk(float speed)
	{
	}

	private void PlayAlert()
	{
	}

	private void PlayRun(float speed)
	{
	}

	private void LookAtDirection()
	{
	}

	private void UpdateSkeletonIndex(int index)
	{
	}

	private void PlayAnimation(string animationName, float speed, bool loop = true)
	{
	}
}
