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

	protected override BaseScrollerModel<InteriorSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	public void RefreshItems(InteriorSlotModel[] items)
	{
	}

	private void OnInject(InteriorSlotScrollerModel model)
	{
	}

	protected override List<InteriorSlotModel> BuildSlotModels()
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
