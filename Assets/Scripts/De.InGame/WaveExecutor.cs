using GameCommon.Data;
using GameCommon.Table;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class WaveExecutor : NetworkBase, IInteractiveObject
{
	public class Args
	{
		public int WaveExecutorId;

		public bool ExecuteImmediately;
	}

	[SerializeField]
	private bool _canTriggered;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameWaveManager _waveManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	private Args _args;

	private readonly NetworkVariable<int> _syncWaveExecutorId = new NetworkVariable<int>();

	private readonly NetworkVariable<bool> _syncIsClearedWave = new NetworkVariable<bool>();

	private CompositeDisposable _disposable;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => TConst.WaveExecutorInteractionTime.FloatValue;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx => _holdingSfx;

	public string InteractionSfx => _interactionSfx;

	public int WaveId { get; private set; }

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("WaveExecutor.Init() should be called before spawning the object.");
			return;
		}
		_args = args;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncWaveExecutorId.Value = _args.WaveExecutorId;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		WaveId = TWaveExecutor.Table[_syncWaveExecutorId.Value].WaveId;
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
		if (base.IsHost && _args.ExecuteImmediately)
		{
			ExecuteWave();
		}
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
		if (!_canTriggered)
		{
			return false;
		}
		if (_waveManager.IsWaveInProgress(base.NetworkObjectId))
		{
			return false;
		}
		return !_syncIsClearedWave.Value;
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

	public void ExecuteWave()
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_waveManager.StartWave(base.NetworkObjectId, WaveId, base.transform.position).OnWaveFinished.Subscribe(OnWaveFinished).AddTo(_disposable);
	}

	private void OnWaveFinished(bool isClear)
	{
		AssertIfNotHost();
		_syncIsClearedWave.Value = isClear;
		if (isClear)
		{
			GoodsData clearReward = TWaveExecutor.Table[_syncWaveExecutorId.Value].ClearReward;
			if (!clearReward.IsEmpty)
			{
				_objectManager.SpawnDropGoods(clearReward.ToGoodsAsBase(), base.transform.position + new Vector3(1f, 0f, 1f));
			}
		}
	}



}
