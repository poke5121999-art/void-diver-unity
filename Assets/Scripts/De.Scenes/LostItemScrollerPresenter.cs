using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class LostItemScrollerPresenter : VerticalScrollerPresenter<LostItemSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private LostItemScrollerModel _model;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<LostItemSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(LostItemScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
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

	protected override List<LostItemSlotModel> BuildSlotModels()
	{
		if (_model.Mode != ELostItemScrollerMode.All)
		{
			return BuildSelectedSlotModels();
		}
		return BuildAllSlotModels();
	}

	private List<LostItemSlotModel> BuildSelectedSlotModels()
	{
		List<LostItemSlotModel> list = new List<LostItemSlotModel>();
		foreach (LostItemSelectInfo rxSelectLostGoodsInfo in _playerManager.RxSelectLostGoodsInfos)
		{
			if (_playerManager.LostGoods.TryGetValue(rxSelectLostGoodsInfo.SlotIndex, out var value))
			{
				bool flag = value.Goods.IsArtifact();
				int num = (int)((float)value.Goods.SellPriceTotal * rxSelectLostGoodsInfo.PriceMultiplier);
				if (flag)
				{
					num /= TConst.CoinToGoldRate.IntValue;
				}
				list.Add(new LostItemSlotModel
				{
					LostGoods = value,
					Price = num,
					IsArtifact = flag,
					Mode = ELostItemScrollerMode.SelectedOnly
				});
			}
			else
			{
				Logger.Warning(string.Format("[LostItemScrollerPresenter] LostGoods not found for SlotIndex: {0}", rxSelectLostGoodsInfo.SlotIndex));
			}
		}
		return list;
	}

	private List<LostItemSlotModel> BuildAllSlotModels()
	{
		List<LostItemSlotModel> list = new List<LostItemSlotModel>();
		foreach (KeyValuePair<int, LostGoods> lostGood in _playerManager.LostGoods)
		{
			LostGoods value = lostGood.Value;
			bool flag = value.Goods.IsArtifact();
			int num = value.Goods.SellPriceTotal;
			if (flag)
			{
				num /= TConst.CoinToGoldRate.IntValue;
			}
			list.Add(new LostItemSlotModel
			{
				LostGoods = value,
				Price = num,
				IsArtifact = flag,
				Mode = ELostItemScrollerMode.All
			});
		}
		return list;
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
