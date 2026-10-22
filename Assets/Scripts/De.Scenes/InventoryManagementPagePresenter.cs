using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.InputSystem;
using VContainer;

public class InventoryManagementPagePresenter : BasePanelPresenter
{
	public enum ELayout
	{
		None = 0,
		InventoryOnly = 1,
		InventoryLooting = 2,
		InventoryStash = 3
	}

	[InjectModel]
	private InventoryManagementPageModel _model;

	[View]
	private InventoryManagementPageView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	private PlayerInventoryPanelModel _myInventoryPanelModel;

	private LootingInventoryPanelModel _lootingInventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private QuickSlotSettingPanelModel _quickSlotPanelModel;

	private BagPanelModel _bagPanelModel;

	private EGoodsSlotCategory _bagHolderCategory;

	private int _bagHolderSlotIndex;

	private ulong _bagHolderNetworkObjectId;

	private bool _isDragging;

	private bool _isPadDragging;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private InventoryGoodsSlotModel _targetSlotModel;

	private DraggingGoodsSlotModel _draggingSlotModel;

	private CompositeDisposable _disposable;

	private CompositeDisposable _bagHolderSubscription;

	private EGoodsType[] _cachedStashFilter;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiKeyboardActions UiKeyboardActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private PlayerInventory PlayerInventory => null;

	private LootingInventory LootingInventory => null;

	private GamePlayer Player => null;

	private int SourceSlotIndex => 0;

	private int TargetSlotIndex => 0;

	private bool IsInventorySelectedOnePressed => false;

	private bool IsInventorySelectHalfPressed => false;

	private bool IsBagPanelOpen => false;

	private void OnInject(InventoryManagementPageModel model)
	{
	}

	private void OnDisable()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void Update()
	{
	}

	private void Close()
	{
	}

	private void InitBagPanel()
	{
	}

	private void FocusBag(ENavDir dir)
	{
	}

	public void OpenBagPanel(BagGoods bag, EGoodsSlotCategory holderCategory, int holderSlotIndex, ulong holderNetworkObjectId)
	{
	}

	public void CloseBagPanel()
	{
	}

	private void RestoreFocusAfterBagClose()
	{
	}

	private void SubscribeBagHolderChanges()
	{
	}

	private void UnsubscribeBagHolderChanges()
	{
	}

	private void RefreshBagPanelFromHolder()
	{
	}

	private IDictionary<int, GoodsSlot> GetBagHolderSlotsForRefresh()
	{
		return null;
	}

	private void InitGamePadNav()
	{
	}

	private void FocusMyInventory(ENavDir dir, PlayerInventoryPanelPresenter.EFocusPanel focusPanel)
	{
	}

	private void FocusLooting(ENavDir dir)
	{
	}

	private void FocusStash(ENavDir dir, StashPanelPresenter.EFocusPanel focusPanel)
	{
	}

	private void FocusQuickSlot(ENavDir dir)
	{
	}

	private void DisableAllPanelInput()
	{
	}

