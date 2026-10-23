using GameCommon.Enum;
using NemoLib.Log;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class InteractiveTrigger : NetworkBase, IInteractiveObject, ITriggerable
{
	public class Args
	{
		public int TriggerId;
	}

	[SerializeField]
	private int _triggerId;

	[SerializeField]
	private float _coolTime;

	[SerializeField]
	[Range(0f, 5f)]
	private float _holdingTime = 0.5f;

	[SerializeField]
	private bool _isIndividual;

	[SerializeField]
	private bool _canTriggered;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	private Args _args;

	private float _nextInteractableTime;

	private readonly NetworkVariable<int> _syncTriggerId = new NetworkVariable<int>();

	public int TriggerId => _triggerId;

	public bool IsIndividual => _isIndividual;

	public bool CanTriggered => _canTriggered;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => _holdingTime;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx => _holdingSfx;

	public string InteractionSfx => _interactionSfx;

	private float TimeNow => _gameManager.TimeNow;

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("InteractiveTrigger.Init() should be called before spawning the object.");
		}
		else
		{
			_args = args;
		}
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncTriggerId.Value = ((_args != null && _args.TriggerId >= 1) ? _args.TriggerId : _triggerId);
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_triggerId = _syncTriggerId.Value;
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	public void SetTriggerable(bool canTriggered)
	{
		_canTriggered = canTriggered;
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		if (_canTriggered)
		{
			return _nextInteractableTime <= _gameManager.TimeNow;
		}
		return false;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
		if (_isIndividual && _gameManager.MyCharacterModel.ClientId != sourceClientId)
		{
			return;
		}
		if (_coolTime < 0f)
		{
			_canTriggered = false;
		}
		else if (_coolTime > 0f)
		{
			_nextInteractableTime = _gameManager.TimeNow + _coolTime;
		}
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
	}
}
