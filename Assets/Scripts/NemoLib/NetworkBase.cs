using UniRx;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;

public abstract class NetworkBase : NetworkBehaviour
{
	[Inject]
	protected INetworkHost NetworkHost;

	[Inject]
	protected INetworkChannel NetworkChannel;

	private NetworkObject _networkObject;

	private bool _isMainComponent;

	protected CompositeDisposable Disposable { get; private set; }

	protected virtual void Awake()
	{
		this.InjectFromRootContainer();
		_networkObject = GetComponent<NetworkObject>();
		// The binary's Awake ends by storing a new CompositeDisposable at the backing field get_Disposable reads (+0xb8,
		// 0x182d129b8); Ghidra's pseudo-C dropped it, and SpecialField.OnHostSpawn's AddTo(Disposable) threw on null.
		Disposable = new CompositeDisposable();
	}

	public virtual void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
		if (IsHost)
		{
			OnHostSpawn();
		}
		if (IsHost && _isMainComponent)
		{
			NetworkHost.OnJoinCompleted.Subscribe(_networkObject.NetworkShow).AddTo(this);
		}
		if (!(NetworkManager == null) && NetworkManager.NetworkTickSystem != null)
		{
			NetworkManager.NetworkTickSystem.Tick += OnNetworkUpdate;
		}
	}

	public override void OnNetworkDespawn()
	{
		if (!(NetworkManager == null) && NetworkManager.NetworkTickSystem != null)
		{
			NetworkManager.NetworkTickSystem.Tick -= OnNetworkUpdate;
		}
	}

	protected virtual void OnNetworkUpdate()
	{
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		Disposable?.Dispose();
	}

	protected void AssertIfNotHost()
	{
		if (!NetworkManager.IsHost)
		{
			throw new System.InvalidOperationException("NetworkManager is not running as Host.");
		}
	}

	public void Spawn(bool destroyWithScene = false)
	{
		AssertIfNotHost();
		_isMainComponent = true;
		_networkObject.SpawnWithObservers = false;
		_networkObject.Spawn(destroyWithScene);
		foreach (ulong joinedClientId in NetworkHost.JoinedClientIds)
		{
			_networkObject.NetworkShow(joinedClientId);
		}
	}

	public void Despawn(float delay = 0f, bool destroy = true)
	{
		if (delay <= 0f)
		{
			TryDespawn(destroy);
			return;
		}
		Observable.Timer(System.TimeSpan.FromSeconds(delay)).Subscribe(delegate
		{
			TryDespawn(destroy);
		}).AddTo(Disposable);
	}

	private void TryDespawn(bool destroy)
	{
		if (!(_networkObject == null) && _networkObject.IsSpawned)
		{
			NetworkManager networkManager = _networkObject.NetworkManager;
			if (!(networkManager == null) && networkManager.IsListening && networkManager.SpawnManager != null)
			{
				_networkObject.Despawn(destroy);
			}
		}
	}
}
