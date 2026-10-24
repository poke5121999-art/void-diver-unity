using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx;
using UnityEngine.UI;

public abstract class VerticalScrollerPresenter<TSlot> : BaseScrollerPresenter<TSlot> where TSlot : BaseSlotModel<TSlot>
{
	private CompositeDisposable _slotDisposable;

	protected List<TSlot> CellViewModels { get; } = new List<TSlot>();

	protected abstract List<TSlot> BuildSlotModels();

	// Bodies read from the shared <object> instantiation: GetItemCount = GetNumberOfCells @0x181296ce0 (same bytes), RefreshScroller @0x181fd2fa0,
	// OnSelect @0x181fd2ed0, OnDeselect @0x181fd2e00, OnDestroy @0x181301030; lambdas b__7_0..4 @0x1813017b0.. (machine code checked)
	protected override int GetItemCount()
	{
		return CellViewModels.Count;
	}

	public override int GetNumberOfCells(EnhancedScroller scroller)
	{
		return CellViewModels.Count;
	}

	protected void RefreshScroller(float scrollPosition = 0f)
	{
		bool hasActiveSelection = _hasActiveSelection;
		if (_enableNav && hasActiveSelection)
		{
			OnDeselect(_focusedIndex);
			_hasActiveSelection = false;
		}
		_slotDisposable?.Dispose();
		_slotDisposable = new CompositeDisposable();
		CellViewModels.Clear();
		CellViewModels.AddRange(BuildSlotModels());
		// subscription order +0x28,+0x40,+0x48,+0x30,+0x38 (OnClick, OnEnter, OnExit, OnLeftClick, OnRightClick); lambdas b__7_0..4 read ScrollerModel field +0x48,+0x38,+0x40,+0x50,+0x58
		foreach (TSlot cellViewModel in CellViewModels)
		{
			cellViewModel.OnClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotClick.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnEnter.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotEnter.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnExit.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotExit.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnLeftClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotLeftClick.OnNext(slot);
			}).AddTo(_slotDisposable);
			cellViewModel.OnRightClick.Subscribe(delegate(TSlot slot)
			{
				ScrollerModel.OnSlotRightClick.OnNext(slot);
			}).AddTo(_slotDisposable);
		}
		Scroller.Delegate = this;
		Scroller.ReloadData();
		Scroller.SetScrollPositionImmediately(scrollPosition);
		if (CellViewModels.Count == 0)
		{
			RefreshNavCount();
			return;
		}
		_hasActiveSelection = hasActiveSelection;
		RefreshNav();
	}

	protected override void OnSelect(int index)
	{
		Selectable selectable = CellViewModels[index].Selectable;
		if (selectable != null)
		{
			InputManager.SelectObject(selectable.gameObject);
		}
	}

	protected override void OnDeselect(int index)
	{
		Selectable selectable = CellViewModels[index].Selectable;
		if (selectable != null)
		{
			InputManager.DeselectObject(selectable.gameObject);
		}
	}

	protected override void OnDestroy()
	{
		_slotDisposable?.Dispose();
		base.OnDestroy();
	}
}
