using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using Spine;
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

	private Vector3 _lastDestination = Vector3.positiveInfinity;

	private bool IsHost => _petModel.IsHost;

	private Vector3 Forward => _petModel.RxForward.Value;

	private bool IsPlaying
	{
		get
		{
			if (_gameManager == null)
			{
				return false;
			}
			return _gameManager.StageState == EStageState.Playing;
		}
	}

	private SkeletonAnimation CurrentSkeletonAnimation
	{
		get
		{
			if (_skeletonAnimations != null && _currentSkeletonIndex < _skeletonAnimations.Length)
			{
				return _skeletonAnimations[_currentSkeletonIndex];
			}
			return null;
		}
	}

	public void Init()
	{
		_agent.enabled = _petModel.IsHost;
		_gameManager.RxStageState.Subscribe(OnStageStateChanged).AddTo(this);
		_petModel.RxState.Subscribe(OnStateChanged).AddTo(this);
	}

	private void OnStateChanged(EPetState state)
	{
		switch (state)
		{
		case EPetState.Idle:
		case EPetState.Teleport:
			StopFollow();
			PlayIdle();
			break;
		case EPetState.Alert:
			StopFollow();
			PlayAlert();
			break;
		case EPetState.Walk:
		case EPetState.BackOff:
			PlayWalk(_petModel.Speed);
			break;
		case EPetState.Run:
			PlayRun(_petModel.Speed);
			break;
		case EPetState.WaitingToFollow:
			break;
		}
	}

	private void Update()
	{
		if (IsPlaying)
		{
			if (!_petModel.IsHost)
			{
				SyncPositionOnClient();
			}
			LookAtDirection();
		}
	}

	private void FixedUpdate()
	{
		if (_petModel.IsHost && IsPlaying && _agent.hasPath)
		{
			_petModel.Position = base.transform.position;
		}
	}

	private void LateUpdate()
	{
		if (IsPlaying && !(CurrentSkeletonAnimation == null))
		{
			CurrentSkeletonAnimation.Update(Time.deltaTime);
			CurrentSkeletonAnimation.LateUpdate();
		}
	}

	private void OnStageStateChanged(EStageState stageState)
	{
		if (IsPlaying)
		{
			_visualGo.SetActive(value: true);
			for (int i = 0; i < _skeletonAnimations.Length; i++)
			{
				_skeletonAnimations[i].gameObject.SetActive(i == _currentSkeletonIndex);
			}
			OnStateChanged(_petModel.RxState.Value);
		}
		else
		{
			_visualGo.SetActive(value: false);
		}
	}

	private void SyncPositionOnClient()
	{
		if (_petModel.Position == Vector3.zero || base.transform.position == _petModel.Position)
		{
			return;
		}
		if (_petModel.Speed <= 0f)
		{
			base.transform.position = _petModel.Position;
			return;
		}
		Vector3 vector = _petModel.Position - base.transform.position;
		Vector3 vector2 = vector.normalized * _petModel.Speed * Time.deltaTime;
		base.transform.position += ((vector.magnitude <= vector2.magnitude) ? vector : vector2);
	}

	public void UpdateAgentPath(Vector3 destination, float speed)
	{
		if (!IsHost)
		{
			return;
		}
		if (!_agent.isOnNavMesh && !TryRecoverOntoNavMesh(destination))
		{
			return;
		}
		_agent.speed = speed;
		if (!(Vector3.Distance(destination, _lastDestination) < 0.3f))
		{
			_agent.SetDestination(destination);
			_lastDestination = destination;
		}
	}

	private bool TryRecoverOntoNavMesh(Vector3 walkableTarget)
	{
		NavMeshHit hit;
		if (NavMesh.SamplePosition(base.transform.position, out hit, 1.8f, -1))
		{
			_agent.Warp(hit.position);
			if (_agent.isOnNavMesh)
			{
				return true;
			}
		}
		if (!NavMesh.SamplePosition(walkableTarget, out hit, 1.8f, -1))
		{
			return false;
		}
		_agent.Warp(hit.position);
		return _agent.isOnNavMesh;
	}

	private void StopFollow()
	{
		if (_petModel.IsHost)
		{
			if (_agent.hasPath)
			{
				_agent.ResetPath();
			}
			_lastDestination = Vector3.positiveInfinity;
		}
	}

	public void Teleport(Vector3 position)
	{
		if (_petModel.IsHost)
		{
			_agent.Warp(position);
			_petModel.Position = position;
			_lastDestination = Vector3.positiveInfinity;
		}
	}

	private void PlayIdle()
	{
		PlayAnimation("idle", 1f);
	}

	private void PlayWalk(float speed)
	{
		PlayAnimation("walk", speed / TConst.MoveSpeedThresholdForRun.FloatValue);
	}

	private void PlayAlert()
	{
		PlayAnimation("alert", 1f);
	}

	private void PlayRun(float speed)
	{
		float walkAnimation1xSpeed = TPet.Table[_petModel.RxPetId.Value].WalkAnimation1xSpeed;
		float floatValue = TConst.MinWalkAnimationSpeed.FloatValue;
		float floatValue2 = TConst.MaxWalkAnimationSpeed.FloatValue;
		PlayAnimation("run", Mathf.Clamp(speed / walkAnimation1xSpeed, floatValue, floatValue2));
	}

	private void LookAtDirection()
	{
		if (Forward == Vector3.zero)
		{
			return;
		}
		Vector2 vector = MathUtility.IsometricToTopDown(Forward);
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		if (num >= 0f && num < 90f)
		{
			UpdateSkeletonIndex(0);
			_skeletonAnimations[0].skeleton.ScaleX = -1f;
		}
		else if (num >= 90f && num < 180f)
		{
			UpdateSkeletonIndex(0);
			_skeletonAnimations[0].skeleton.ScaleX = 1f;
		}
		else if (num >= -180f && num < -90f)
		{
			UpdateSkeletonIndex(1);
			_skeletonAnimations[1].skeleton.ScaleX = 1f;
		}
		else if (num >= -90f && num < 0f)
		{
			UpdateSkeletonIndex(1);
			_skeletonAnimations[1].skeleton.ScaleX = -1f;
		}
	}

	private void UpdateSkeletonIndex(int index)
	{
		if (index == _currentSkeletonIndex)
		{
			return;
		}
		SkeletonAnimation skeletonAnimation = _skeletonAnimations[_currentSkeletonIndex];
		TrackEntry current = skeletonAnimation.state.GetCurrent(0);
		string text = null;
		float trackTime = 0f;
		bool loop = false;
		if (current != null)
		{
			text = current.Animation.Name;
			trackTime = current.TrackTime;
			loop = current.Loop;
		}
		float timeScale = skeletonAnimation.timeScale;
		skeletonAnimation.gameObject.SetActive(value: false);
		_currentSkeletonIndex = index;
		_skeletonAnimations[index].gameObject.SetActive(value: true);
		_skeletonAnimations[_currentSkeletonIndex].timeScale = timeScale;
		if (text != null)
		{
			TrackEntry trackEntry = _skeletonAnimations[_currentSkeletonIndex].state.SetAnimation(0, text, loop);
			trackEntry.TrackTime = trackTime;
			trackEntry.MixBlend = MixBlend.Setup;
		}
	}

	private void PlayAnimation(string animationName, float speed, bool loop = true)
	{
		LookAtDirection();
		if (!(CurrentSkeletonAnimation == null))
		{
			CurrentSkeletonAnimation.state.SetAnimation(0, animationName, loop);
			CurrentSkeletonAnimation.timeScale = speed;
		}
	}
}
