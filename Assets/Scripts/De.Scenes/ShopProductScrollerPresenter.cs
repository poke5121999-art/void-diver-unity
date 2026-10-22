using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class ShopProductScrollerPresenter : VerticalScrollerPresenter<ShopProductSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private ShopProductScrollerModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameShopManager _gameShopManager;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<ShopProductSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(ShopProductScrollerModel model)
	{
	}

	protected override List<ShopProductSlotModel> BuildSlotModels()
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
