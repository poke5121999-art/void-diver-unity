using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class SalesSlotScrollerPresenter : VerticalScrollerPresenter<SalesSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private SalesSlotScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<SalesSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(SalesSlotScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RefreshScroller();
		_model.OnSlotClick.Subscribe(OnSlotClick).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
		if (CellViewModels.Count > 0)
		{
			SalesSlotModel salesSlotModel = ((_model.InitialSelectedId >= 0) ? (CellViewModels.Find((SalesSlotModel m) => m.Id == _model.InitialSelectedId) ?? CellViewModels[0]) : CellViewModels[0]);
			RefreshNav(CellViewModels.IndexOf(salesSlotModel));
		}
	}

	protected override List<SalesSlotModel> BuildSlotModels()
	{
		return _model.SlotModels;
	}

	private void OnSlotClick(SalesSlotModel slotModel)
	{
		CellViewModels.ForEach(delegate(SalesSlotModel m)
		{
			m.RxIsSelected.Value = m.Id == slotModel.Id;
		});
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
