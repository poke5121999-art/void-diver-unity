using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx;

public abstract class HorizontalScrollerPresenter<TSlot> : BaseScrollerPresenter<TSlot> where TSlot : BaseSlotModel<TSlot>
{
	private CompositeDisposable _slotDisposable;

	protected override bool IsHorizontal => false;

	protected List<TSlot> CellViewModels { get; }

	protected abstract List<TSlot> BuildSlotModels();

	protected override int GetColumnCount()
	{
		return 0;
	}

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
