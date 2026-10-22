using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
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

	public readonly Subject<Unit> OnUnlockComplete = new Subject<Unit>();

	private readonly NetworkVariable<int> _syncExitId = new NetworkVariable<int>();

	protected GameHost GameHost => NetworkHost as GameHost;

	public int ExitId => _syncExitId.Value;

	public abstract float HoldingTime { get; }

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public EOutlineType OutlineType => EOutlineType.Default;

	public abstract EExitState ExitState { get; }

	public ReactiveProperty<EExitState> RxExitState { get; } = new ReactiveProperty<EExitState>();

	public string HoldingSfx
	{
		get
		{
			if (ExitState == EExitState.Deactivated)
			{
				return TExit.Table[_syncExitId.Value].ActivatingStateHoldingSfx;
			}
			return TExit.Table[_syncExitId.Value].HoldingSfx;
		}
	}

	public virtual string InteractionSfx => TExit.Table[_syncExitId.Value].EscapeSfx;

	ulong IInteractiveObject.NetworkObjectId => NetworkObjectId;

	public virtual void Init(Args args)
	{
		AssertIfNotHost();
		_args = args;
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncExitId.Value = _args.ExitId;
	}

	public abstract void OnInteraction(UnitController interactingUnit);

	protected bool TryConsumeUnlockGoods(UnitController interactingUnit)
	{
		TExit tExit = TExit.Table[_syncExitId.Value];
		int level = GameContext.Players[interactingUnit.ClientId].Profile.Level;
		GoodsData unlockGoods = tExit.GetUnlockGoodsData(level);
		if (unlockGoods.IsEmpty)
		{
			return true;
		}
		Inventory inventory = interactingUnit.GetComponent<Inventory>();
		if (inventory != null)
		{
			long num = ((unlockGoods.Category == EGoodsCategory.Item) ? inventory.GetItemCount(unlockGoods.Id) : ((unlockGoods.Category == EGoodsCategory.Equipment) ? inventory.GetEquipmentCount(unlockGoods.Id) : 0));
			if (unlockGoods.Count <= num)
			{
				inventory.TryPullGoods(unlockGoods.ToGoodsAsBase(), out var _);
				OnUnlockComplete.OnNext(Unit.Default);
				return true;
			}
			GameHost.SendNoticeToast(TLocalizedText.NotEnoughGoodsToUnlockExit.Id, interactingUnit.ClientId);
		}
		return false;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
	}

	public abstract bool CanInteract(UnitModel sourceModel);

	public abstract void OnStartHolding(ulong sourceClientId);

	public abstract void OnStartInteraction(ulong sourceClientId);

	public abstract void OnEndInteraction(ulong sourceClientId);



}
