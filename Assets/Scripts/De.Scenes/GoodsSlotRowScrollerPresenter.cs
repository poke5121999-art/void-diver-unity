using EnhancedUI.EnhancedScroller;
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
	private int _slotsPerRow;

	[InjectModel]
	private GoodsSlotRowScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<GoodsSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(GoodsSlotRowScrollerModel model)
	{
	}

	protected override GoodsSlotModel[] BuildSlotModels()
	{
		return null;
	}

	protected override GoodsSlotRowModel[] BuildRowModels(GoodsSlotModel[] slots)
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
