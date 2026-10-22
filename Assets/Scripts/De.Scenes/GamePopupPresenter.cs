using System.Collections.Generic;
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

	private readonly Dictionary<Selectable, NavNode> _navGraph;

	private Selectable _focusedSelectable;

	private Vector2 _heldDir;

	private float _nextNavTime;

	private CompositeDisposable _navDisposable;

	private readonly SliderPadAccelerator _sliderAccel;

	private static float FirstDelay => 0f;

	private static float RepeatRate => 0f;

	private GameInputManager GameInputManager => null;

	private InputAction MoveScrollAction => null;

	protected Selectable FocusedSelectable => null;

	protected void ClearNav()
	{
	}

	protected void ConnectNav(Selectable from, ENavDir dir, Selectable to)
	{
	}

	protected void SetNavVertical(IReadOnlyList<Selectable> items)
	{
	}

	protected void SetNavHorizontal(IReadOnlyList<Selectable> items)
	{
	}

	protected void SetNavGrid(IReadOnlyList<Selectable> items, int columnCount)
	{
	}

	protected void SetDefaultNav(Selectable selectable)
	{
	}

	protected void SuspendNav()
	{
	}

	protected void ResumeNav()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	protected virtual void Update()
	{
	}

	private void SubscribeNav()
	{
	}

	private void OnMoveScrollStarted(InputAction.CallbackContext ctx)
	{
	}

	private void OnMoveScrollPerformed(InputAction.CallbackContext ctx)
	{
	}

	private void OnMoveScrollCanceled(InputAction.CallbackContext ctx)
	{
	}

	private static Vector2Int GetDiscreteDir(Vector2 v)
	{
		return default;
	}

	private void DoNavigate(Vector2 dir)
	{
	}

	private void Navigate(ENavDir dir)
	{
	}

	protected virtual void OnNavOutOfBounds(ENavDir dir)
	{
	}

	protected virtual void DoSelect(Selectable selectable)
	{
	}

	protected virtual void DoDeselect(Selectable selectable)
	{
	}

	private NavNode GetOrCreateNode(Selectable selectable)
	{
		return null;
	}
}
