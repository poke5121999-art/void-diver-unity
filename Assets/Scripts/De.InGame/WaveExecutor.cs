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

	private readonly NetworkVariable<int> _syncWaveExecutorId;

	private readonly NetworkVariable<bool> _syncIsClearedWave;

	private CompositeDisposable _disposable;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public int WaveId { get; private set; }

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	public void Init(Args args)
	{
	}

	public override void OnDestroy()
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

	public void SetTriggerable(bool canTriggered)
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

	public void ExecuteWave()
	{
	}

	private void OnWaveFinished(bool isClear)
	{
	}



}
