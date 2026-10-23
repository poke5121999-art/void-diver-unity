using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SelectPlayerPopupPresenter : GamePopupPresenter
{
	[View]
	private SelectPlayerPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	private readonly List<PlayerSlotModel> _slotModels = new List<PlayerSlotModel>();

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		for (int i = 0; i < _view.SaveSlots.Length; i++)
		{
			PlayerSlotModel playerSlotModel = new PlayerSlotModel
			{
				PlayerSlotIndex = i
			};
			playerSlotModel.OnClick.Subscribe(OnSlotClick).AddTo(this);
			playerSlotModel.OnDeleteClick.Subscribe(OnSlotDeleteClick).AddTo(this);
			_view.SaveSlots[i].GetComponent<PlayerSlotPresenter>().Inject(playerSlotModel);
			_slotModels.Add(playerSlotModel);
		}
		_view.Restore_Button.OnClickWithSfx().Subscribe(delegate
		{
			OnRestoreClick();
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		List<Button> list = _view.SaveSlots.Select((GameObject _) => _.GetComponent<Button>()).ToList();
		SetNavVertical(list);
		SetDefaultNav(list[_playerManager.PlayerSlotIndex]);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		UiPadActions.Delete.OnClickWithSfx().Subscribe(delegate
		{
			OnDeleteClick();
		}).AddTo(InputDisposable);
		UiPadActions.Restore.OnClickWithSfx().Subscribe(delegate
		{
			OnRestoreClick();
		}).AddTo(InputDisposable);
	}

	private void OnSlotClick(PlayerSlotModel model)
	{
		_playerManager.SelectPlayerSlot(model.PlayerSlotIndex);
	}

	private void OnSlotDeleteClick(PlayerSlotModel model)
	{
		ExecuteDeleteAsync(model).Forget();
	}

	private void OnDeleteClick()
	{
		for (int i = 0; i < _view.SaveSlots.Length; i++)
		{
			if (_view.SaveSlots[i].GetComponent<PlayerSlotView>().Button == FocusedSelectable)
			{
				ExecuteDeleteAsync(_slotModels[i]).Forget();
				return;
			}
		}
	}

	private void OnRestoreClick()
	{
		_popupManager.Open<RestorePlayerPopupPresenter>();
	}

	private async UniTask ExecuteDeleteAsync(PlayerSlotModel model)
	{
		PlayerSlot playerSlot = _playerManager.PlayerSlots[model.PlayerSlotIndex];
		if (string.IsNullOrEmpty(playerSlot.PlayerName))
		{
			_toastManager.NoticeFail(TLocalizedText.CannotDeleteEmptySlot.LocalText);
		}
		else if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmDeletePlayerSlotFormat.Format(model.PlayerSlotIndex + 1, playerSlot.PlayerName)))
		{
			await _playerManager.DeletePlayer(model.PlayerSlotIndex);
			await _playerManager.SaveAccountAsync();
		}
	}
}
