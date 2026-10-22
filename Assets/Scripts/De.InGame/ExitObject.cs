using GameCommon.Enum;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public abstract class ExitObject : NetworkBase, IInteractiveObject
{
	public class Args
	{
		public int ExitId;
	}

	[SerializeField]
	protected GameObject GoDeactivated;

	[SerializeField]
	protected GameObject GoActivating;

	[SerializeField]
	protected GameObject GoActivated;

	[SerializeField]
	protected GameObject GoFinished;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected SoundManager SoundManager;

	[Inject]
	protected GameWaveManager WaveManager;

	[Inject]
	protected GameContext GameContext;

	private Args _args;

	public readonly Subject<Unit> OnUnlockComplete;

	private readonly NetworkVariable<int> _syncExitId;

	protected GameHost GameHost => null;

	public int ExitId => 0;

	public abstract float HoldingTime { get; }

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public EOutlineType OutlineType => EOutlineType.None;

	public abstract EExitState ExitState { get; }

	public ReactiveProperty<EExitState> RxExitState { get; }

	public string HoldingSfx => null;

	public virtual string InteractionSfx => null;

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	public virtual void Init(Args args)
	{
	}

	public override void OnHostSpawn()
	{
	}

	public abstract void OnInteraction(UnitController interactingUnit);

	protected bool TryConsumeUnlockGoods(UnitController interactingUnit)
	{
		return false;
	}

	public override void OnNetworkSpawn()
	{
	}

	public abstract bool CanInteract(UnitModel sourceModel);

	public abstract void OnStartHolding(ulong sourceClientId);

	public abstract void OnStartInteraction(ulong sourceClientId);

	public abstract void OnEndInteraction(ulong sourceClientId);

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
