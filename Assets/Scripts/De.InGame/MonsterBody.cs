using GameCommon.Enum;
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

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public bool IsInteracting { get; private set; }

	public LootingInventory[] LootingInventories { get; private set; }

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public LootingInventory FindInventory(ulong sourceClientId)
	{
		return null;
	}

	public bool IsAllRevealed()
	{
		return false;
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return false;
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