	private void OnSlotFocused(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	private bool CanPushDraggingGoodsToOpenedBag(GoodsSlot sourceSlot)
	{
		return false;
	}

	private void SwapInventorySlots()
	{
	}

	private void TransferInventorySlotToSafeSlot()
	{
	}

	private void TransferInventorySlotToLootingSlot(int targetIndex, long count)
	{
	}

	private void SwapLootingSlots()
	{
	}

	private void TransferLootingSlotToInventorySlot(int targetSlotIndex, long count)
	{
	}

	private void TransferLootingSlotToSafeSlot(int targetSlotIndex, long count)
	{
	}

	private void MoveAllToStash()
	{
	}

	private void TransferInventorySlotToStashSlot(int targetIndex, long count)
	{
	}

	private void SwapSafeSlots()
	{
	}

	private void TransferSafeSlotToInventorySlot()
	{
	}

	private void TransferSafeSlotToLootingSlot(int targetIndex, long count)
	{
	}

	private void TransferSafeSlotToStashSlot(int targetIndex, long count)
	{
	}

	private void SwapStashSlots()
	{
	}

	private void TransferStashSlotToInventorySlot(int targetIndex, long count)
	{
	}

	private void TransferStashSlotToSafeSlot(int targetIndex, long count)
	{
	}

	private void SetQuickSlot(int quickSlotIndex)
	{
	}

	private void SwapQuickSlot(int targetSlotIndex)
	{
	}

	private void DropInventoryGoods(long count)
	{
	}

	private void DropSafeGoods(long count)
	{
	}

	private bool CheckEquipable(GoodsSlot slot, EGoodsSlotCategory targetCategory, out string failMessage)
	{
		failMessage = null;
		return false;
	}

	private bool HasDuplicateEquipPartGroup(Character character, int equipPartGroup, EGoodsSlotCategory replaceCategory, int replaceSlotIndex)
	{
		return false;
	}

	private void EquipInventorySlot()
	{
	}

	private void EquipSafeSlot()
	{
	}

	private void EquipLootingSlot()
	{
	}

	private void EquipStashSlot()
	{
	}

	private void UnequipToInventorySlot()
	{
	}

	private void UnequipToSafeSlot()
	{
	}

	private void UnequipToLootingSlot()
	{
	}

	private void UnequipToStashSlot()
	{
	}

	private void UseBag()
	{
	}

	private long GetExistingStackInBag(BagGoods bag, int goodsId)
	{
		return 0L;
	}

	private void TransferBagSlotToInventory(InventoryGoodsSlotModel bagSlotModel)
	{
	}

	private void TransferBagSlotToAnyTarget(InventoryGoodsSlotModel bagSlotModel, InventoryGoodsSlotModel targetSlotModel, long count)
	{
	}

	private long? ResolveAddableForTarget(EGoodsSlotCategory targetCategory, int itemId)
	{
		return null;
	}

	private void TransferStashBagToStashSlot(GoodsSlot bagSlot, GoodsSlot stashTargetSlot, long amountToPull)
	{
	}

	private void TransferBagInnerSwap(GoodsSlot fromSlot, GoodsSlot toSlot)
	{
	}

	private void TransferToBagSlot(long count, int innerSlotIndex = -1)
	{
	}

	private void TransferInventorySlotToBagSlot(long count, int innerSlotIndex = -1)
	{
	}

	private void TransferSafeSlotToBagSlot(long count, int innerSlotIndex = -1)
	{
	}

	private void TransferLootingSlotToBagSlot(long count, int innerSlotIndex = -1)
	{
	}

	private void TransferEquipmentSlotToBagSlot(int innerSlotIndex = -1)
	{
	}

	private void TransferStashSlotToBagSlot(long count, int innerSlotIndex = -1)
	{
	}

	private void UseItem()
	{
	}

	private void PlayEquipSound(EGoodsSlotCategory subType)
	{
	}

	private void HandleInteraction()
	{
	}

	private void HandleBagSlotToAnyTarget()
	{
	}

	private void HandleTransferSlots()
	{
	}

	private void HandleEquipSlot()
	{
	}

	private void HandleUnequipSlot()
	{
	}

	private void HandleTransferToBagSlot()
	{
	}

	private void HandleSwapEquipmentSlots()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnOrganizeButtonClick(InputAction.CallbackContext context)
	{
	}

	private void OnUseButtonClick(InputAction.CallbackContext context)
	{
	}

	private void OnSetQuickSlotByInput(InputAction.CallbackContext context)
	{
	}

	private void OnEscapeAction(InputAction.CallbackContext context)
	{
	}

	private void OnSwapWeaponButtonClick(InputAction.CallbackContext context)
	{
	}

	private void OnInsertGoods(InputAction.CallbackContext context)
	{
	}

	private void OnPadDragAndDrop(InputAction.CallbackContext context)
	{
	}

	private void OnMoveToStashAll(InputAction.CallbackContext context)
	{
	}

	private void OnDropGoods(InputAction.CallbackContext context)
	{
	}

	private void InitLootingInventory()
	{
	}

	private void OnLootingSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnLootingSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void InitMyInventoryPanel()
	{
	}

	private void OnInventorySlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnInventorySlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSafeInventorySlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSafeInventorySlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnEquipmentSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnEquipmentSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void InitQuickSlotPanel()
	{
	}

	private void OnQuickSlotSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnQuickSlotSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void InitStash()
	{
	}

	private void OnStashSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnStashSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnStashFilterChanged(EGoodsType[] allowedTypes)
	{
	}
}
