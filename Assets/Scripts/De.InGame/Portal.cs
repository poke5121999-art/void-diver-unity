using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using VContainer;

public class Portal : NetworkBase, IInteractiveObject
{
	public class Args
	{
		public Vector3 ExitPosition;
	}

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => TConst.PortalInteractionTime.FloatValue;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx => string.IsNullOrEmpty(_holdingSfx) ? "FootStep" : _holdingSfx;

	public string InteractionSfx => string.IsNullOrEmpty(_interactionSfx) ? "Portal_Move" : _interactionSfx;

	public Vector3 ExitPosition { get; private set; }

	ulong IInteractiveObject.NetworkObjectId => NetworkObjectId;

	public void Init(Args args)
	{
		ExitPosition = args.ExitPosition;
	}

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
