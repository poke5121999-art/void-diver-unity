using System;
using EnhancedUI.EnhancedScroller;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(EnhancedScroller))]
public abstract class BaseScrollerPresenter<TSlot> : Presenter, IEnhancedScrollerDelegate where TSlot : BaseSlotModel<TSlot>
{
	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameInputManager InputManager;

	private ScrollRect _scrollRect;

	private int _cellCount;

	private int _columnCount;

	protected int _focusedIndex;

	protected bool _enableNav;

	protected bool _hasActiveSelection;

	private bool _pendingSelect;

	private BaseScrollerModel<TSlot> _lastNavModel;

	private Vector2 _heldDir;

	private float _nextNavTime;

	protected CompositeDisposable InputDisposable;

	protected EnhancedScroller Scroller { get; private set; }

	protected abstract BaseScrollerModel<TSlot> ScrollerModel { get; }

	private InputAction MoveScrollAction => InputManager.UiPadActions.MoveScroll;

	private InputAction MovePanelAction => InputManager.UiPadActions.MovePanel;

	private static float FirstDelay => TConst.PadNavFirstDelay.FloatValue;

	private static float RepeatRate => TConst.PadNavRepeatRate.FloatValue;

	protected virtual bool IsHorizontal => false;

	// TODO(decomp): the generic bodies of BaseScrollerPresenter<T> have no address in the binary (0x0 in methods.tsv).
	// RefreshNav, the input-subscription part of OnInputEnabled and OnDestroy are copied from the bodies that subclasses inline;
	// the rest is inferred from the signatures, BasePanelPresenter and EnhancedScroller.
	protected override void Awake()
	{
		base.Awake();
		Scroller = GetComponent<EnhancedScroller>();
		_scrollRect = GetComponent<ScrollRect>();
	}

	protected virtual void Start()
	{
	}

	protected abstract int GetColumnCount();

	protected abstract int GetItemCount();

	public abstract int GetNumberOfCells(EnhancedScroller scroller);

	public abstract float GetCellViewSize(EnhancedScroller scroller, int dataIndex);

