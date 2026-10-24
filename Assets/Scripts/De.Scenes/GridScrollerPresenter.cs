using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx;
using UnityEngine.UI;

public abstract class GridScrollerPresenter<TSlot, TRow> : BaseScrollerPresenter<TSlot> where TSlot : BaseSlotModel<TSlot> where TRow : class
{
	private TSlot[] _slotModels;

	private CompositeDisposable _slotDisposable;

	protected List<TRow> CellViewModels { get; } = new List<TRow>();

	protected abstract TSlot[] BuildSlotModels();

	protected abstract TRow[] BuildRowModels(TSlot[] slots);

	// Bodies read from the shared <object, object> instantiation (methodSpec -> genericMethodPointers): GetItemCount @0x181296cc0, GetNumberOfCells @0x181296ce0, RefreshScroller @0x181296eb0, OnSelect @0x181296e00, OnDestroy @0x181296db0
	protected override int GetItemCount()
	{
		return _slotModels.Length;
	}

	public override int GetNumberOfCells(EnhancedScroller scroller)
	{
		return CellViewModels.Count;
	}

	protected void RefreshScroller(float scrollPosition = 0f)
	{
		bool hasActiveSelection = _hasActiveSelection;
		if (_enableNav && _hasActiveSelection && _slotModels.Length != 0)
		{
			OnDeselect(_focusedIndex);
			_hasActiveSelection = false;
		}
		_slotDisposable?.Dispose();
		_slotDisposable = new CompositeDisposable();
		_slotModels = BuildSlotModels();
		CellViewModels.Clear();
		CellViewModels.AddRange(BuildRowModels(_slotModels));
		// subscription order +0x28,+0x40,+0x48,+0x30,+0x38 (OnClick, OnEnter, OnExit, OnLeftClick, OnRightClick); lambdas b__9_0..4 read ScrollerModel field +0x48,+0x38,+0x40,+0x50,+0x58 = OnSlotClick, OnSlotEnter, OnSlotExit, OnSlotLeftClick, OnSlotRightClick (machine code of the Vertical twins b__7_*)
		foreach (TSlot slotModel in _slotModels)
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
		if (_slotModels.Length == 0)
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
		Selectable selectable = _slotModels[index].Selectable;
		if (selectable != null)
		{
			InputManager.SelectObject(selectable.gameObject);
		}
	}

	protected override void OnDeselect(int index)
	{
		Selectable selectable = _slotModels[index].Selectable;
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
