using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx;
using UnityEngine;

public abstract class VerticalScrollerPresenter<TSlot> : BaseScrollerPresenter<TSlot> where TSlot : BaseSlotModel<TSlot>
{
	private CompositeDisposable _slotDisposable;

	protected List<TSlot> CellViewModels { get; } = new List<TSlot>();

	protected abstract List<TSlot> BuildSlotModels();

	// TODO(decomp): generic body has no address in the dump (shared generic, @0x0); written from signatures and the callers' behaviour
	protected override int GetItemCount()
	{
		return CellViewModels.Count;
	}

	public override int GetNumberOfCells(EnhancedScroller scroller)
	{
		int columnCount = Mathf.Max(1, GetColumnCount());
		return Mathf.CeilToInt((float)GetItemCount() / (float)columnCount);
	}

	protected void RefreshScroller(float scrollPosition = 0f)
	{
		_slotDisposable?.Dispose();
		_slotDisposable = new CompositeDisposable();
		CellViewModels.Clear();
		CellViewModels.AddRange(BuildSlotModels());
		// TODO(decomp): five slot-event lambdas (<RefreshScroller>b__7_0..4) assumed to forward OnClick/OnLeftClick/OnRightClick/OnEnter/OnExit to ScrollerModel.OnSlot*
		foreach (TSlot cellViewModel in CellViewModels)
		{
			cellViewModel.OnClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotClick.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnLeftClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotLeftClick.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnRightClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotRightClick.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnEnter.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotEnter.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnExit.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotExit.OnNext(slot);
			}).AddTo(_slotDisposable);
		}
		Scroller.ReloadData(scrollPosition);
		RefreshNavCount();
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
