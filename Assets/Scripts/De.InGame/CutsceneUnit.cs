using GameCommon.Enum;
using Spine.Unity;
using UnityEngine;
using VContainer;

public class CutsceneUnit : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _gameObjectManager;

	[Inject]
	private GameContext _gameContext;

	[SerializeField]
	private int _targetClientIndex;

	[SerializeField]
	private bool _useClientCharacter;

	[SerializeField]
	private SkeletonAnimation[] _skeletonAnimations;

	private int _currentSkeletonIndex;

	public SkeletonAnimation CurrentSkeletonAnimation => null;

	protected override void Awake()
	{
	}

	public void UpdateSkeletonData(SkeletonAnimation destination, SkeletonAnimation source)
	{
	}

	public void SetDirection(ECutsceneUnitDirection direction)
	{
	}

	private void UpdateSkeletonIndex(int index)
	{
	}

	public void SetAnimationSpeed(float speed)
	{
	}

	public void PlayAnimation(string animationName, bool isLoop)
	{
	}

	private void LateUpdate()
	{
	}
}
