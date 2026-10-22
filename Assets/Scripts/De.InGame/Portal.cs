using GameCommon.Enum;
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

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public Vector3 ExitPosition { get; private set; }

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	public void Init(Args args)
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
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



}
