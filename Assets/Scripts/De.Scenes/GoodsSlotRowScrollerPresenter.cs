using System;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class GoodsSlotRowScrollerPresenter : GridScrollerPresenter<GoodsSlotModel, GoodsSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[SerializeField]
	private int _slotsPerRow = 7;

	[InjectModel]
	private GoodsSlotRowScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<GoodsSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return _slotsPerRow;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(GoodsSlotRowScrollerModel model)
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

	protected override GoodsSlotModel[] BuildSlotModels()
	{
		return _model.GoodsArray?.Select((Goods _) => new GoodsSlotModel(_)
		{
			ShowItemCount = false,
			ShowUnknownGoods = _model.ShowUnknownGoods
		}).ToArray() ?? Array.Empty<GoodsSlotModel>();
	}

	protected override GoodsSlotRowModel[] BuildRowModels(GoodsSlotModel[] slots)
	{
		int num = (int)Math.Ceiling((float)slots.Length / (float)_slotsPerRow);
		GoodsSlotRowModel[] array = new GoodsSlotRowModel[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new GoodsSlotRowModel
			{
				SlotModels = slots.Skip(i * _slotsPerRow).Take(_slotsPerRow).ToArray()
			};
		}
		return array;
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(base.CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
