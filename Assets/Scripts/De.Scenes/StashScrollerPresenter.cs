using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class StashScrollerPresenter : GridScrollerPresenter<InventoryGoodsSlotModel, InventoryGoodsSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private StashScrollerModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private List<InventoryGoodsSlotModel> _slotModels;

	private GoodsSlot[] _lastSeenStashSlotRefs;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<InventoryGoodsSlotModel> ScrollerModel => null;

	private GamePlayer MyPlayer => null;

	private int NumberOfStashSlots => 0;

	private int SlotsPerRow => 0;

	private int RowsPerPage => 0;

	private float PageChangeThreshold => 0f;

	private IDictionary<int, GoodsSlot> StashSlots => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(StashScrollerModel model)
	{
	}

	private void InitSlots()
	{
	}

	private void OnTalnetUpdated()
	{
	}

	protected override InventoryGoodsSlotModel[] BuildSlotModels()
	{
		return null;
	}

	protected override InventoryGoodsSlotRowModel[] BuildRowModels(InventoryGoodsSlotModel[] slots)
	{
		return null;
	}

	private void OnScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
	{
	}

	private bool CheckFilter(GoodsSlot goodsSlot)
	{
		return false;
	}

	private void OnAllowedSubTypesChanged(EGoodsType[] allowedSubTypes)
	{
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnStashChanged()
	{
	}

	private void SnapshotStashSlotRefs()
	{
	}

	private void OnMovePage(int index)
	{
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
