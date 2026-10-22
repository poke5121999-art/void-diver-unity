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

	private readonly NetworkVariable<bool> _syncIsInteracting = new NetworkVariable<bool>();

	private readonly NetworkVariable<int> _syncEntranceId = new NetworkVariable<int>();

	private readonly NetworkVariable<Vector3> _syncForward = new NetworkVariable<Vector3>();

	private readonly NetworkVariable<ELockState> _syncLockState = new NetworkVariable<ELockState>();

	protected GameHost GameHost => NetworkHost as GameHost;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx
	{
		get
		{
			if (_syncLockState.Value == ELockState.Locked)
			{
				if (!string.IsNullOrEmpty(_lockedHoldingSfx))
				{
					return _lockedHoldingSfx;
				}
				return "DoorKey";
			}
			if (!string.IsNullOrEmpty(_holdingSfx))
			{
				return _holdingSfx;
			}
			return "DoorInteractied2";
		}
	}

	public string InteractionSfx
	{
		get
		{
			if (_syncLockState.Value == ELockState.Locked)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(_interactionSfx))
			{
				return _interactionSfx;
			}
			return "DoorOpen";
		}
	}

	public float HoldingTime => ((_syncLockState.Value == ELockState.Opened) ? TConst.EntranceCloseInteractionTime : TConst.EntranceOpenInteractionTime).FloatValue;

	public ELockState LockState => _syncLockState.Value;

	public Vector3 Forward => _syncForward.Value;

	public int EntranceId => _syncEntranceId.Value;

	public TEntrance TableData => TEntrance.Table[_syncEntranceId.Value];

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	ulong IInteractiveObject.NetworkObjectId => NetworkObjectId;

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("RewardBox.Init() should be called before spawning the object.");
			return;
		}
		_args = args;
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncEntranceId.Value = _args.EntranceId;
		_syncForward.Value = _args.Forward.normalized;
		TEntrance tableData = TableData;
		_syncLockState.Value = (tableData.IsOpened ? ELockState.Opened : ((!TableData.HasKeyInteraction) ? ELockState.Closed : ELockState.Locked));
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
		_syncLockState.OnValueChanged += OnLockStateChanged;
		OnLockStateChanged(ELockState.None, _syncLockState.Value);
		LookAtForward();
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		_syncLockState.OnValueChanged -= OnLockStateChanged;
		base.OnNetworkDespawn();
	}

	private void OnLockStateChanged(ELockState oldValue, ELockState newValue)
	{
		bool isOpen = newValue == ELockState.Opened;
		bool isClosed = !isOpen;
		if (_openGameObjects != null)
		{
			foreach (GameObject go in _openGameObjects)
			{
				go.SetActive(isOpen);
			}
		}
		if (_closeGameObjects != null)
		{
			foreach (GameObject go2 in _closeGameObjects)
			{
				go2.SetActive(isClosed);
			}
		}
		_hitAbleCollider.enabled = isClosed;
		_navMeshObstacle.enabled = isClosed;
	}

	private void LookAtForward()
	{
		InGameUtility.LookAtForward(_meshType, _syncForward.Value, _frontObject, _backObject);
		Vector3 vector = MathUtility.SnapToCardinalDirection(_syncForward.Value);
		if (vector == Vector3.left || vector == Vector3.right)
		{
			Vector3 size = _interactionCollider.size;
			_interactionCollider.size = new Vector3(size.z, size.y, size.x);
			Vector3 size2 = _navMeshObstacle.size;
			_navMeshObstacle.size = new Vector3(size2.z, size2.y, size2.x);
			Vector3 size3 = _hitAbleCollider.bounds.size;
			_hitAbleCollider.size = new Vector3(size3.z, size3.y, size3.x);
		}
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		if (_syncIsInteracting.Value)
		{
			return false;
		}
		if (_syncLockState.Value == ELockState.PermanentlyLocked)
		{
			return false;
		}
		if (TableData.EntranceType != EEntranceType.Gate)
		{
			return true;
		}
		return _syncLockState.Value != ELockState.Opened;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
		if (!base.IsHost)
		{
			return;
		}
		_syncIsInteracting.Value = true;
		ELockState value = _syncLockState.Value;
		if (value == ELockState.Closed)
		{
			AssertIfNotHost();
			_syncLockState.Value = ELockState.Opened;
		}
		else if (value == ELockState.Opened)
		{
			AssertIfNotHost();
			_syncLockState.Value = ELockState.Closed;
		}
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
		if (base.IsHost)
		{
			_syncIsInteracting.Value = false;
		}
	}

	public void SetLockState(ELockState lockState)
	{
		AssertIfNotHost();
		_syncLockState.Value = lockState;
	}

	public float GetStat(EStatType statType)
	{
		if (statType != EStatType.Atk)
		{
			return 0f;
		}
		return TableData.HitBoxAtk;
	}
}
