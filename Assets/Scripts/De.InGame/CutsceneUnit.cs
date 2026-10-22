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
	private int _targetClientIndex = -1;

	[SerializeField]
	private bool _useClientCharacter;

	[SerializeField]
	private SkeletonAnimation[] _skeletonAnimations;

	private int _currentSkeletonIndex;

	public SkeletonAnimation CurrentSkeletonAnimation
	{
		get
		{
			if (_currentSkeletonIndex >= _skeletonAnimations.Length)
			{
				return null;
			}
			return _skeletonAnimations[_currentSkeletonIndex];
		}
	}

	protected override void Awake()
	{
		base.Awake();
		ulong clientId = 0uL;
		if (_useClientCharacter)
		{
			clientId = _gameManager.LocalClientId;
		}
		else
		{
			if (_targetClientIndex < 0)
			{
				return;
			}
			int index = 0;
			foreach (ulong key in _gameContext.Players.Keys)
			{
				if (index == _targetClientIndex)
				{
					clientId = key;
					break;
				}
				index++;
			}
		}
		GamePlayer player = _gameContext.Players[clientId];
		CharacterController controller = _gameManager.FindCharacterController(clientId);
		SkeletonAnimation[] sources = controller.GetComponentsInChildren<SkeletonAnimation>(true);
		for (int i = 0; i < sources.Length && i < _skeletonAnimations.Length; i++)
		{
			int skinPresetId = player.Character.SkinPresetId;
			int weaponId = player.Character.WeaponId;
			UpdateSkeletonData(_skeletonAnimations[i], sources[i]);
			_skeletonAnimations[i].skeleton.UpdateCharacterSkin(skinPresetId, weaponId);
		}
	}

	public void UpdateSkeletonData(SkeletonAnimation destination, SkeletonAnimation source)
	{
		if (!(source == null) && !(destination == null))
		{
			destination.skeletonDataAsset = source.skeletonDataAsset;
			destination.Initialize(overwrite: true);
		}
	}

	public void SetDirection(ECutsceneUnitDirection direction)
	{
		switch (direction)
		{
		case ECutsceneUnitDirection.TopLeft:
			UpdateSkeletonIndex(0);
			_skeletonAnimations[0].skeleton.ScaleX = -1f;
			break;
		case ECutsceneUnitDirection.TopRight:
			UpdateSkeletonIndex(0);
			_skeletonAnimations[0].skeleton.ScaleX = 1f;
			break;
		case ECutsceneUnitDirection.BottomLeft:
			UpdateSkeletonIndex(1);
			_skeletonAnimations[1].skeleton.ScaleX = -1f;
			break;
		case ECutsceneUnitDirection.BottomRight:
			UpdateSkeletonIndex(1);
			_skeletonAnimations[1].skeleton.ScaleX = 1f;
			break;
		}
	}

	private void UpdateSkeletonIndex(int index)
	{
		if (index == _currentSkeletonIndex)
		{
			return;
		}
		SkeletonAnimation previous = _skeletonAnimations[_currentSkeletonIndex];
		previous.gameObject.SetActive(value: false);
		_currentSkeletonIndex = index;
		_skeletonAnimations[index].gameObject.SetActive(value: true);
		_skeletonAnimations[_currentSkeletonIndex].timeScale = previous.timeScale;
		Spine.TrackEntry current = previous.state.GetCurrent(0);
		if (current != null)
		{
			Spine.TrackEntry entry = _skeletonAnimations[_currentSkeletonIndex].state.SetAnimation(0, current.Animation.Name, current.Loop);
			entry.MixBlend = Spine.MixBlend.Setup;
			entry.TrackTime = current.TrackTime;
		}
	}

	public void SetAnimationSpeed(float speed)
	{
		if (!(CurrentSkeletonAnimation == null))
		{
			_skeletonAnimations[_currentSkeletonIndex].timeScale = speed;
		}
	}

	public void PlayAnimation(string animationName, bool isLoop)
	{
		if (CurrentSkeletonAnimation == null || _skeletonAnimations[_currentSkeletonIndex].state == null)
		{
			Debug.LogError("SkeletonAnimation or its state is not initialized!");
			return;
		}
		Spine.TrackEntry entry = _skeletonAnimations[_currentSkeletonIndex].state.SetAnimation(0, animationName, isLoop);
		entry.TrackTime = 0.2f;
	}

	private void LateUpdate()
	{
		if (!(CurrentSkeletonAnimation == null))
		{
			_skeletonAnimations[_currentSkeletonIndex].Update(Time.deltaTime);
			_skeletonAnimations[_currentSkeletonIndex].LateUpdate();
		}
	}
}
