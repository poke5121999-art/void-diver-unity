using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class SelectionPopupPresenter : GamePopupPresenter
{
	[View]
	private SelectionPopupView _view;

	[InjectModel]
	private SelectionPopupModel _model;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private CameraManager _cameraManager;

	private SelectionSlotScrollerModel _scrollerModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		if (_model.SelectionDatas != null)
		{
			_scrollerModel = new SelectionSlotScrollerModel
			{
				SelectionDatas = _model.SelectionDatas,
				EntryDirection = ENavDir.Down
			};
			_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(OpenDisposable);
			_view.Scroller.Inject(_scrollerModel);
			AlignToPosition(_model.Position);
		}
		else
		{
			Logger.Error("No selections provided for Popup");
			Close();
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close();
			}).AddTo(InputDisposable);
			_scrollerModel.RxIsInputEnabled.Value = true;
			_scrollerModel.EntryDirection = ENavDir.None;
		}
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void AlignToPosition(Vector3 position)
	{
		RectTransform component = _view.Pivot.GetComponent<RectTransform>();
		Vector3 screenPoint = _cameraManager.MainCamera.WorldToScreenPoint(position);
		Canvas overlayCanvas = _popupManager.OverlayCanvas;
		RectTransform rect = overlayCanvas.transform as RectTransform;
		Camera cam = ((overlayCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : overlayCanvas.worldCamera);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out var localPoint);
		component.anchoredPosition = localPoint;
	}

	private void OnSlotClick(SelectionSlotModel slotModel)
	{
		if (slotModel.Index >= 0 && slotModel.Index < _model.SelectionDatas.Count)
		{
			Close(slotModel.Index);
		}
	}
}
