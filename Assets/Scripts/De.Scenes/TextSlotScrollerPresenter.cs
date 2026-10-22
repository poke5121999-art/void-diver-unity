using System.Collections.Generic;
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

	protected override BaseScrollerModel<TextSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(TextSlotScrollerModel model)
	{
	}

	protected override List<TextSlotModel> BuildSlotModels()
	{
		return null;
	}

	public void SetSelected(int index)
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
