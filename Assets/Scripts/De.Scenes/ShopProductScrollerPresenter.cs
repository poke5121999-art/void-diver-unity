using System.Collections.Generic;
using System.Linq;
using GameCommon.Table;
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

	protected override BaseScrollerModel<ShopProductSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(ShopProductScrollerModel model)
	{
		RefreshScroller();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override List<ShopProductSlotModel> BuildSlotModels()
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		return _gameShopManager.GetShopProductIds(_model.ShopProductGroupId).Select((int _) => TShopProduct.Table[_]).Where((TShopProduct _) => _.UnlockConditions == null || _.UnlockConditions.Length == 0 || _.UnlockConditions.All(_playerManager.CheckStateCondition))
			.Select((TShopProduct _) => new ShopProductSlotModel
			{
				Id = _.Id
			})
			.ToList();
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
