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

	protected override BaseScrollerModel<SalesSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(SalesSlotScrollerModel model)
	{
	}

	protected override List<SalesSlotModel> BuildSlotModels()
	{
		return null;
	}

	private void OnSlotClick(SalesSlotModel slotModel)
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
