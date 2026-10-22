using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx;

public abstract class GridScrollerPresenter<TSlot, TRow> : BaseScrollerPresenter<TSlot> where TSlot : BaseSlotModel<TSlot> where TRow : class
{
	private TSlot[] _slotModels;

	private CompositeDisposable _slotDisposable;

	protected List<TRow> CellViewModels { get; }

	protected abstract TSlot[] BuildSlotModels();

	protected abstract TRow[] BuildRowModels(TSlot[] slots);

	protected override int GetItemCount()
	{
		return 0;
	}

	public override int GetNumberOfCells(EnhancedScroller scroller)
	{
		return 0;
	}

	protected void RefreshScroller(float scrollPosition = 0f)
	{
	}

	protected override void OnSelect(int index)
	{
	}

	protected override void OnDeselect(int index)
	{
	}

	protected override void OnDestroy()
	{
	}
}
