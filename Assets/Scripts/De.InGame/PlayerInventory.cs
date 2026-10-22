using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using Tracker;
using UniRx;

public class PlayerInventory : Inventory
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnStageEndedAsync_003Ed__42 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public PlayerInventory _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	private CharacterModel _characterModel;

	private Goods[] _initHighGradeGoodsCache;

	private bool _isFirstSyncReceived;

	private readonly TrackableDictionary<int, GoodsSlot> _trackableSafeSlots;

	private readonly Dictionary<int, GoodsSlot> _safeSlots;

	public readonly Subject<IList<GoodsSlotChangeData>> OnSafeInventoryChanged;

	private int _syncSafeSlotsTick;

	private ulong ClientId => 0uL;

	public bool IsMine => false;

	public GamePlayer Player => null;

	public override int SlotCount => 0;

	private IDictionary<int, GoodsSlot> PlayerSlots => null;

	public int SafeSlotCount => 0;

	public IDictionary<int, GoodsSlot> SafeSlots => null;

	private bool HasSafeSlotsChange => false;

	private IDictionary<int, GoodsSlot> PlayerSafeSlots => null;

	protected override void Awake()
	{
	}

	public override void Init(ICollection<GoodsSlot> goodsSlots, int slotCount)
	{
	}

	public override void Init(ICollection<GoodsSlot> slots, ICollection<GoodsSlot> subSlots)
	{
	}

	protected override void Synchronize()
	{
	}

	public override long GetItemCount(int itemId)
	{
		return 0L;
	}

	public override long GetEquipmentCount(int equipmentId)
	{
		return 0L;
	}

	public override long GetGoodsCount(EGoodsType goodsType, int goodsId)
	{
		return 0L;
	}

	public long GetBagCount(int bagId)
	{
		return 0L;
	}

	public long GetEmptyBagCount(int bagId)
	{
		return 0L;
	}

	public bool HasDuplicateBagType(EBagType type, EGoodsSlotCategory excludeSourceCategory, int excludeSourceIndex, EGoodsSlotCategory excludeDestCategory, int excludeDestIndex)
	{
		return false;
	}

	public override bool CanMoveGoods(Goods goods)
	{
		return false;
	}

	public bool CanAddGoods(Goods goods)
	{
		return false;
	}

	public bool CanAddGoods(Goods goods, out string reason)
	{
		reason = null;
		return false;
	}

	private void Start()
	{
	}

	protected override void OnResSyncInventory(ResSyncInventory message)
	{
	}

	public override void PushGoods(Goods goods, int slotIndex)
	{
	}

	private Goods StackIntoExistingBags(Goods goods)
	{
		return null;
	}

	public override bool TryPushGoods(Goods goods, out Goods remainGoods, int slotIndex = -1)
	{
		remainGoods = null;
		return false;
	}

	public override bool TryPullGoods(Goods goods, out Goods[] pullGoods)
	{
		pullGoods = null;
		return false;
	}

	public bool TryPullSlotGoods(EGoodsSlotCategory slotCategory, int slotIndex, long count, out Goods pullGoods)
	{
		pullGoods = null;
		return false;
	}

	public GoodsSlot GetGoodsSlot(EGoodsSlotCategory slotCategory, int slotIndex)
	{
		return null;
	}

	public bool TryConsumeGoods(GoodsData goodsData, out GoodsData remainGoodsData)
	{
		remainGoodsData = default;
		return false;
	}

	public bool TryConsumeSlotGoods(EGoodsSlotCategory slotCategory, int slotIndex, Goods consumeGoods)
	{
		return false;
	}

	private void OnSlotsChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	public long GetAddableItemCount(int itemId)
	{
		return 0L;
	}

	public long GetAddableCountInBagStacks(int itemId)
	{
		return 0L;
	}

	public long GetAddableItemCountWithBags(int itemId)
	{
		return 0L;
	}

	public long GetAddableInventoryItemCount(int itemId)
	{
		return 0L;
	}

	private int GetInventoryItemCountMax(int itemId)
	{
		return 0;
	}

	[AsyncStateMachine(typeof(_003COnStageEndedAsync_003Ed__42))]
	private UniTask OnStageEndedAsync()
	{
		return default;
	}

	private void InitSafeSlots(ICollection<GoodsSlot> goodsSlots)
	{
	}

	private void InitSafeInventoryEvents()
	{
	}

	private GoodsSlot[] FlushSafeSlots()
	{
		return null;
	}

	public bool IsSafeSlotFull()
	{
		return false;
	}

	public bool CanAddGoodsToSafeSlot(Goods goods)
	{
		return false;
	}

	private void ApplySafeSlots(GoodsSlot[] slots)
	{
	}

	private void OnSafeSlotsChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	public void PushGoodsToSafeSlot(Goods goods, int slotIndex)
	{
	}

	private void OnSyncSafeInventory(ReqSyncSafeInventory message)
	{
	}

	private void OnUpdateSafeInventory(NtfUpdateSafeInventory message)
	{
	}

	private void OnResSyncSafeInventory(ResSyncSafeInventory message)
	{
	}
}