	public abstract EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);

	protected void RefreshNav(int initialIndex = -1)
	{
		_cellCount = GetItemCount();
		_columnCount = Math.Max(1, GetColumnCount());
		if (ScrollerModel != _lastNavModel)
		{
			_lastNavModel = ScrollerModel;
			_focusedIndex = 0;
			_hasActiveSelection = false;
		}
		int num = Math.Max(0, _cellCount - 1);
		int num2 = initialIndex;
		if (num2 < 0)
		{
			num2 = Math.Max(0, _focusedIndex);
		}
		if (num2 > num)
		{
			num2 = num;
		}
		bool enableNav = _enableNav;
		_focusedIndex = num2;
		if (enableNav && _hasActiveSelection)
		{
			_pendingSelect = true;
		}
	}

	protected void RefreshNavCount()
	{
		_cellCount = GetItemCount();
		_columnCount = Math.Max(1, GetColumnCount());
		_focusedIndex = Math.Clamp(_focusedIndex, 0, Math.Max(0, _cellCount - 1));
		if (_cellCount < 1)
		{
			_hasActiveSelection = false;
		}
	}

	protected virtual void OnDestroy()
	{
		InputDisposable.Dispose();
	}

	protected virtual void OnInputEnabled()
	{
		_enableNav = false;
		_heldDir = MoveScrollAction.ReadValue<Vector2>();
		_nextNavTime = Time.unscaledTime + FirstDelay;
		InputDisposable?.Dispose();
		InputDisposable = new CompositeDisposable();
		MoveScrollAction.OnPressStartAsObservable().Subscribe(OnMoveScrollStarted).AddTo(InputDisposable);
		MoveScrollAction.OnClickAsObservable().Subscribe(OnMoveScrollPerformed).AddTo(InputDisposable);
		MoveScrollAction.OnPressEndAsObservable().Subscribe(OnMoveScrollCanceled).AddTo(InputDisposable);
		MovePanelAction.OnPressStartAsObservable().Subscribe(OnMovePanelStarted).AddTo(InputDisposable);
		InputManager.RxControlDevice.Where((EControlDevice device) => device == EControlDevice.GamePad).Subscribe(delegate
		{
			if (_enableNav && _cellCount > 0)
			{
				Navigate(_focusedIndex);
			}
		}).AddTo(InputDisposable);
		_enableNav = true;
		BaseScrollerModel<TSlot> scrollerModel = ScrollerModel;
		int num = ((scrollerModel == null || scrollerModel.EntryDirection == ENavDir.None || _cellCount < 1) ? _focusedIndex : FindDirectionalEntryIndex(scrollerModel.EntryDirection));
		if (InputManager.RxControlDevice.Value == EControlDevice.GamePad)
		{
			Navigate(num);
			return;
		}
		_focusedIndex = Math.Clamp(num, 0, Math.Max(0, _cellCount - 1));
	}

	protected virtual void OnInputDisabled()
	{
		_enableNav = false;
		_heldDir = Vector2.zero;
		InputDisposable?.Dispose();
		if (_hasActiveSelection && _cellCount > 0)
		{
			OnDeselect(_focusedIndex);
		}
	}

	private void Update()
	{
		if (!_enableNav)
		{
			return;
		}
		if (_pendingSelect)
		{
			_pendingSelect = false;
			if (_hasActiveSelection && _cellCount > 0)
			{
				OnSelect(_focusedIndex);
			}
		}
		if (_heldDir != Vector2.zero && Time.unscaledTime >= _nextNavTime)
		{
			DoNavigate(_heldDir);
			_nextNavTime = Time.unscaledTime + RepeatRate;
		}
	}

	private void OnMoveScrollStarted(InputAction.CallbackContext context)
	{
		Vector2 vector = context.ReadValue<Vector2>();
		_heldDir = vector;
		DoNavigate(vector);
		_nextNavTime = Time.unscaledTime + FirstDelay;
	}

	private void OnMoveScrollPerformed(InputAction.CallbackContext context)
	{
		Vector2 vector = context.ReadValue<Vector2>();
		if (GetDiscreteDir(vector) == GetDiscreteDir(_heldDir))
		{
			_heldDir = vector;
			return;
		}
		_heldDir = vector;
		DoNavigate(vector);
		_nextNavTime = Time.unscaledTime + FirstDelay;
	}

	private static Vector2Int GetDiscreteDir(Vector2 v)
	{
		if (v.magnitude < 0.5f)
		{
			return Vector2Int.zero;
		}
		if (Mathf.Abs(v.y) <= Mathf.Abs(v.x))
		{
			return new Vector2Int(v.x > 0f ? 1 : -1, 0);
		}
		return new Vector2Int(0, v.y > 0f ? 1 : -1);
	}

	private void OnMoveScrollCanceled(InputAction.CallbackContext context)
	{
		_heldDir = Vector2.zero;
	}

	private void OnMovePanelStarted(InputAction.CallbackContext ctx)
	{
		Gamepad current = Gamepad.current;
		Vector2 dir = current != null ? current.rightStick.ReadValue() : Vector2.zero;
		dir.Normalize();
		DoNavigatePanelOut(dir);
	}

	private void DoNavigatePanelOut(Vector2 dir)
	{
		BaseScrollerModel<TSlot> scrollerModel = ScrollerModel;
		if (scrollerModel == null)
		{
			return;
		}
		if (dir.y > 0.5f)
		{
			scrollerModel.OutOfTop.OnNext(Unit.Default);
		}
		else if (dir.y < -0.5f)
		{
			scrollerModel.OutOfBottom.OnNext(Unit.Default);
		}
		else if (dir.x < -0.5f)
		{
			scrollerModel.OutOfLeft.OnNext(Unit.Default);
		}
		else if (dir.x > 0.5f)
		{
			scrollerModel.OutOfRight.OnNext(Unit.Default);
		}
	}

	private void DoNavigate(Vector2 value)
	{
		if (value.y > 0.5f)
		{
			OnNavigateUp();
		}
		else if (value.y < -0.5f)
		{
			OnNavigateDown();
		}
		else if (value.x < -0.5f)
		{
			OnNavigateLeft();
		}
		else if (value.x > 0.5f)
		{
			OnNavigateRight();
		}
	}

	private void OnNavigateUp()
	{
		if (_cellCount < 1)
		{
			return;
		}
		int num = _focusedIndex - _columnCount;
		if (num < 0)
		{
			ScrollerModel.OutOfTop.OnNext(Unit.Default);
		}
		else
		{
			Navigate(num);
		}
	}

	private void OnNavigateDown()
	{
		if (_cellCount < 1)
		{
			return;
		}
		int num = _focusedIndex + _columnCount;
		if (num >= _cellCount)
		{
			ScrollerModel.OutOfBottom.OnNext(Unit.Default);
		}
		else
		{
			Navigate(num);
		}
	}

	private void OnNavigateLeft()
	{
		if (_cellCount < 1)
		{
			return;
		}
		if (_focusedIndex % _columnCount == 0)
		{
			ScrollerModel.OutOfLeft.OnNext(Unit.Default);
		}
		else
		{
			Navigate(_focusedIndex - 1);
		}
	}

	private void OnNavigateRight()
	{
		if (_cellCount < 1)
		{
			return;
		}
		if (_focusedIndex % _columnCount == _columnCount - 1 || _focusedIndex + 1 >= _cellCount)
		{
			ScrollerModel.OutOfRight.OnNext(Unit.Default);
		}
		else
		{
			Navigate(_focusedIndex + 1);
		}
	}

	private int FindEntryIndex()
	{
		BaseScrollerModel<TSlot> scrollerModel = ScrollerModel;
		if (scrollerModel != null && scrollerModel.EntryDirection != ENavDir.None && _cellCount > 0)
		{
			return FindDirectionalEntryIndex(scrollerModel.EntryDirection);
		}
		Vector2? currentSelectedPosition = InputManager.CurrentSelectedPosition;
		if (currentSelectedPosition.HasValue && _cellCount > 0)
		{
			return FindClosestIndex(currentSelectedPosition.Value);
		}
		return _focusedIndex;
	}

	private int FindDirectionalEntryIndex(ENavDir dir)
	{
		int num = Math.Max(1, _columnCount);
		int num2 = Math.Max(0, _cellCount - 1);
		int num3 = _focusedIndex / num;
		int num4 = _focusedIndex % num;
		int num5 = num2 / num;
		switch (dir)
		{
		case ENavDir.Up:
			return Math.Min(num4, num2);
		case ENavDir.Down:
			return Math.Min(num5 * num + num4, num2);
		case ENavDir.Left:
			return Math.Min(num3 * num, num2);
		case ENavDir.Right:
			return Math.Min(num3 * num + num - 1, num2);
		default:
			return Math.Clamp(_focusedIndex, 0, num2);
		}
	}

	private int FindClosestRow(Vector2 worldPosition, int rowCount)
	{
		int result = 0;
		float num = float.MaxValue;
		for (int i = 0; i < rowCount; i++)
		{
			EnhancedScrollerCellView cellViewAtDataIndex = Scroller.GetCellViewAtDataIndex(i * _columnCount);
			if (!(cellViewAtDataIndex == null))
			{
				Vector3 position = cellViewAtDataIndex.transform.position;
				float num2 = (IsHorizontal ? Mathf.Abs(position.x - worldPosition.x) : Mathf.Abs(position.y - worldPosition.y));
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
		}
		return result;
	}

	private int FindClosestIndex(Vector2 worldPosition)
	{
		int num = Math.Max(1, _columnCount);
		int rowCount = (_cellCount + num - 1) / num;
		int num2 = FindClosestRow(worldPosition, rowCount);
		int num3 = num2 * num;
		int result = Math.Min(num3, Math.Max(0, _cellCount - 1));
		float num4 = float.MaxValue;
		for (int i = 0; i < num; i++)
		{
			int num5 = num3 + i;
			if (num5 >= _cellCount)
			{
				break;
			}
			EnhancedScrollerCellView cellViewAtDataIndex = Scroller.GetCellViewAtDataIndex(num5);
			if (!(cellViewAtDataIndex == null))
			{
				float sqrMagnitude = ((Vector2)cellViewAtDataIndex.transform.position - worldPosition).sqrMagnitude;
				if (sqrMagnitude < num4)
				{
					num4 = sqrMagnitude;
					result = num5;
				}
			}
		}
		return result;
	}

	private void Navigate(int target)
	{
		if (_cellCount < 1)
		{
			return;
		}
		target = Math.Clamp(target, 0, _cellCount - 1);
		int focusedIndex = _focusedIndex;
		if (_hasActiveSelection && focusedIndex != target)
		{
			OnDeselect(focusedIndex);
		}
		_focusedIndex = target;
		_hasActiveSelection = true;
		ScrollIntoView(target / Math.Max(1, _columnCount), target > focusedIndex);
		_pendingSelect = true;
	}

	private void ScrollIntoView(int rowIndex, bool scrollingDown)
	{
		if (Scroller == null)
		{
			return;
		}
		int dataIndex = rowIndex * Math.Max(1, _columnCount);
		float scrollPositionForDataIndex = Scroller.GetScrollPositionForDataIndex(dataIndex, EnhancedScroller.CellViewPositionEnum.Before);
		float num = Scroller.GetScrollPositionForDataIndex(dataIndex, EnhancedScroller.CellViewPositionEnum.After) - Scroller.ScrollRectSize;
		float scrollPosition = Scroller.ScrollPosition;
		float num2 = scrollPosition;
		if (scrollingDown)
		{
			if (scrollPosition < num)
			{
				num2 = num;
			}
			else if (scrollPosition > scrollPositionForDataIndex)
			{
				num2 = scrollPositionForDataIndex;
			}
		}
		else if (scrollPosition > scrollPositionForDataIndex)
		{
			num2 = scrollPositionForDataIndex;
		}
		else if (scrollPosition < num)
		{
			num2 = num;
		}
		if (num2 != scrollPosition)
		{
			Scroller.ScrollPosition = Mathf.Clamp(num2, 0f, Mathf.Max(0f, Scroller.ScrollSize - Scroller.ScrollRectSize));
		}
	}

	protected abstract void OnSelect(int index);

	protected abstract void OnDeselect(int index);
}
