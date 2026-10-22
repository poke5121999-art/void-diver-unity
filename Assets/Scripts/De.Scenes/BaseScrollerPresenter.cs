using EnhancedUI.EnhancedScroller;
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

	private InputAction MoveScrollAction => null;

	private InputAction MovePanelAction => null;

	private static float FirstDelay => 0f;

	private static float RepeatRate => 0f;

	protected virtual bool IsHorizontal => false;

	protected override void Awake()
	{
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
	}

	protected void RefreshNavCount()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	protected virtual void OnInputEnabled()
	{
	}

	protected virtual void OnInputDisabled()
	{
	}

	private void Update()
	{
	}

	private void OnMoveScrollStarted(InputAction.CallbackContext context)
	{
	}

	private void OnMoveScrollPerformed(InputAction.CallbackContext context)
	{
	}

	private static Vector2Int GetDiscreteDir(Vector2 v)
	{
		return default;
	}

	private void OnMoveScrollCanceled(InputAction.CallbackContext context)
	{
	}

	private void OnMovePanelStarted(InputAction.CallbackContext ctx)
	{
	}

	private void DoNavigatePanelOut(Vector2 dir)
	{
	}

	private void DoNavigate(Vector2 value)
	{
	}

	private void OnNavigateUp()
	{
	}

	private void OnNavigateDown()
	{
	}

	private void OnNavigateLeft()
	{
	}

	private void OnNavigateRight()
	{
	}

	private int FindEntryIndex()
	{
		return 0;
	}

	private int FindDirectionalEntryIndex(ENavDir dir)
	{
		return 0;
	}

	private int FindClosestRow(Vector2 worldPosition, int rowCount)
	{
		return 0;
	}

	private int FindClosestIndex(Vector2 worldPosition)
	{
		return 0;
	}

	private void Navigate(int target)
	{
	}

	private void ScrollIntoView(int rowIndex, bool scrollingDown)
	{
	}

	protected abstract void OnSelect(int index);

	protected abstract void OnDeselect(int index);
}
