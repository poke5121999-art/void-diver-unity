using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

public class Entrance : NetworkBase, IInteractiveObject, IActionStats
{
	public class Args
	{
		public int EntranceId;

		public Vector3 Forward;
	}

	[SerializeField]
	private NavMeshObstacle _navMeshObstacle;

	[SerializeField]
	private BoxCollider _interactionCollider;

	[SerializeField]
	private BoxCollider _hitAbleCollider;

	[SerializeField]
	private EMeshType _meshType;

	[SerializeField]
	private GameObject _frontObject;

	[SerializeField]
	private GameObject _backObject;

	[SerializeField]
	private GameObject[] _openGameObjects;

	[SerializeField]
	private GameObject[] _closeGameObjects;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _lockedHoldingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	private Args _args;

	private readonly NetworkVariable<bool> _syncIsInteracting;

	private readonly NetworkVariable<int> _syncEntranceId;

	private readonly NetworkVariable<Vector3> _syncForward;

	private readonly NetworkVariable<ELockState> _syncLockState;

	protected GameHost GameHost => null;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public float HoldingTime => 0f;

	public ELockState LockState => ELockState.None;

	public Vector3 Forward => default;

	public int EntranceId => 0;

	public TEntrance TableData => null;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	public void Init(Args args)
	{
	}

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	private void OnLockStateChanged(ELockState oldValue, ELockState newValue)
	{
	}

	private void LookAtForward()
	{
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

	public void SetLockState(ELockState lockState)
	{
	}

	public float GetStat(EStatType statType)
	{
		return 0f;
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
