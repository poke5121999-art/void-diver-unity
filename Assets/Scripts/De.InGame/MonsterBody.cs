using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using VContainer;

public class MonsterBody : MonoBase, IInteractiveObject
{
	[SerializeField]
	private MonsterModel _monsterModel;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameManager _gameManager;

	public NetworkBase NetworkBase => _monsterModel;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => TConst.LootingInteractionTime.FloatValue;

	public EOutlineType OutlineType => TMonster.Table[_monsterModel.MonsterId].OutlineType;

	public string HoldingSfx => string.IsNullOrEmpty(_holdingSfx) ? "DoorInteractied2" : _holdingSfx;

	public string InteractionSfx => _interactionSfx;

	public bool IsInteracting { get; private set; }

	public LootingInventory[] LootingInventories { get; private set; }

	private void Start()
	{
		LootingInventories = GetComponents<LootingInventory>();
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
	}

	private void OnDestroy()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
	}

	public LootingInventory FindInventory(ulong sourceClientId)
	{
		if (LootingInventories == null)
		{
			return null;
		}
		return LootingInventories.FirstOrDefault((LootingInventory i) => !i.IsIndividual || i.IndividualClientId == sourceClientId);
	}

	public bool IsAllRevealed()
	{
		LootingInventory lootingInventory = FindInventory(_gameManager.MyCharacterController.ClientId);
		if (lootingInventory == null)
		{
			return false;
		}
		return lootingInventory.IsAllRevealed();
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		if (_monsterModel == null)
		{
			return false;
		}
		if (_monsterModel.State == EActionState.Dead && _monsterModel.NextPhaseMonsterId < 1 && !_monsterModel.IsLifeTimeFinished && sourceModel is CharacterModel characterModel)
		{
			LootingInventory lootingInventory = FindInventory(characterModel.ClientId);
			if (lootingInventory != null && !lootingInventory.IsEmpty)
			{
				return true;
			}
		}
		return false;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
		IsInteracting = true;
		if (_monsterModel.IsHost)
		{
			LootingInventory lootingInventory = FindInventory(sourceClientId);
			lootingInventory.StartLooting(_gameManager.FindCharacterModel(sourceClientId));
		}
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
		IsInteracting = false;
		if (_monsterModel.IsHost)
		{
			FindInventory(sourceClientId).EndLooting(sourceClientId);
		}
	}
}
