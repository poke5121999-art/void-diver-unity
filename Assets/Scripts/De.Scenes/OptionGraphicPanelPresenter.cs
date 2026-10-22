using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class OptionGraphicPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private OptionGraphicPanelModel _model;

	[View]
	private OptionGraphicPanelView _view;

	[Inject]
	private GraphicSettingService _graphicSettings;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GamePlaySettingService _playSettingService;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(OptionGraphicPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitScreenMode();
		InitResolution();
		InitGraphicsQuality();
		InitFrameRate();
		InitVSync();
		InitCameraShaking();
		InitGamma();
		_graphicSettings.RxVSyncOn.Subscribe(OnVSyncChanged).AddTo(_disposable);
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
		_playSettingService.RxLanguage.Subscribe(OnLanguageChanged).AddTo(_disposable);
	}

	private void OnDisable()
	{
		_graphicSettings.Flush();
	}

	private void OnLanguageChanged(ELanguage language)
	{
		ApplyScreenModeLocalText();
		ApplyResolutionLocalText();
		ApplyGraphicsQualityLocalText();
		ApplyFrameRateLocalText();
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> list = _view.Screen_Toggles.Take(Enum.GetNames(typeof(EScreenMode)).Length).Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list2 = new List<Selectable> { _view.Resolution_Button };
		List<Selectable> list3 = _view.GraphicsQuality_Toggles.Take(Enum.GetNames(typeof(EQualityLevel)).Length).Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list4 = _view.VSync_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list5 = _view.FPS_Toggles.Take(Enum.GetNames(typeof(EFrameRate)).Length).Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list6 = new List<Selectable> { _view.Gamma_Button };
		List<Selectable> list7 = _view.CameraShaking_Toggles.Select((GameObject _) => (Selectable)_.GetComponent<Toggle>()).ToList();
		List<Selectable> list8 = new List<Selectable> { _view.ResetButton };
		List<List<Selectable>> list9 = new List<List<Selectable>>();
		list9.Add(list);
		list9.Add(list2);
		list9.Add(list3);
		list9.Add(list4);
		if (!_graphicSettings.VSyncOn)
		{
			list9.Add(list5);
		}
		list9.Add(list6);
		list9.Add(list7);
		list9.Add(list8);
		foreach (List<Selectable> item in list9)
		{
			SetNavHorizontal(item);
		}
		for (int i = 0; i < list9.Count - 1; i++)
		{
			List<Selectable> list10 = list9[i];
			List<Selectable> list11 = list9[i + 1];
			for (int j = 0; j < list10.Count; j++)
			{
				ConnectNav(list10[j], ENavDir.Down, list11[Math.Max(j + (list11.Count - list10.Count), 0)]);
			}
			for (int k = 0; k < list11.Count; k++)
			{
				ConnectNav(list11[k], ENavDir.Up, list10[Math.Max(k + (list10.Count - list11.Count), 0)]);
			}
		}
	}

	private void InitScreenMode()
	{
		int length = Enum.GetNames(typeof(EScreenMode)).Length;
		for (int i = 0; i < _view.Screen_Toggles.Length; i++)
		{
			EScreenMode screenMode = (EScreenMode)i;
			Toggle component = _view.Screen_Toggles[i].GetComponent<Toggle>();
			component.gameObject.SetActive(i < length);
			if (i < length)
			{
				component.isOn = i == (int)_graphicSettings.ScreenMode;
				component.OnValueTrueWithSfx().Subscribe(delegate
				{
					SetScreenMode(screenMode);
				}).AddTo(_disposable);
			}
		}
	}

	private void ApplyScreenModeLocalText()
	{
		int length = Enum.GetNames(typeof(EScreenMode)).Length;
		for (int i = 0; i < _view.Screen_Toggles.Length; i++)
		{
			if (i < length)
			{
				_view.Screen_Toggles[i].GetComponent<Toggle>().gameObject.FindChildComponent<TextMeshProUGUI>("Text").text = ((EScreenMode)i).ToLocalText();
			}
		}
	}

	private void SetScreenMode(EScreenMode screenMode)
	{
		if (_graphicSettings.ScreenMode != screenMode)
		{
			_graphicSettings.SetScreenMode(screenMode);
		}
	}

	private void InitResolution()
	{
		_view.Resolution_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnResolutionButtonClickAsync().Forget();
		}).AddTo(_disposable);
	}

	private void ApplyResolutionLocalText()
	{
		_view.Resolution_Button_TextTitle.text = _graphicSettings.Resolution.ToLocalText();
	}

	private async UniTask OnResolutionButtonClickAsync()
	{
		EResolution[] resolutions = (EResolution[])Enum.GetValues(typeof(EResolution));
		string[] options = resolutions.Select((EResolution _) => _.ToLocalText()).ToArray();
		OptionListPopupPresenter optionListPopupPresenter = await _popupManager.OpenAsync<OptionListPopupPresenter>(new OptionListPopupModel
		{
			Title = TLocalizedText.Resolution.LocalText,
			Options = options,
			SelectedIndex = (int)_graphicSettings.Resolution
		});
		object obj = await optionListPopupPresenter.Task;
		if (obj is int)
		{
			EResolution eResolution = resolutions[(int)obj];
			if (_graphicSettings.Resolution != eResolution)
			{
				_graphicSettings.SetResolution(eResolution);
				ApplyResolutionLocalText();
			}
		}
	}

	private void InitGraphicsQuality()
	{
		int length = Enum.GetNames(typeof(EQualityLevel)).Length;
		for (int i = 0; i < _view.GraphicsQuality_Toggles.Length; i++)
		{
			EQualityLevel qualityLevel = (EQualityLevel)i;
			Toggle component = _view.GraphicsQuality_Toggles[i].GetComponent<Toggle>();
			component.gameObject.SetActive(i < length);
			if (i < length)
			{
				component.isOn = i == (int)_graphicSettings.QualityLevel;
				component.OnValueTrueWithSfx().Subscribe(delegate
				{
					SetQualityLevel(qualityLevel);
				}).AddTo(_disposable);
			}
		}
	}

	private void ApplyGraphicsQualityLocalText()
	{
		int length = Enum.GetNames(typeof(EQualityLevel)).Length;
		for (int i = 0; i < _view.GraphicsQuality_Toggles.Length; i++)
		{
			if (i < length)
			{
				_view.GraphicsQuality_Toggles[i].GetComponent<Toggle>().gameObject.FindChildComponent<TextMeshProUGUI>("Text").text = ((EQualityLevel)i).ToLocalText();
			}
		}
	}

	private void SetQualityLevel(EQualityLevel qualityLevel)
	{
		if (_graphicSettings.QualityLevel != qualityLevel)
		{
			_graphicSettings.SetQualityLevel(qualityLevel);
		}
	}

	private void InitFrameRate()
	{
		int length = Enum.GetNames(typeof(EFrameRate)).Length;
		for (int i = 0; i < _view.FPS_Toggles.Length; i++)
		{
			EFrameRate frameRate = (EFrameRate)i;
			Toggle component = _view.FPS_Toggles[i].GetComponent<Toggle>();
			component.gameObject.SetActive(i < length);
			if (i < length)
			{
				component.isOn = i == (int)_graphicSettings.FrameRate;
				component.OnValueTrueWithSfx().Subscribe(delegate
				{
					SetFrameRate(frameRate);
				}).AddTo(_disposable);
			}
		}
	}

	private void ApplyFrameRateLocalText()
	{
		int length = Enum.GetNames(typeof(EFrameRate)).Length;
		for (int i = 0; i < _view.FPS_Toggles.Length; i++)
		{
			if (i < length)
			{
				_view.FPS_Toggles[i].GetComponent<Toggle>().gameObject.FindChildComponent<TextMeshProUGUI>("Text").text = ((EFrameRate)i).ToLocalText();
			}
		}
	}

	private void SetFrameRate(EFrameRate frameRate)
	{
		if (_graphicSettings.FrameRate != frameRate)
		{
			_graphicSettings.SetFrameRate(frameRate);
		}
	}

	private void InitVSync()
	{
		for (int i = 0; i < _view.VSync_Toggles.Length; i++)
		{
			bool vsyncOn = i == 0;
			Toggle component = _view.VSync_Toggles[i].GetComponent<Toggle>();
			if (vsyncOn == _graphicSettings.VSyncOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetVSyncOn(vsyncOn);
			}).AddTo(_disposable);
		}
	}

	private void SetVSyncOn(bool vsyncOn)
	{
		if (_graphicSettings.VSyncOn != vsyncOn)
		{
			_graphicSettings.SetVSyncOn(vsyncOn);
		}
	}

	private void OnVSyncChanged(bool vsyncOn)
	{
		_view.FPS.SetActive(!vsyncOn);
		InitGamePadNav();
	}

	private void InitCameraShaking()
	{
		for (int i = 0; i < _view.CameraShaking_Toggles.Length; i++)
		{
			bool cameraShakingOn = i == 0;
			Toggle component = _view.CameraShaking_Toggles[i].GetComponent<Toggle>();
			if (cameraShakingOn == _graphicSettings.CameraShakingOn)
			{
				component.isOn = true;
			}
			component.OnValueTrueWithSfx().Subscribe(delegate
			{
				SetCameraShakingOn(cameraShakingOn);
			}).AddTo(_disposable);
		}
	}

	private void SetCameraShakingOn(bool cameraShakingOn)
	{
		if (_graphicSettings.CameraShakingOn != cameraShakingOn)
		{
			_graphicSettings.SetCameraShakingOn(cameraShakingOn);
		}
	}

	private void InitGamma()
	{
		_view.Gamma_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnGammaButtonClickAsync().Forget();
		}).AddTo(_disposable);
	}

	private async UniTask OnGammaButtonClickAsync()
	{
		GammaPopupPresenter gammaPopupPresenter = await _popupManager.OpenAsync<GammaPopupPresenter>(new GammaPopupModel
		{
			GammaPercent = _graphicSettings.GammaPercent
		});
		object obj = await gammaPopupPresenter.Task;
		if (obj is int)
		{
			int num = (int)obj;
			if (_graphicSettings.GammaPercent != num)
			{
				_graphicSettings.SetGamma(num);
			}
		}
	}

	private void OnResetClick()
	{
		_graphicSettings.Reset();
		ApplyResolutionLocalText();
		SyncToggle(_view.Screen_Toggles, (int)_graphicSettings.ScreenMode);
		SyncToggle(_view.GraphicsQuality_Toggles, (int)_graphicSettings.QualityLevel);
		SyncToggle(_view.FPS_Toggles, (int)_graphicSettings.FrameRate);
		SyncToggle(_view.VSync_Toggles, _graphicSettings.VSyncOn ? 0 : 1);
		SyncToggle(_view.CameraShaking_Toggles, _graphicSettings.CameraShakingOn ? 0 : 1);
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
