using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class RoomSettingPopupPresenter : GamePopupPresenter
{
	[View]
	private RoomSettingPopupView _view;

	[InjectModel]
	private RoomSettingPopupModel _model;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Cancel_Btn.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.Confirm_Btn.OnClickWithSfx().Subscribe(delegate
		{
			OnConfirmButtonClick();
		}).AddTo(this);
		_view.LoungeDifficulty_NextButton.gameObject.SetActive(_model.IsCreate);
		_view.LoungeDifficulty_PrevButton.gameObject.SetActive(_model.IsCreate);
		_view.LoungeDifficulty_PadButton.gameObject.SetActive(_model.IsCreate);
		_view.LoungeDifficulty_NextButton.OnClickWithSfx().Subscribe(delegate
		{
			OnClickNextDifficulty();
		}).AddTo(this);
		_view.LoungeDifficulty_PrevButton.OnClickWithSfx().Subscribe(delegate
		{
			OnClickPrevDifficulty();
		}).AddTo(this);
		_view.LoungeLock_NextButton.OnClickWithSfx().Subscribe(delegate
		{
			OnClickNextPublishType();
		}).AddTo(this);
		_view.LoungeLock_PrevButton.OnClickWithSfx().Subscribe(delegate
		{
			OnClickPrevPublishType();
		}).AddTo(this);
		_view.Title.text = (_model.IsCreate ? TLocalizedText.URoomCreationPopupTitleText.LocalText : TLocalizedText.RoomSetting.LocalText);
		_view.InputFieldTitle.text = _model.RoomTitle;
		_view.InputFieldTitle_TextArea_Placeholder.text = TLocalizedText.DefaultLobbyNamePlaceholder.LocalText;
		_view.LoungeDifficulty_Text.color = _model.RoomDifficulty.GetColor();
		_view.LoungeDifficulty_Text.text = ((System.Enum)_model.RoomDifficulty).ToLocalText();
		_view.LoungeLock_Text.text = ((System.Enum)_model.PublishType).ToLocalText();
		// the value stored into InputFieldController._description is **(0x1864889d0 + 0xb8) = string.Empty (machine code checked)
		_view.InputFieldTitle.GetComponent<InputFieldController>().SetDescription(string.Empty);
		_view.InputFieldTitle.onEndEdit.AsObservable().Subscribe(delegate(string text)
		{
			_model.RoomTitle = text;
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		List<Selectable> list = new List<Selectable>();
		if (_model.IsCreate)
		{
			list.Add(_view.LoungeDifficulty_PadButton);
		}
		list.Add(_view.LoungeLock_PadButton);
		list.Add(_view.InputFieldTitle);
		list.Add(_view.Confirm_Btn);
		SetNavVertical(list);
		ConnectNav(_view.Confirm_Btn, ENavDir.Right, _view.Cancel_Btn);
		ConnectNav(_view.Cancel_Btn, ENavDir.Up, _view.InputFieldTitle);
		ConnectNav(_view.Cancel_Btn, ENavDir.Left, _view.Confirm_Btn);
		SetDefaultNav(_view.Confirm_Btn);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		UiPadActions.OptionLeft.OnClickWithSfx().Subscribe(delegate
		{
			OnOptionLeftClick();
		}).AddTo(InputDisposable);
		UiPadActions.OptionRight.OnClickWithSfx().Subscribe(delegate
		{
			OnOptionRightClick();
		}).AddTo(InputDisposable);
	}

	private void OnOptionLeftClick()
	{
		if (InputManager.GetSelectedObject() == _view.LoungeDifficulty_PadButton.gameObject)
		{
			OnClickPrevDifficulty();
		}
		else if (InputManager.GetSelectedObject() == _view.LoungeLock_PadButton.gameObject)
		{
			OnClickPrevPublishType();
		}
	}

	private void OnOptionRightClick()
	{
		if (InputManager.GetSelectedObject() == _view.LoungeDifficulty_PadButton.gameObject)
		{
			OnClickNextDifficulty();
		}
		else if (InputManager.GetSelectedObject() == _view.LoungeLock_PadButton.gameObject)
		{
			OnClickNextPublishType();
		}
	}

	private void OnConfirmButtonClick()
	{
		if (!_appManager.IsOnline())
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
		}
		else if (string.IsNullOrEmpty(_model.RoomTitle))
		{
			_toastManager.Notice(TLocalizedText.LoungeNameEmptyMessage.LocalText);
		}
		else if (!UIInputUtility.ValidateText(_model.RoomTitle))
		{
			_toastManager.NoticeFail(TLocalizedText.ContainsProhibitedWords.LocalText);
		}
		else
		{
			_soundManager.PlaySfx("Lounge_Create");
			Close(new RoomSettingPopupResult
			{
				RoomTitle = _model.RoomTitle,
				PlayerCountMax = TConst.RoomPlayerCountMax.IntValue,
				RoomDifficulty = _model.RoomDifficulty,
				PublishType = _model.PublishType
			});
		}
	}

	private void OnCancel()
	{
		Close();
	}

	private void OnClickNextDifficulty()
	{
		_model.RoomDifficulty = _model.RoomDifficulty.GetNextDifficulty();
		_view.LoungeDifficulty_Text.color = _model.RoomDifficulty.GetColor();
		_view.LoungeDifficulty_Text.text = ((System.Enum)_model.RoomDifficulty).ToLocalText();
	}

	private void OnClickPrevDifficulty()
	{
		_model.RoomDifficulty = _model.RoomDifficulty.GetPrevDifficulty();
		_view.LoungeDifficulty_Text.color = _model.RoomDifficulty.GetColor();
		_view.LoungeDifficulty_Text.text = ((System.Enum)_model.RoomDifficulty).ToLocalText();
	}

	private void OnClickNextPublishType()
	{
		_model.PublishType = _model.PublishType.GetNextPublishType();
		_view.LoungeLock_Text.text = ((System.Enum)_model.PublishType).ToLocalText();
	}

	private void OnClickPrevPublishType()
	{
		_model.PublishType = _model.PublishType.GetPrevPublishType();
		_view.LoungeLock_Text.text = ((System.Enum)_model.PublishType).ToLocalText();
	}
}
