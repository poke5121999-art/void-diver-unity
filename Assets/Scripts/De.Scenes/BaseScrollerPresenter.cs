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

	// Bodies read from the shared <object> instantiation (methodSpec -> genericMethodPointers, machine code checked):
	// Awake @0x1823dfd10, Start @0x1823e1e80, RefreshNav @0x1823e1c40, OnInputEnabled @0x1823e0f20, Update @0x1823e1f50, DoNavigate @0x1823dff50,
	// Navigate @0x1823e0b40, ScrollIntoView @0x1823e1d30, FindDirectionalEntryIndex @0x1823e0630, FindClosestRow @0x1823e03d0, FindClosestIndex @0x1823e01d0.
	protected override void Awake()
	{
		Scroller = GetComponent<EnhancedScroller>();
		_scrollRect = GetComponent<ScrollRect>();
		base.Awake();
	}

	protected virtual void Start()
	{
		_scrollRect.decelerationRate = Const.Scroller.DECELERATION_RATE;
		_scrollRect.scrollSensitivity = Const.Scroller.SCROLL_SENSITIVITY;
		_scrollRect.movementType = ScrollRect.MovementType.Clamped;
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
	}

	protected virtual void OnDestroy()
	{
		InputDisposable?.Dispose();
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
				OnSelect(_focusedIndex);
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
		if (_enableNav && _cellCount > 0 && _hasActiveSelection)
		{
			OnDeselect(_focusedIndex);
			_hasActiveSelection = false;
		}
		_enableNav = false;
		_pendingSelect = false;
		_heldDir = Vector2.zero;
		InputDisposable?.Dispose();
		InputDisposable = null;
	}

	private void Update()
	{
		if (_pendingSelect && _enableNav && _cellCount > 0)
		{
			_hasActiveSelection = true;
			_pendingSelect = false;
			OnSelect(_focusedIndex);
		}
		if (_enableNav && _heldDir != Vector2.zero && Time.unscaledTime >= _nextNavTime)
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
		dir = Vector2.Normalize(dir);
		DoNavigatePanelOut(dir);
	}

	private void DoNavigatePanelOut(Vector2 dir)
	{
		if (dir.y > 0.5f)
		{
			ScrollerModel?.OutOfTop.OnNext(Unit.Default);
		}
		else if (dir.y < -0.5f)
		{
			ScrollerModel?.OutOfBottom.OnNext(Unit.Default);
		}
		else if (dir.x < -0.5f)
		{
			ScrollerModel?.OutOfLeft.OnNext(Unit.Default);
		}
		else if (dir.x > 0.5f)
		{
			ScrollerModel?.OutOfRight.OnNext(Unit.Default);
		}
	}

	private void DoNavigate(Vector2 value)
	{
		if (_cellCount <= 0)
		{
			DoNavigatePanelOut(value);
		}
		else if (!IsHorizontal)
		{
			if (value.y > 0.5f)
			{
				OnNavigateUp();
			}
			else if (value.y < -0.5f)
			{
				OnNavigateDown();
			}
			else if (value.x > 0.5f)
			{
				OnNavigateRight();
			}
			else if (value.x < -0.5f)
			{
				OnNavigateLeft();
			}
		}
		else if (value.x < -0.5f)
		{
			OnNavigateLeft();
		}
		else if (value.x > 0.5f)
		{
			OnNavigateRight();
		}
		else if (value.y > 0.5f)
		{
			ScrollerModel?.OutOfTop.OnNext(Unit.Default);
		}
		else if (value.y < -0.5f)
		{
			ScrollerModel?.OutOfBottom.OnNext(Unit.Default);
		}
	}

	private void OnNavigateUp()
	{
		Navigate(_focusedIndex - _columnCount);
	}

	private void OnNavigateDown()
	{
		Navigate(_focusedIndex + _columnCount);
	}

	private void OnNavigateLeft()
	{
		if (IsHorizontal || _focusedIndex % _columnCount != 0)
		{
			Navigate(_focusedIndex - 1);
		}
		else
		{
			ScrollerModel?.OutOfLeft.OnNext(Unit.Default);
		}
	}

	private void OnNavigateRight()
	{
		if (IsHorizontal || ((_focusedIndex + 1) % _columnCount != 0 && _focusedIndex + 1 < _cellCount))
		{
			Navigate(_focusedIndex + 1);
		}
		else
		{
			ScrollerModel?.OutOfRight.OnNext(Unit.Default);
		}
	}

	private int FindEntryIndex()
	{
		BaseScrollerModel<TSlot> scrollerModel = ScrollerModel;
		if (scrollerModel != null && scrollerModel.EntryDirection != ENavDir.None && _cellCount > 0)
		{
			return FindDirectionalEntryIndex(scrollerModel.EntryDirection);
		}
		return _focusedIndex;
	}

	private int ClampToCells(int index)
	{
		return index < 0 ? 0 : Math.Min(index, _cellCount - 1);
	}

	private int FindDirectionalEntryIndex(ENavDir dir)
	{
		int numberOfCells = GetNumberOfCells(Scroller);
		Vector2? currentSelectedPosition = InputManager.CurrentSelectedPosition;
		bool hasValue = currentSelectedPosition.HasValue;
		Vector2 position = hasValue ? InputManager.CurrentSelectedPosition.Value : Vector2.zero;
		if ((!IsHorizontal && (dir == ENavDir.Left || dir == ENavDir.Right)) || (IsHorizontal && (dir == ENavDir.Up || dir == ENavDir.Down)))
		{
			int num = (dir == ENavDir.Left || dir == ENavDir.Up) ? _columnCount - 1 : 0;
			int num2 = hasValue ? FindClosestRow(position, numberOfCells) : 0;
			return ClampToCells(num2 * _columnCount + num);
		}
		int num3 = ((IsHorizontal ? dir == ENavDir.Left : dir == ENavDir.Up) ? numberOfCells - 1 : 0) * _columnCount;
		if (_columnCount > 1 && hasValue)
		{
			RectTransform component = Scroller.GetComponent<RectTransform>();
			Vector3 vector = component.InverseTransformPoint(position);
			float width = component.rect.width;
			int columnCount = _columnCount;
			int num4 = Mathf.FloorToInt((vector.x - component.rect.x) / (width / (float)columnCount));
			if (num4 < 0)
			{
				num4 = 0;
			}
			else if (num4 > _columnCount - 1)
			{
				num4 = _columnCount - 1;
			}
			return ClampToCells(num4 + num3);
		}
		return ClampToCells(num3);
	}

	private int FindClosestRow(Vector2 worldPosition, int rowCount)
	{
		RectTransform component = Scroller.GetComponent<RectTransform>();
		Vector3 vector = component.InverseTransformPoint(worldPosition);
		Rect rect = component.rect;
		float num = ((!IsHorizontal) ? (rect.y + rect.height - vector.y) : (vector.x - rect.x));
		num += Scroller.ScrollPosition;
		float num2 = float.MaxValue;
		int result = 0;
		for (int i = 0; i < rowCount; i++)
		{
			float scrollPositionForDataIndex = Scroller.GetScrollPositionForDataIndex(i, EnhancedScroller.CellViewPositionEnum.Before);
			float num3 = Mathf.Abs(GetCellViewSize(Scroller, i) * 0.5f + scrollPositionForDataIndex - num);
			float num4 = num2;
			if (num2 > num3)
			{
				num2 = num3;
			}
			if (!(num4 <= num3))
			{
				result = i;
			}
		}
		return result;
	}

	private int FindClosestIndex(Vector2 worldPosition)
	{
		if (_cellCount == 0)
		{
			return 0;
		}
		int numberOfCells = GetNumberOfCells(Scroller);
		if (numberOfCells == 0)
		{
			return 0;
		}
		int num = FindClosestRow(worldPosition, numberOfCells) * _columnCount;
		if (_columnCount > 1)
		{
			RectTransform component = Scroller.GetComponent<RectTransform>();
			Vector3 vector = component.InverseTransformPoint(worldPosition);
			float width = component.rect.width;
			int columnCount = _columnCount;
			int num2 = Mathf.FloorToInt((vector.x - component.rect.x) / (width / (float)columnCount));
			if (num2 < 0)
			{
				num2 = 0;
			}
			else if (num2 > _columnCount - 1)
			{
				num2 = _columnCount - 1;
			}
			return ClampToCells(num2 + num);
		}
		return ClampToCells(num);
	}

	private void Navigate(int target)
	{
		if (!_enableNav || Scroller == null || !Scroller.gameObject.activeInHierarchy || _cellCount == 0)
		{
			return;
		}
		if (target < 0)
		{
			if (IsHorizontal)
			{
				ScrollerModel?.OutOfLeft.OnNext(Unit.Default);
			}
			else
			{
				ScrollerModel?.OutOfTop.OnNext(Unit.Default);
			}
			return;
		}
		if (target >= _cellCount)
		{
			if (IsHorizontal)
			{
				ScrollerModel?.OutOfRight.OnNext(Unit.Default);
			}
			else
			{
				ScrollerModel?.OutOfBottom.OnNext(Unit.Default);
			}
			return;
		}
		int focusedIndex = _focusedIndex;
		if (target != focusedIndex && _hasActiveSelection)
		{
			OnDeselect(focusedIndex);
		}
		_focusedIndex = target;
		ScrollIntoView(target / _columnCount, target > focusedIndex);
		_hasActiveSelection = true;
		_pendingSelect = true;
		OnSelect(_focusedIndex);
	}

	private void ScrollIntoView(int rowIndex, bool scrollingDown)
	{
		float scrollPositionForDataIndex = Scroller.GetScrollPositionForDataIndex(rowIndex, EnhancedScroller.CellViewPositionEnum.Before);
		float cellViewSize = GetCellViewSize(Scroller, rowIndex);
		float scrollPosition = Scroller.ScrollPosition;
		float scrollRectSize = Scroller.ScrollRectSize;
		if (!scrollingDown)
		{
			if (scrollPosition > scrollPositionForDataIndex)
			{
				Scroller.SetScrollPositionImmediately(rowIndex == 0 ? 0f : scrollPositionForDataIndex);
			}
		}
		else if (scrollPositionForDataIndex + cellViewSize > scrollPosition + scrollRectSize)
		{
			Scroller.JumpToDataIndex(rowIndex, 1f, 1f, true, EnhancedScroller.TweenType.immediate, 0f, null, EnhancedScroller.LoopJumpDirectionEnum.Closest, false);
			Scroller.SetScrollPositionImmediately(Mathf.Max(0f, Scroller.ScrollPosition));
		}
	}

	protected abstract void OnSelect(int index);

	protected abstract void OnDeselect(int index);
}
