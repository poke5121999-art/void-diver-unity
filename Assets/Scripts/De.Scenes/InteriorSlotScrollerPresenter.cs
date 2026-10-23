using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class InteriorSlotScrollerPresenter : VerticalScrollerPresenter<InteriorSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private InteriorSlotScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<InteriorSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	public void RefreshItems(InteriorSlotModel[] items)
	{
		_model.Items = items;
		RefreshScroller();
	}

	private void OnInject(InteriorSlotScrollerModel model)
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

	protected override List<InteriorSlotModel> BuildSlotModels()
	{
		if (_model.Items == null)
		{
			return new List<InteriorSlotModel>();
		}
		return new List<InteriorSlotModel>(_model.Items);
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
