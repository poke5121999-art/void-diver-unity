using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx;
using UnityEngine;

public abstract class HorizontalScrollerPresenter<TSlot> : BaseScrollerPresenter<TSlot> where TSlot : BaseSlotModel<TSlot>
{
	private CompositeDisposable _slotDisposable;

	protected override bool IsHorizontal => true;

	protected List<TSlot> CellViewModels { get; } = new List<TSlot>();

	protected abstract List<TSlot> BuildSlotModels();

	// TODO(decomp): generic body has no address in the dump (shared generic, @0x0); written from signatures, the callers' behaviour and VerticalScrollerPresenter
	protected override int GetColumnCount()
	{
		return Mathf.Max(1, CellViewModels.Count);
	}

	protected override int GetItemCount()
	{
		return CellViewModels.Count;
	}

	public override int GetNumberOfCells(EnhancedScroller scroller)
	{
		return CellViewModels.Count;
	}

	// TODO(decomp): shape copied from GridScrollerPresenter.RefreshScroller (shared body @0x181296eb0); the Horizontal shared body was not disassembled
	protected void RefreshScroller(float scrollPosition = 0f)
	{
		bool hasActiveSelection = _hasActiveSelection;
		if (_enableNav && _hasActiveSelection && CellViewModels.Count != 0)
		{
			OnDeselect(_focusedIndex);
			_hasActiveSelection = false;
		}
		_slotDisposable?.Dispose();
		_slotDisposable = new CompositeDisposable();
		CellViewModels.Clear();
		CellViewModels.AddRange(BuildSlotModels());
		// TODO(decomp): the five slot-event lambdas (<RefreshScroller>b__10_0..4) have no address; assumed to forward OnClick/OnEnter/OnExit/OnLeftClick/OnRightClick to ScrollerModel.OnSlot* @0x181296eb0
		foreach (TSlot slotModel in CellViewModels)
		{
			slotModel.OnClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotClick.OnNext(slot);
			}).AddTo(_slotDisposable);
			slotModel.OnEnter.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotEnter.OnNext(slot);
			}).AddTo(_slotDisposable);
			slotModel.OnExit.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotExit.OnNext(slot);
			}).AddTo(_slotDisposable);
			slotModel.OnLeftClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotLeftClick.OnNext(slot);
			}).AddTo(_slotDisposable);
			slotModel.OnRightClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotRightClick.OnNext(slot);
			}).AddTo(_slotDisposable);
		}
		Scroller.Delegate = this;
		Scroller.ReloadData();
		Scroller.SetScrollPositionImmediately(scrollPosition);
		_hasActiveSelection = hasActiveSelection;
		if (CellViewModels.Count == 0)
		{
			RefreshNavCount();
		}
		else
		{
			RefreshNav();
		}
	}

	protected override void OnSelect(int index)
	{
		if (index >= 0 && index < CellViewModels.Count)
		{
			CellViewModels[index].TriggerSlotEnter();
		}
	}

	protected override void OnDeselect(int index)
	{
		if (index >= 0 && index < CellViewModels.Count)
		{
			CellViewModels[index].TriggerSlotExit();
		}
	}

	protected override void OnDestroy()
	{
		_slotDisposable?.Dispose();
		base.OnDestroy();
	}
}
