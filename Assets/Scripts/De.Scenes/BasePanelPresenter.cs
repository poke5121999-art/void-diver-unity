using System.Collections.Generic;
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

	private readonly Dictionary<Selectable, NavNode> _navGraph;

	private Selectable _focusedSelectable;

	private Vector2 _heldDir;

	private float _nextNavTime;

	private CompositeDisposable _navDisposable;

	protected CompositeDisposable InputDisposable;

	private readonly SliderPadAccelerator _sliderAccel;

	protected abstract BasePanelModel PanelModel { get; }

	protected bool IsInputEnabled => false;

	protected Selectable FocusedSelectable => null;

	private static float FirstDelay => 0f;

	private static float RepeatRate => 0f;

	private InputAction MoveScrollAction => null;

	private InputAction MovePanelAction => null;

	protected void TriggerOutOfTop()
	{
	}

	protected void TriggerOutOfBottom()
	{
	}

	protected void TriggerOutOfLeft()
	{
	}

	protected void TriggerOutOfRight()
	{
	}

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

	private void SubscribeNav()
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

	private void OnMoveScrollStarted(InputAction.CallbackContext ctx)
	{
	}

	private void OnMoveScrollPerformed(InputAction.CallbackContext ctx)
	{
	}

	private void OnMoveScrollCanceled(InputAction.CallbackContext ctx)
	{
	}

	private void OnMovePanelStarted(InputAction.CallbackContext ctx)
	{
	}

	private void DoNavigatePanelOut(Vector2 dir)
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

	protected virtual void DoSelect(Selectable selectable)
	{
	}

	protected virtual void DoDeselect(Selectable selectable)
	{
	}

	private Selectable FindEntrySelectable()
	{
		return null;
	}

	private Selectable FindClosestSelectable(Vector2 position)
	{
		return null;
	}

	private Selectable FindEdgeSelectable(ENavDir direction)
	{
		return null;
	}

	private bool IsBetter(Vector3 candidate, Vector3 currentBest, ENavDir direction)
	{
		return false;
	}

	private NavNode GetOrCreateNode(Selectable selectable)
	{
		return null;
	}
}
