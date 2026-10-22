using UniRx;
using Unity.Netcode;
using VContainer;

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
	}

	public virtual void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	protected virtual void OnNetworkUpdate()
	{
	}

	public override void OnDestroy()
	{
	}

	protected void AssertIfNotHost()
	{
	}

	public void Spawn(bool destroyWithScene = false)
	{
	}

	public void Despawn(float delay = 0f, bool destroy = true)
	{
	}

	private void TryDespawn(bool destroy)
	{
	}



}
