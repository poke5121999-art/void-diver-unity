using System.Collections.Generic;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public abstract class BasePanelPresenter : Presenter
{
	private class NavNode
	{
		public Selectable Up;

		public Selectable Down;

		public Selectable Left;

		public Selectable Right;
	}

	[Inject]
	protected GameInputManager InputManager;

	[Inject]
	protected SeriLogger Logger;

	private readonly Dictionary<Selectable, NavNode> _navGraph = new Dictionary<Selectable, NavNode>();

	private Selectable _focusedSelectable;

	private Vector2 _heldDir;

	private float _nextNavTime;

	private CompositeDisposable _navDisposable;

	protected CompositeDisposable InputDisposable;

	private readonly SliderPadAccelerator _sliderAccel = new SliderPadAccelerator();

	protected abstract BasePanelModel PanelModel { get; }

	protected bool IsInputEnabled
	{
		get
		{
			if (PanelModel == null)
			{
				return false;
			}
			return PanelModel.IsInputEnabled;
		}
	}

	protected Selectable FocusedSelectable => _focusedSelectable;

	private static float FirstDelay => TConst.PadNavFirstDelay.FloatValue;

	private static float RepeatRate => TConst.PadNavRepeatRate.FloatValue;

	private InputAction MoveScrollAction => InputManager.UiPadActions.MoveScroll;

	private InputAction MovePanelAction => InputManager.UiPadActions.MovePanel;

	protected void TriggerOutOfTop()
	{
		PanelModel.OutOfTop.OnNext(Unit.Default);
	}

	protected void TriggerOutOfBottom()
	{
		PanelModel.OutOfBottom.OnNext(Unit.Default);
	}

	protected void TriggerOutOfLeft()
	{
		PanelModel.OutOfLeft.OnNext(Unit.Default);
	}

	protected void TriggerOutOfRight()
	{
		PanelModel.OutOfRight.OnNext(Unit.Default);
	}

	protected void ClearNav()
	{
		if (PanelModel != null && PanelModel.IsInputEnabled && _focusedSelectable != null)
		{
			DoDeselect(_focusedSelectable);
		}
		_navGraph.Clear();
		_focusedSelectable = null;
	}

	protected void ConnectNav(Selectable from, ENavDir dir, Selectable to)
	{
		NavNode orCreateNode = GetOrCreateNode(from);
		switch (dir)
		{
		case ENavDir.Up:
			orCreateNode.Up = to;
			break;
		case ENavDir.Down:
			orCreateNode.Down = to;
			break;
		case ENavDir.Left:
			orCreateNode.Left = to;
			break;
		case ENavDir.Right:
			orCreateNode.Right = to;
			break;
		}
	}

	protected void SetNavVertical(IReadOnlyList<Selectable> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			GetOrCreateNode(items[i]);
			if (i > 0)
			{
				ConnectNav(items[i], ENavDir.Up, items[i - 1]);
			}
			if (i < items.Count - 1)
			{
				ConnectNav(items[i], ENavDir.Down, items[i + 1]);
			}
		}
	}

	protected void SetNavHorizontal(IReadOnlyList<Selectable> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			GetOrCreateNode(items[i]);
			if (i > 0)
			{
				ConnectNav(items[i], ENavDir.Left, items[i - 1]);
			}
			if (i < items.Count - 1)
			{
				ConnectNav(items[i], ENavDir.Right, items[i + 1]);
			}
		}
	}

	protected void SetNavGrid(IReadOnlyList<Selectable> items, int columnCount)
	{
		int columns = columnCount > 0 ? columnCount : 1;
		int count = items.Count;
		for (int i = 0; i < count; i++)
		{
			GetOrCreateNode(items[i]);
			if (i >= columns)
			{
				ConnectNav(items[i], ENavDir.Up, items[i - columns]);
			}
			if (i + columns < count)
			{
				ConnectNav(items[i], ENavDir.Down, items[i + columns]);
			}
			if (i % columns != 0)
			{
				ConnectNav(items[i], ENavDir.Left, items[i - 1]);
			}
			if ((i + 1) % columns != 0 && i + 1 < count)
			{
				ConnectNav(items[i], ENavDir.Right, items[i + 1]);
			}
		}
	}

	protected void SetDefaultNav(Selectable selectable)
	{
		_focusedSelectable = selectable;
		if (PanelModel != null && PanelModel.IsInputEnabled && selectable != null)
		{
			DoSelect(selectable);
		}
	}

	protected void SuspendNav()
	{
		_heldDir = Vector2.zero;
		_navDisposable?.Dispose();
		_navDisposable = null;
	}

	protected void ResumeNav()
	{
		SubscribeNav();
		Selectable selectable = FindEntrySelectable();
		if (selectable != null)
		{
			_focusedSelectable = selectable;
			if (PanelModel != null && PanelModel.IsInputEnabled)
			{
				DoSelect(selectable);
			}
		}
	}

	private void SubscribeNav()
	{
		_navDisposable?.Dispose();
		_navDisposable = new CompositeDisposable();
		InputManager.UiPadActions.MoveScroll.OnPressStartAsObservable().Subscribe(OnMoveScrollStarted).AddTo(_navDisposable);
		InputManager.UiPadActions.MoveScroll.OnClickAsObservable().Subscribe(OnMoveScrollPerformed).AddTo(_navDisposable);
		InputManager.UiPadActions.MoveScroll.OnPressEndAsObservable().Subscribe(OnMoveScrollCanceled).AddTo(_navDisposable);
		InputManager.UiPadActions.MovePanel.OnPressStartAsObservable().Subscribe(OnMovePanelStarted).AddTo(_navDisposable);
		InputManager.RxControlDevice.Skip(1).Where((EControlDevice device) => device == EControlDevice.GamePad).Subscribe(delegate
		{
			if (_focusedSelectable != null)
			{
				DoSelect(_focusedSelectable);
			}
		})
			.AddTo(_navDisposable);
	}

	protected virtual void OnDestroy()
	{
		_sliderAccel.Reset();
		_navDisposable?.Dispose();
		InputDisposable?.Dispose();
	}

	protected virtual void OnInputEnabled()
	{
		InputDisposable?.Dispose();
		InputDisposable = new CompositeDisposable();
		if (_navGraph.Count > 0)
		{
			Selectable selectable = FindEntrySelectable();
			if (selectable != null)
			{
				_focusedSelectable = selectable;
			}
		}
		_heldDir = MoveScrollAction.ReadValue<Vector2>();
		_nextNavTime = Time.unscaledTime + FirstDelay;
		SubscribeNav();
		if (_focusedSelectable != null)
		{
			DoSelect(_focusedSelectable);
		}
	}

	protected virtual void OnInputDisabled()
	{
		_heldDir = Vector2.zero;
		_sliderAccel.Reset();
		if (_focusedSelectable != null)
		{
			DoDeselect(_focusedSelectable);
		}
		_navDisposable?.Dispose();
		_navDisposable = null;
		// Ghidra dropped this tail after the write barrier (PIC warning); the disassembly @0x1807f657d disposes and clears it.
		InputDisposable?.Dispose();
		InputDisposable = null;
	}

	private void Update()
	{
		_sliderAccel.Tick(_focusedSelectable, _heldDir, global::InputManager.IsGamePad);
		if (PanelModel != null && PanelModel.IsInputEnabled && _heldDir != Vector2.zero && _nextNavTime <= Time.unscaledTime)
		{
			DoNavigate(_heldDir);
			_nextNavTime = Time.unscaledTime + RepeatRate;
		}
	}

	private void OnMoveScrollStarted(InputAction.CallbackContext ctx)
	{
		Vector2 vector = ctx.ReadValue<Vector2>();
		_heldDir = vector;
		ENavDir dir = ENavDir.None;
		if (vector.y > 0.5f)
		{
			dir = ENavDir.Up;
		}
		else if (vector.y < -0.5f)
		{
			dir = ENavDir.Down;
		}
		else if (vector.x < -0.5f)
		{
			dir = ENavDir.Left;
		}
		else if (vector.x > 0.5f)
		{
			dir = ENavDir.Right;
		}
		if (dir != ENavDir.None)
		{
			Navigate(dir);
		}
		_nextNavTime = Time.unscaledTime + FirstDelay;
	}

	private void OnMoveScrollPerformed(InputAction.CallbackContext ctx)
	{
		Vector2 vector = ctx.ReadValue<Vector2>();
		if (GetDiscreteDir(vector) == GetDiscreteDir(_heldDir))
		{
			_heldDir = vector;
			return;
		}
		_heldDir = vector;
		DoNavigate(vector);
		_nextNavTime = Time.unscaledTime + FirstDelay;
	}

	private void OnMoveScrollCanceled(InputAction.CallbackContext ctx)
	{
		_heldDir = Vector2.zero;
	}

	private void OnMovePanelStarted(InputAction.CallbackContext ctx)
	{
		Gamepad current = Gamepad.current;
		Vector2 dir = current != null ? current.rightStick.ReadValue() : Vector2.zero;
		dir.Normalize();
		if (dir.y > 0.5f)
		{
			TriggerOutOfTop();
		}
		else if (dir.y < -0.5f)
		{
			TriggerOutOfBottom();
		}
		else if (dir.x < -0.5f)
		{
			TriggerOutOfLeft();
		}
		else if (dir.x > 0.5f)
		{
			TriggerOutOfRight();
		}
	}

	private void DoNavigatePanelOut(Vector2 dir)
	{
		if (dir.y > 0.5f)
		{
			PanelModel.OutOfTop.OnNext(Unit.Default);
		}
		else if (dir.y < -0.5f)
		{
			PanelModel.OutOfBottom.OnNext(Unit.Default);
		}
		else if (dir.x < -0.5f)
		{
			PanelModel.OutOfLeft.OnNext(Unit.Default);
		}
		else if (dir.x > 0.5f)
		{
			PanelModel.OutOfRight.OnNext(Unit.Default);
		}
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

	private void DoNavigate(Vector2 dir)
	{
		ENavDir dir2;
		if (dir.y > 0.5f)
		{
			dir2 = ENavDir.Up;
		}
		else if (dir.y < -0.5f)
		{
			dir2 = ENavDir.Down;
		}
		else if (dir.x < -0.5f)
		{
			dir2 = ENavDir.Left;
		}
		else
		{
			if (!(dir.x > 0.5f))
			{
				return;
			}
			dir2 = ENavDir.Right;
		}
		Navigate(dir2);
	}

	private void Navigate(ENavDir dir)
	{
		if (_navGraph.Count == 0 || PanelModel == null || !PanelModel.IsInputEnabled || _focusedSelectable == null || !_navGraph.TryGetValue(_focusedSelectable, out var value))
		{
			return;
		}
		Selectable selectable = null;
		switch (dir)
		{
		case ENavDir.Up:
			selectable = value.Up;
			break;
		case ENavDir.Down:
			selectable = value.Down;
			break;
		case ENavDir.Left:
			selectable = value.Left;
			break;
		case ENavDir.Right:
			selectable = value.Right;
			break;
		}
		if (selectable != null)
		{
			Selectable focusedSelectable = _focusedSelectable;
			_focusedSelectable = selectable;
			DoDeselect(focusedSelectable);
			DoSelect(_focusedSelectable);
			return;
		}
		switch (dir)
		{
		case ENavDir.Up:
			TriggerOutOfTop();
			break;
		case ENavDir.Down:
			TriggerOutOfBottom();
			break;
		case ENavDir.Left:
			TriggerOutOfLeft();
			break;
		case ENavDir.Right:
			TriggerOutOfRight();
			break;
		}
	}

	protected virtual void DoSelect(Selectable selectable)
	{
		if (global::InputManager.IsGamePad)
		{
			InputManager.SelectObject(selectable.gameObject);
		}
	}

	protected virtual void DoDeselect(Selectable selectable)
	{
		if (selectable != null)
		{
			InputManager.DeselectObject(selectable.gameObject);
		}
	}

	private Selectable FindEntrySelectable()
	{
		if (PanelModel == null)
		{
			return null;
		}
		if (InputManager.CurrentSelectedPosition.HasValue && PanelModel.EntryDirection != ENavDir.None)
		{
			return FindClosestSelectable(InputManager.CurrentSelectedPosition.Value);
		}
		if (PanelModel.EntryDirection == ENavDir.None && _focusedSelectable != null && _navGraph.ContainsKey(_focusedSelectable))
		{
			return _focusedSelectable;
		}
		return FindEdgeSelectable(PanelModel.EntryDirection);
	}

	private Selectable FindClosestSelectable(Vector2 position)
	{
		Selectable result = null;
		float num = float.MaxValue;
		foreach (Selectable key in _navGraph.Keys)
		{
			Vector2 vector = (Vector2)key.transform.position - position;
			float sqrMagnitude = vector.x * vector.x + vector.y * vector.y;
			if (sqrMagnitude < num)
			{
				result = key;
				num = sqrMagnitude;
			}
		}
		return result;
	}

	private Selectable FindEdgeSelectable(ENavDir direction)
	{
		Selectable selectable = null;
		foreach (Selectable key in _navGraph.Keys)
		{
			if (selectable == null || IsBetter(key.transform.position, selectable.transform.position, direction))
			{
				selectable = key;
			}
		}
		return selectable;
	}

	private bool IsBetter(Vector3 candidate, Vector3 currentBest, ENavDir direction)
	{
		bool Compare(float a, float b, bool preferGreater)
		{
			if (!preferGreater)
			{
				return a < b;
			}
			return b < a;
		}
		bool TieBreak(float primaryA, float primaryB, float secondaryA, float secondaryB, bool preferGreater, bool preferLess)
		{
			float num = Mathf.Max(Mathf.Abs(primaryA), Mathf.Abs(primaryB));
			float num2 = Mathf.Max(num * 1E-06f, Mathf.Epsilon * 8f);
			if (Mathf.Abs(primaryB - primaryA) < num2)
			{
				if (preferGreater)
				{
					return secondaryB < secondaryA;
				}
				if (preferLess)
				{
					return secondaryA < secondaryB;
				}
			}
			return false;
		}
		switch (direction)
		{
		case ENavDir.Up:
			if (Compare(candidate.y, currentBest.y, preferGreater: false))
			{
				return true;
			}
			return TieBreak(candidate.y, currentBest.y, candidate.x, currentBest.x, preferGreater: false, preferLess: true);
		case ENavDir.Down:
			if (Compare(candidate.y, currentBest.y, preferGreater: true))
			{
				return true;
			}
			return TieBreak(candidate.y, currentBest.y, candidate.x, currentBest.x, preferGreater: false, preferLess: true);
		case ENavDir.Left:
		{
			bool flag = Compare(candidate.x, currentBest.x, preferGreater: true);
			if (flag)
			{
				return true;
			}
			return TieBreak(candidate.x, currentBest.x, candidate.y, currentBest.y, preferGreater: true, flag);
		}
		case ENavDir.Right:
		{
			bool flag2 = Compare(candidate.x, currentBest.x, preferGreater: false);
			if (flag2)
			{
				return true;
			}
			return TieBreak(candidate.x, currentBest.x, candidate.y, currentBest.y, preferGreater: true, flag2);
		}
		default:
			return false;
		}
	}

	private NavNode GetOrCreateNode(Selectable selectable)
	{
		if (!_navGraph.TryGetValue(selectable, out var value))
		{
			value = new NavNode();
			_navGraph[selectable] = value;
		}
		return value;
	}
}
