using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class PlayerInventoryScrollerPresenter : GridScrollerPresenter<InventoryGoodsSlotModel, InventoryGoodsSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private PlayerInventoryScrollerModel _model;

	private List<InventoryGoodsSlotModel> _inventorySlotModels;

	private readonly List<InventoryGoodsSlotModel> _appendedSlotModels;

	private CompositeDisposable _disposable;

	private CompositeDisposable _appendedSlotDisposable;

	protected override BaseScrollerModel<InventoryGoodsSlotModel> ScrollerModel => null;

	private int NumberOfInventorySlots => 0;

	private int RowsPerPage => 0;

	private int SlotsPerRow => 0;

	private float PageChangeThreshold => 0f;

	private IDictionary<int, GoodsSlot> InventorySlots => null;

	private IDictionary<int, GoodsSlot> SafeSlots => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	private void OnInject(PlayerInventoryScrollerModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void InitSlots()
	{
	}

	private void OnTalnetUpdated()
	{
	}

	private void RebuildAppendedSlots()
	{
	}

	protected override InventoryGoodsSlotModel[] BuildSlotModels()
	{
		return null;
	}

	private void ApplyTotalOwnedCount(InventoryGoodsSlotModel slotModel)
	{
	}

	protected override InventoryGoodsSlotRowModel[] BuildRowModels(InventoryGoodsSlotModel[] slots)
	{
		return null;
	}

	private void OnScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
	{
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnMovePage(int index)
	{
	}

	private bool HasFilter()
	{
		return false;
	}

	private bool CheckFilter(GoodsSlot goodsSlot)
	{
		return false;
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		return null;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 0f;
	}
}
