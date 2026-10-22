using System.Collections.Generic;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class OptionSoundPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private OptionSoundPanelModel _model;

	[View]
	private OptionSoundPanelView _view;

	[Inject]
	private SoundSettingService _soundSettings;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	private int VolumeLevelMax => 100;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(OptionSoundPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitMasterVolume();
		InitBgmVolume();
		InitSfxVolume();
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
		_soundSettings.Flush();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		InputManager.UiPadActions.MuteToggle.OnClickAsObservable().Subscribe(delegate
		{
			OnMuteTogglePressed();
		}).AddTo(InputDisposable);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> items = new List<Selectable>
		{
			_view.TotalVolume_Volume_Slider,
			_view.Bgm_Volume_Slider,
			_view.Sfx_Volume_Slider,
			_view.ResetButton
		};
		SetNavVertical(items);
	}

	private void InitMasterVolume()
	{
		_view.TotalVolume_Volume_OnOffToggle.isOn = _soundSettings.MasterVolumeOn;
		_view.TotalVolume_Volume_Slider.maxValue = 100f;
		_soundSettings.RxMasterVolumeLevel.Subscribe(OnMasterVolumeLevelUpdated).AddTo(_disposable);
		_view.TotalVolume_Volume_OnOffToggle.OnValueChangedWithSfx().Subscribe(SetMasterVolumeOn).AddTo(_disposable);
		_view.TotalVolume_Volume_Slider.OnValueChangedWithSfx().Subscribe(delegate(float value)
		{
			SetMasterVolumeLevel((int)value);
		}).AddTo(_disposable);
	}

	private void OnMasterVolumeLevelUpdated(int value)
	{
		_view.TotalVolume_Volume_Text.text = string.Format("{0}", value);
		_view.TotalVolume_Volume_Slider.SetValueWithoutNotify(value);
	}

	private void SetMasterVolumeOn(bool value)
	{
		_soundSettings.SetMasterVolumeOn(value);
	}

	private void SetMasterVolumeLevel(int value)
	{
		if (value > VolumeLevelMax)
		{
			_toastManager.NoticeFail(TLocalizedText.WarningMaxVolume.LocalText);
		}
		else if (value < 0)
		{
			_toastManager.NoticeFail(TLocalizedText.WarningMinVolume.LocalText);
		}
		else
		{
			_soundSettings.SetMasterVolumeLevel(value);
		}
	}

	private void InitBgmVolume()
	{
		_view.Bgm_Volume_OnOffToggle.isOn = _soundSettings.BgmVolumeOn;
		_view.Bgm_Volume_Slider.maxValue = 100f;
		_soundSettings.RxBgmVolumeLevel.Subscribe(OnBgmVolumeLevelUpdated).AddTo(_disposable);
		_view.Bgm_Volume_OnOffToggle.OnValueChangedWithSfx().Subscribe(SetBgmVolumeOn).AddTo(_disposable);
		_view.Bgm_Volume_Slider.OnValueChangedWithSfx().Subscribe(delegate(float value)
		{
			SetBgmVolumeLevel((int)value);
		}).AddTo(_disposable);
	}

	private void OnBgmVolumeLevelUpdated(int value)
	{
		_view.Bgm_Volume_Text.text = string.Format("{0}", value);
		_view.Bgm_Volume_Slider.SetValueWithoutNotify(value);
	}

	private void SetBgmVolumeOn(bool value)
	{
		_soundSettings.SetBgmVolumeOn(value);
	}

	private void SetBgmVolumeLevel(int value)
	{
		if (value > VolumeLevelMax)
		{
			_toastManager.NoticeFail(TLocalizedText.WarningMaxVolume.LocalText);
		}
		else if (value < 0)
		{
			_toastManager.NoticeFail(TLocalizedText.WarningMinVolume.LocalText);
		}
		else
		{
			_soundSettings.SetBgmVolumeLevel(value);
		}
	}

	private void InitSfxVolume()
	{
		_view.Sfx_Volume_OnOffToggle.isOn = _soundSettings.SfxVolumeOn;
		_view.Sfx_Volume_Slider.maxValue = 100f;
		_soundSettings.RxSfxVolumeLevel.Subscribe(OnSfxVolumeLevelUpdated).AddTo(_disposable);
		_view.Sfx_Volume_OnOffToggle.OnValueChangedWithSfx().Subscribe(SetSfxVolumeOn).AddTo(_disposable);
		_view.Sfx_Volume_Slider.OnValueChangedWithSfx().Subscribe(delegate(float value)
		{
			SetSfxVolumeLevel((int)value);
		}).AddTo(_disposable);
	}

	private void OnSfxVolumeLevelUpdated(int value)
	{
		_view.Sfx_Volume_Text.text = string.Format("{0}", value);
		_view.Sfx_Volume_Slider.SetValueWithoutNotify(value);
	}

	private void SetSfxVolumeOn(bool value)
	{
		_soundSettings.SetSfxVolumeOn(value);
	}

	private void SetSfxVolumeLevel(int value)
	{
		if (value > VolumeLevelMax)
		{
			_toastManager.NoticeFail(TLocalizedText.WarningMaxVolume.LocalText);
		}
		else if (value < 0)
		{
			_toastManager.NoticeFail(TLocalizedText.WarningMinVolume.LocalText);
		}
		else
		{
			_soundSettings.SetSfxVolumeLevel(value);
		}
	}

	private void OnResetClick()
	{
		_soundSettings.Reset();
		SyncToggle(_view.TotalVolume_Volume_OnOffToggle, _soundSettings.MasterVolumeOn);
		SyncToggle(_view.Bgm_Volume_OnOffToggle, _soundSettings.BgmVolumeOn);
		SyncToggle(_view.Sfx_Volume_OnOffToggle, _soundSettings.SfxVolumeOn);
	}

	private static void SyncToggle(Toggle toggle, bool isOn)
	{
		if (toggle.TryGetComponent<ToggleController>(out var component))
		{
			component.SetIsOnWithoutNotify(isOn);
		}
		else
		{
			toggle.SetIsOnWithoutNotify(isOn);
		}
	}

	private void OnMuteTogglePressed()
	{
		Toggle focusedVolumeToggle = GetFocusedVolumeToggle();
		if (focusedVolumeToggle != null)
		{
			focusedVolumeToggle.isOn = !focusedVolumeToggle.isOn;
		}
		Toggle GetFocusedVolumeToggle()
		{
			Selectable focusedSelectable = FocusedSelectable;
			if (focusedSelectable == _view.TotalVolume_Volume_Slider)
			{
				return _view.TotalVolume_Volume_OnOffToggle;
			}
			if (focusedSelectable == _view.Bgm_Volume_Slider)
			{
				return _view.Bgm_Volume_OnOffToggle;
			}
			if (focusedSelectable == _view.Sfx_Volume_Slider)
			{
				return _view.Sfx_Volume_OnOffToggle;
			}
			return null;
		}
	}
}
