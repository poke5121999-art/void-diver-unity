using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class TextSlotScrollerPresenter : VerticalScrollerPresenter<TextSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private TextSlotScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<TextSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(TextSlotScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RefreshScroller();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override List<TextSlotModel> BuildSlotModels()
	{
		return _model.Texts.Select((string text, int i) => new TextSlotModel
		{
			Index = i,
			Text = text
		}).ToList();
	}

	public void SetSelected(int index)
	{
		for (int i = 0; i < base.CellViewModels.Count; i++)
		{
			base.CellViewModels[i].RxIsSelected.Value = i == index;
		}
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
