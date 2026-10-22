using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class CraftingGoodsScrollerPresenter : VerticalScrollerPresenter<CraftingGoodsSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private CraftingGoodsScrollerModel _model;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<CraftingGoodsSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(CraftingGoodsScrollerModel model)
	{
	}

	private void OnCraftableFilterChanged()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnSelect(int index)
	{
	}

	public CraftingGoodsSlotModel FindSlotModel(int craftingId)
	{
		return null;
	}

	public void NavigateToItem(int craftingId)
	{
	}

	public void ChangeFilter(EGoodsType subType)
	{
	}

	protected override List<CraftingGoodsSlotModel> BuildSlotModels()
	{
		return null;
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
