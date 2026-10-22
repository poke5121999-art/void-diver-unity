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
	private float _holdingTime;

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

	private readonly NetworkVariable<int> _syncTriggerId;

	public int TriggerId => 0;

	public bool IsIndividual => false;

	public bool CanTriggered => false;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	private float TimeNow => 0f;

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



}
