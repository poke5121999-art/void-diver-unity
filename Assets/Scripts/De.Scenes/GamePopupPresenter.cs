using System.Collections.Generic;
using GameCommon.Table;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class GamePopupPresenter : BasePopupPresenter
{
	private class NavNode
	{
		public Selectable Up;

		public Selectable Down;

		public Selectable Left;

		public Selectable Right;
	}

	private readonly Dictionary<Selectable, NavNode> _navGraph = new Dictionary<Selectable, NavNode>();

	private Selectable _focusedSelectable;

	private Vector2 _heldDir;

	private float _nextNavTime;

	private CompositeDisposable _navDisposable;

	private readonly SliderPadAccelerator _sliderAccel = new SliderPadAccelerator();

	private static float FirstDelay => TConst.PadNavFirstDelay.FloatValue;

	private static float RepeatRate => TConst.PadNavRepeatRate.FloatValue;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputAction MoveScrollAction => GameInputManager.UiPadActions.MoveScroll;

	protected Selectable FocusedSelectable => _focusedSelectable;

	protected void ClearNav()
	{
		if (IsInputEnable && _focusedSelectable != null)
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
		if (IsInputEnable && selectable != null)
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
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		_heldDir = MoveScrollAction.ReadValue<Vector2>();
		_nextNavTime = Time.unscaledTime + FirstDelay;
		SubscribeNav();
		if (_focusedSelectable != null)
		{
			DoSelect(_focusedSelectable);
		}
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_heldDir = Vector2.zero;
		_sliderAccel.Reset();
		if (_focusedSelectable != null)
		{
			DoDeselect(_focusedSelectable);
		}
		_navDisposable?.Dispose();
		_navDisposable = null;
	}

	protected virtual void OnDestroy()
	{
		_sliderAccel.Reset();
		_navDisposable?.Dispose();
	}

	protected virtual void Update()
	{
		_sliderAccel.Tick(_focusedSelectable, _heldDir, global::InputManager.IsGamePad);
		if (IsInputEnable && _heldDir != Vector2.zero && _nextNavTime <= Time.unscaledTime)
		{
			DoNavigate(_heldDir);
			_nextNavTime = Time.unscaledTime + RepeatRate;
		}
	}

	private void SubscribeNav()
	{
		_navDisposable?.Dispose();
		_navDisposable = new CompositeDisposable();
		MoveScrollAction.OnPressStartAsObservable().Subscribe(OnMoveScrollStarted).AddTo(_navDisposable);
		MoveScrollAction.OnClickAsObservable().Subscribe(OnMoveScrollPerformed).AddTo(_navDisposable);
		MoveScrollAction.OnPressEndAsObservable().Subscribe(OnMoveScrollCanceled).AddTo(_navDisposable);
		InputManager.RxControlDevice.Skip(1).Where((EControlDevice device) => device == EControlDevice.GamePad).Subscribe(delegate
		{
			if (_focusedSelectable != null)
			{
				DoSelect(_focusedSelectable);
			}
		})
			.AddTo(_navDisposable);
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
		if (dir.y > 0.5f)
		{
			Navigate(ENavDir.Up);
		}
		else if (dir.y < -0.5f)
		{
			Navigate(ENavDir.Down);
		}
		else if (dir.x < -0.5f)
		{
			Navigate(ENavDir.Left);
		}
		else if (dir.x > 0.5f)
		{
			Navigate(ENavDir.Right);
		}
	}

	private void Navigate(ENavDir dir)
	{
		if (!IsInputEnable || _focusedSelectable == null || !_navGraph.TryGetValue(_focusedSelectable, out var value))
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
		if (selectable == null)
		{
			OnNavOutOfBounds(dir);
			return;
		}
		Selectable focusedSelectable = _focusedSelectable;
		_focusedSelectable = selectable;
		DoDeselect(focusedSelectable);
		DoSelect(_focusedSelectable);
	}

	protected virtual void OnNavOutOfBounds(ENavDir dir)
	{
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
		InputManager.DeselectObject(selectable.gameObject);
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
