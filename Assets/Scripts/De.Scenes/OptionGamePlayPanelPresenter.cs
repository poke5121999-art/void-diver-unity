using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class OptionGamePlayPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private OptionGamePlayPanelModel _model;

	[View]
	private OptionGamePlayPanelView _view;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(OptionGamePlayPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitAimSupport();
		InitPadSensitivity();
		InitAutoAttack();
		InitLockMouse();
		InitRstickAttack();
		InitRun();
		InitAimLine();
		InitLanguage();
		InitGamePadNav();
		_view.ResetButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnResetClick();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnDisable()
	{
		_gamePlaySettings.Flush();
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> list = _view.AimSupport_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list2 = _view.AutoAttack_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list3 = _view.LockMouse_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list4 = _view.RstickAttack_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list5 = _view.Run_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list6 = _view.AimLine_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> item = new List<Selectable> { _view.Language_Button };
		List<Selectable> item2 = new List<Selectable> { _view.PadSensitivity_Slider };
		List<Selectable> item3 = new List<Selectable> { _view.ResetButton };
		List<List<Selectable>> list7 = new List<List<Selectable>>();
		list7.Add(item);
		list7.Add(list2);
		list7.Add(list5);
		list7.Add(list6);
		list7.Add(list3);
		list7.Add(list4);
		list7.Add(list);
		list7.Add(item2);
		list7.Add(item3);
		foreach (List<Selectable> item4 in list7)
		{
			SetNavHorizontal(item4);
		}
		for (int i = 0; i < list7.Count - 1; i++)
		{
			List<Selectable> list8 = list7[i];
			List<Selectable> list9 = list7[i + 1];
			for (int j = 0; j < list8.Count; j++)
			{
				ConnectNav(list8[j], ENavDir.Down, list9[Math.Max(j + (list9.Count - list8.Count), 0)]);
			}
			for (int k = 0; k < list9.Count; k++)
			{
				ConnectNav(list9[k], ENavDir.Up, list8[Math.Max(k + (list8.Count - list9.Count), 0)]);
			}
		}
	}

	private void InitAimSupport()
	{
		for (int i = 0; i < _view.AimSupport_Toggles.Length; i++)
		{
			bool aimSupportOn = i == 0;
			Toggle component = _view.AimSupport_Toggles[i].GetComponent<Toggle>();
			if (aimSupportOn == _gamePlaySettings.AimSupportOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetAimSupportOn(aimSupportOn);
			}).AddTo(_disposable);
		}
	}

	private void SetAimSupportOn(bool aimSupportOn)
	{
		if (_gamePlaySettings.AimSupportOn != aimSupportOn)
		{
			_gamePlaySettings.SetAimSupportOn(aimSupportOn);
		}
	}

	private void InitPadSensitivity()
	{
		_view.PadSensitivity_Slider.wholeNumbers = true;
		_view.PadSensitivity_Slider.minValue = 1f;
		_view.PadSensitivity_Slider.maxValue = 100f;
		_view.PadSensitivity_Slider.SetValueWithoutNotify(_gamePlaySettings.PadSensitivity);
		UpdatePadSensitivityText(_gamePlaySettings.PadSensitivity);
		_view.PadSensitivity_Slider.OnValueChangedAsObservable().Subscribe(OnPadSensitivityChanged).AddTo(_disposable);
	}

	private void OnPadSensitivityChanged(float value)
	{
		UpdatePadSensitivityText((int)value);
		_gamePlaySettings.SetPadSensitivity((int)value);
	}

	private void UpdatePadSensitivityText(int sensitivity)
	{
		_view.PadSensitivity_Text.text = string.Format("{0}%", sensitivity);
	}

	private void InitAutoAttack()
	{
		for (int i = 0; i < _view.AutoAttack_Toggles.Length; i++)
		{
			bool autoAttackOn = i == 0;
			Toggle component = _view.AutoAttack_Toggles[i].GetComponent<Toggle>();
			if (autoAttackOn == _gamePlaySettings.AutoAttackOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetAutoAttackOn(autoAttackOn);
			}).AddTo(_disposable);
		}
	}

	private void SetAutoAttackOn(bool autoAttackOn)
	{
		if (_gamePlaySettings.AutoAttackOn != autoAttackOn)
		{
			_gamePlaySettings.SetAutoAttackOn(autoAttackOn);
		}
	}

	private void InitLockMouse()
	{
		for (int i = 0; i < _view.LockMouse_Toggles.Length; i++)
		{
			bool lockMouseOn = i == 0;
			Toggle component = _view.LockMouse_Toggles[i].GetComponent<Toggle>();
			if (lockMouseOn == _gamePlaySettings.LockMouseOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetLockMouseOn(lockMouseOn);
			}).AddTo(_disposable);
		}
	}

	private void SetLockMouseOn(bool lockMouseOn)
	{
		if (_gamePlaySettings.LockMouseOn != lockMouseOn)
		{
			_gamePlaySettings.SetLockMouseOn(lockMouseOn);
		}
	}

	private void InitRstickAttack()
	{
		for (int i = 0; i < _view.RstickAttack_Toggles.Length; i++)
		{
			bool rstickAttackOn = i == 0;
			Toggle component = _view.RstickAttack_Toggles[i].GetComponent<Toggle>();
			if (rstickAttackOn == _gamePlaySettings.RstickAttackOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetRstickAttackOn(rstickAttackOn);
			}).AddTo(_disposable);
		}
	}

	private void SetRstickAttackOn(bool rstickAttackOn)
	{
		if (_gamePlaySettings.RstickAttackOn != rstickAttackOn)
		{
			_gamePlaySettings.SetRstickAttackOn(rstickAttackOn);
		}
	}

	private void InitRun()
	{
		for (int i = 0; i < _view.Run_Toggles.Length; i++)
		{
			bool runToggleOn = i == 0;
			Toggle component = _view.Run_Toggles[i].GetComponent<Toggle>();
			if (runToggleOn == _gamePlaySettings.RunToggleOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetRunToggleOn(runToggleOn);
			}).AddTo(_disposable);
		}
	}

	private void SetRunToggleOn(bool runToggleOn)
	{
		if (_gamePlaySettings.RunToggleOn != runToggleOn)
		{
			_gamePlaySettings.SetRunToggleOn(runToggleOn);
		}
	}

	private void InitAimLine()
	{
		for (int i = 0; i < _view.AimLine_Toggles.Length; i++)
		{
			bool aimLineOn = i == 0;
			Toggle component = _view.AimLine_Toggles[i].GetComponent<Toggle>();
			if (aimLineOn == _gamePlaySettings.AimLineOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetAimLineOn(aimLineOn);
			}).AddTo(_disposable);
		}
	}

	private void SetAimLineOn(bool aimLineOn)
	{
		if (_gamePlaySettings.AimLineOn != aimLineOn)
		{
			_gamePlaySettings.SetAimLineOn(aimLineOn);
		}
	}

	private void InitLanguage()
	{
		_view.Language_Button_TextTitle.text = _gamePlaySettings.Language.ToLocalText();
		_view.Language_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnLanguageButtonClickAsync().Forget();
		}).AddTo(_disposable);
	}

	private async UniTask OnLanguageButtonClickAsync()
	{
		if (_gameManager.IsParticipant)
		{
			_toastManager.NoticeFail(TLocalizedText.CannotChangeLanguageWhilePlaying.LocalText);
			return;
		}
		ELanguage[] languages = Enum.GetValues(typeof(ELanguage)).Cast<ELanguage>().Where((ELanguage _) => _ != ELanguage.None)
			.ToArray();
		string[] options = languages.Select((ELanguage _) => _.ToLocalText()).ToArray();
		OptionListPopupPresenter optionListPopupPresenter = await _popupManager.OpenAsync<OptionListPopupPresenter>(new OptionListPopupModel
		{
			Title = TLocalizedText.Language.LocalText,
			Options = options,
			SelectedIndex = (int)(_gamePlaySettings.Language - 1)
		});
		object obj = await optionListPopupPresenter.Task;
		if (obj is int)
		{
			ELanguage eLanguage = languages[(int)obj];
			if (_gamePlaySettings.Language != eLanguage)
			{
				_gamePlaySettings.SetLanguage(eLanguage);
				_view.Language_Button_TextTitle.text = eLanguage.ToLocalText();
			}
		}
	}

	private void OnResetClick()
	{
		_gamePlaySettings.Reset();
		_view.Language_Button_TextTitle.text = _gamePlaySettings.Language.ToLocalText();
		SyncToggle(_view.AimSupport_Toggles, _gamePlaySettings.AimSupportOn ? 0 : 1);
		SyncToggle(_view.AutoAttack_Toggles, _gamePlaySettings.AutoAttackOn ? 0 : 1);
		SyncToggle(_view.LockMouse_Toggles, _gamePlaySettings.LockMouseOn ? 0 : 1);
		SyncToggle(_view.RstickAttack_Toggles, _gamePlaySettings.RstickAttackOn ? 0 : 1);
		SyncToggle(_view.Run_Toggles, _gamePlaySettings.RunToggleOn ? 0 : 1);
		SyncToggle(_view.AimLine_Toggles, _gamePlaySettings.AimLineOn ? 0 : 1);
		_view.PadSensitivity_Slider.SetValueWithoutNotify(_gamePlaySettings.PadSensitivity);
		UpdatePadSensitivityText(_gamePlaySettings.PadSensitivity);
	}

	private static void SyncToggle(GameObject[] toggles, int selectedIndex)
	{
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].TryGetComponent<ToggleController>(out var component))
			{
				component.SetIsOnWithoutNotify(i == selectedIndex);
			}
		}
	}
}
