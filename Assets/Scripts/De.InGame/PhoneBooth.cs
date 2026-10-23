using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using VContainer;

public class PhoneBooth : NetworkBase, IInteractiveObject
{
	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameManager _gameManager;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => TConst.DungeonEntranceInteractionTime.FloatValue;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx
	{
		get
		{
			if (!string.IsNullOrEmpty(_holdingSfx))
			{
				return _holdingSfx;
			}
			return "InteractionDoor";
		}
	}

	public string InteractionSfx
	{
		get
		{
			if (!string.IsNullOrEmpty(_interactionSfx))
			{
				return _interactionSfx;
			}
			return "EnterPhoneBooth";
		}
	}

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	public ECanEnterPhoneBoothResult CheckCanEnter(ulong clientId)
	{
		if (_roomContext.RoomState != ERoomState.Playing)
		{
			return ECanEnterPhoneBoothResult.InvalidRoomState;
		}
		if (_campaignManager.GetPlayerCampaignState(clientId) != ECampaignState.InProgress)
		{
			return ECanEnterPhoneBoothResult.InvalidQuestState;
		}
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (characterModel == null)
		{
			return ECanEnterPhoneBoothResult.NoWeapon;
		}
		if (characterModel.WeaponId <= 0)
		{
			return ECanEnterPhoneBoothResult.NoWeapon;
		}
		return ECanEnterPhoneBoothResult.Success;
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return true;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
	}



}
