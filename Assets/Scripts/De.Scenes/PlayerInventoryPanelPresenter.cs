using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class PlayerInventoryPanelPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Equipment = 1,
		Scroller = 2,
		Empty = 3,
		Safe = 4
	}

	[InjectModel]
	private PlayerInventoryPanelModel _model;

	[View]
	private PlayerInventoryPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	private bool _isSafeInventoryUnlocked;

	private InventoryGoodsSlotModel _cachedSelectedSlot;

	private PlayerInventoryScrollerModel _scrollerModel;

	private EquipmentInventoryPanelModel _equipmentModel;

	private EmptyPanelModel _emptyPanelModel;

	private SafeInventoryPanelModel _safeModel;

	private CompositeDisposable _disposable;

	private CompositeDisposable _corruptionDisposable;

	protected override BasePanelModel PanelModel => null;

	private PlayerInventory Inventory => null;

	private int SlotCount => 0;

	private EFocusPanel FocusedPanel => EFocusPanel.None;

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
	}

	protected void OnInject(PlayerInventoryPanelModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void InitEquipmentInventoryPanel(PlayerInventoryPanelModel model)
	{
	}

	private void InitSafeInventoryPanel(PlayerInventoryPanelModel model)
	{
	}

	private bool CheckSafeInventoryUnlocked()
	{
		return false;
	}

	private void SetFocusedSlot(InventoryGoodsSlotModel slotModel)
	{
	}

	private void SetFocusedEquipmentSlot(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSafeSlotFocused(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnPageChanged(int pageIndex)
	{
	}

	private void OnPageCountChanged(int pageCount)
	{
	}

	private void UpdatePagination()
	{
	}

	private void UpdateSlotCounts()
	{
	}

	private void InitEmptyPanel()
	{
	}

	private void InitInventoryScroller()
	{
	}

	private void OnPrevPageButtonClick()
	{
	}

	private void OnNextPageButtonClick()
	{
	}

	private void OnFocusedSlotChanged(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnSlotsEmpty(bool isEmpty)
	{
	}

	private void BindCorruption(CharacterModel characterModel)
	{
	}

	private void OnCorruptionChanged()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void FocusScrollerOrEmpty(ENavDir dir)
	{
	}

	private void FocusPanel(EFocusPanel panel, ENavDir dir)
	{
	}
}
