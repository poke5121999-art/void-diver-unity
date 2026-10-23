using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class RestorePlayerPopupPresenter : GamePopupPresenter
{
	[View]
	private RestorePlayerPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	private PlayerSaveSlotScrollerModel _saveScrollerModel;

	private PlayerRestoreSlotScrollerModel _restoreScrollerModel;

	private PlayerSaveSlotModel _selectedSaveSlot;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_saveScrollerModel = new PlayerSaveSlotScrollerModel();
		_saveScrollerModel.OnSlotClick.Subscribe(OnSaveSlotClick).AddTo(this);
		_view.SaveSlotScroller.Inject(_saveScrollerModel);
		_restoreScrollerModel = new PlayerRestoreSlotScrollerModel();
		_restoreScrollerModel.OnSlotClick.Subscribe(OnRestoreSlotClick).AddTo(this);
		_view.RestoreSlotScroller.Inject(_restoreScrollerModel);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		_saveScrollerModel.RxIsInputEnabled.Value = _selectedSaveSlot == null;
		_restoreScrollerModel.RxIsInputEnabled.Value = _selectedSaveSlot != null;
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_saveScrollerModel.RxIsInputEnabled.Value = false;
		_restoreScrollerModel.RxIsInputEnabled.Value = false;
	}

	private void OnSaveSlotClick(PlayerSaveSlotModel slotModel)
	{
		if (_selectedSaveSlot == slotModel)
		{
			return;
		}
		if (_selectedSaveSlot != null)
		{
			_selectedSaveSlot.RxIsSelected.Value = false;
		}
		_selectedSaveSlot = slotModel;
		_selectedSaveSlot.RxIsSelected.Value = true;
		if (IsInputEnable)
		{
			_saveScrollerModel.RxIsInputEnabled.Value = false;
			_restoreScrollerModel.RxIsInputEnabled.Value = true;
		}
	}

	private void OnRestoreSlotClick(PlayerRestoreSlotModel slotModel)
	{
		if (_selectedSaveSlot != null)
		{
			ResetPlayerAsync().Forget();
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.SelectTargetSaveSlotFirst.LocalText);
		}
		async UniTask ResetPlayerAsync()
		{
			if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmRestoreSaveData.LocalText))
			{
				if (!(await _playerManager.RestoreAsync(_selectedSaveSlot.PlayerSlotIndex, slotModel.SaveTimeTicks)))
				{
					_toastManager.NoticeFail(TLocalizedText.RestoreSaveDataFailed.LocalText);
				}
				else
				{
					_toastManager.Notice(TLocalizedText.RestoreSaveDataCompleted.LocalText, "Talent_Success");
					OnBack();
				}
			}
		}
	}

	private void OnBack()
	{
		if (_selectedSaveSlot == null)
		{
			Close();
			return;
		}
		_selectedSaveSlot.RxIsSelected.Value = false;
		_selectedSaveSlot = null;
		if (IsInputEnable)
		{
			_saveScrollerModel.RxIsInputEnabled.Value = true;
			_restoreScrollerModel.RxIsInputEnabled.Value = false;
		}
	}
}
