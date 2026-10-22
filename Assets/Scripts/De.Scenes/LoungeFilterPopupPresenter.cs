using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LoungeFilterPopupPresenter : GamePopupPresenter
{
	[View]
	private LoungeFilterPopupView _view;

	[InjectModel]
	private LoungeFilterPopupModel _model;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		InitUI();
		BindUIEvents();
		BindButtonEvents();
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		List<Selectable> list = _view.Ping_Toggles.Select((GameObject go) => go.GetComponent<Toggle>()).Cast<Selectable>().ToList();
		ConnectNav(_view.Reset_Button, ENavDir.Down, _view.ShowPlayingLounge_Toggle);
		ConnectNav(_view.ShowPlayingLounge_Toggle, ENavDir.Up, _view.Reset_Button);
		ConnectNav(_view.ShowPlayingLounge_Toggle, ENavDir.Down, _view.BlockOtherArea_Toggle);
		ConnectNav(_view.BlockOtherArea_Toggle, ENavDir.Up, _view.ShowPlayingLounge_Toggle);
		ConnectNav(_view.BlockOtherArea_Toggle, ENavDir.Down, list[0]);
		SetNavHorizontal(list);
		foreach (Selectable item in list)
		{
			ConnectNav(item, ENavDir.Up, _view.BlockOtherArea_Toggle);
			ConnectNav(item, ENavDir.Down, _view.LoungeDifficulty_Story_Toggle);
		}
		ConnectNav(_view.LoungeDifficulty_Story_Toggle, ENavDir.Up, list[0]);
		ConnectNav(_view.LoungeDifficulty_Story_Toggle, ENavDir.Down, _view.LoungeDifficulty_Junior_Toggle);
		ConnectNav(_view.LoungeDifficulty_Junior_Toggle, ENavDir.Up, _view.LoungeDifficulty_Story_Toggle);
		ConnectNav(_view.LoungeDifficulty_Junior_Toggle, ENavDir.Down, _view.LoungeDifficulty_Classic_Toggle);
		ConnectNav(_view.LoungeDifficulty_Classic_Toggle, ENavDir.Up, _view.LoungeDifficulty_Junior_Toggle);
		ConnectNav(_view.LoungeDifficulty_Classic_Toggle, ENavDir.Down, _view.LoungeDifficulty_Veteran_Toggle);
		ConnectNav(_view.LoungeDifficulty_Veteran_Toggle, ENavDir.Up, _view.LoungeDifficulty_Classic_Toggle);
		ConnectNav(_view.LoungeDifficulty_Veteran_Toggle, ENavDir.Down, _view.MinHostLevel_Slider);
		ConnectNav(_view.MinHostLevel_Slider, ENavDir.Up, _view.LoungeDifficulty_Veteran_Toggle);
		ConnectNav(_view.MinHostLevel_Slider, ENavDir.Down, _view.Confirm_Button);
		ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.MinHostLevel_Slider);
		ConnectNav(_view.Confirm_Button, ENavDir.Right, _view.Cancel_Button);
		ConnectNav(_view.Cancel_Button, ENavDir.Up, _view.MinHostLevel_Slider);
		ConnectNav(_view.Cancel_Button, ENavDir.Left, _view.Confirm_Button);
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnCancel();
			}).AddTo(InputDisposable);
		}
	}

	private void InitUI()
	{
		_view.LoungeDifficulty_Story_Text.text = ERoomDifficulty.Story.ToLocalText().WithColor(ERoomDifficulty.Story.GetColor()) + " - " + ERoomDifficulty.Story.GetRoomDifficultyDescription();
		_view.LoungeDifficulty_Junior_Text.text = ERoomDifficulty.Junior.ToLocalText().WithColor(ERoomDifficulty.Junior.GetColor()) + " - " + ERoomDifficulty.Junior.GetRoomDifficultyDescription();
		_view.LoungeDifficulty_Classic_Text.text = ERoomDifficulty.Classic.ToLocalText().WithColor(ERoomDifficulty.Classic.GetColor()) + " - " + ERoomDifficulty.Classic.GetRoomDifficultyDescription();
		_view.LoungeDifficulty_Veteran_Text.text = ERoomDifficulty.Veteran.ToLocalText().WithColor(ERoomDifficulty.Veteran.GetColor()) + " - " + ERoomDifficulty.Veteran.GetRoomDifficultyDescription();
		RefreshUI();
	}

	private void RefreshUI()
	{
		_view.BlockOtherArea_Toggle.isOn = _model.BlockOtherAreas;
		_view.ShowPlayingLounge_Toggle.isOn = _model.ShowPlayingLounge;
		_view.LoungeDifficulty_Story_Toggle.isOn = !_model.BlockedDifficulties.Contains(ERoomDifficulty.Story);
		_view.LoungeDifficulty_Junior_Toggle.isOn = !_model.BlockedDifficulties.Contains(ERoomDifficulty.Junior);
		_view.LoungeDifficulty_Classic_Toggle.isOn = !_model.BlockedDifficulties.Contains(ERoomDifficulty.Classic);
		_view.LoungeDifficulty_Veteran_Toggle.isOn = !_model.BlockedDifficulties.Contains(ERoomDifficulty.Veteran);
		_view.MinHostLevel_Slider.minValue = 1f;
		_view.MinHostLevel_Slider.maxValue = TConst.LevelMax.IntValue;
		_view.MinHostLevel_Slider.value = _model.MinHostLevel;
		int minPingFilterType = (int)_model.MinPingFilterType;
		for (int i = 0; i < _view.Ping_Toggles.Length; i++)
		{
			_view.Ping_Toggles[i].GetComponent<Toggle>().isOn = i == minPingFilterType;
		}
	}

	private void BindUIEvents()
	{
		_view.BlockOtherArea_Toggle.OnValueChangedAsObservable().Subscribe(OnBlockOtherAreaToggleChanged).AddTo(this);
		_view.ShowPlayingLounge_Toggle.OnValueChangedAsObservable().Subscribe(delegate(bool isOn)
		{
			_model.ShowPlayingLounge = isOn;
		}).AddTo(this);
		_view.LoungeDifficulty_Story_Toggle.OnValueChangedAsObservable().Subscribe(delegate(bool isOn)
		{
			OnDifficultyToggleChanged(ERoomDifficulty.Story, isOn);
		}).AddTo(this);
		_view.LoungeDifficulty_Junior_Toggle.OnValueChangedAsObservable().Subscribe(delegate(bool isOn)
		{
			OnDifficultyToggleChanged(ERoomDifficulty.Junior, isOn);
		}).AddTo(this);
		_view.LoungeDifficulty_Classic_Toggle.OnValueChangedAsObservable().Subscribe(delegate(bool isOn)
		{
			OnDifficultyToggleChanged(ERoomDifficulty.Classic, isOn);
		}).AddTo(this);
		_view.LoungeDifficulty_Veteran_Toggle.OnValueChangedAsObservable().Subscribe(delegate(bool isOn)
		{
			OnDifficultyToggleChanged(ERoomDifficulty.Veteran, isOn);
		}).AddTo(this);
		_view.MinHostLevel_Slider.OnValueChangedAsObservable().Subscribe(OnMinHostLevelChanged).AddTo(this);
		_view.MinHostLevel_UpButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMinHostLevelUpClick();
		}).AddTo(this);
		_view.MinHostLevel_DownButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMinHostLevelDownClick();
		}).AddTo(this);
		for (int i = 0; i < _view.Ping_Toggles.Length; i++)
		{
			int index = i;
			_view.Ping_Toggles[i].GetComponent<Toggle>().OnValueChangedAsObservable().Subscribe(delegate(bool isOn)
			{
				OnPingToggleChanged(index, isOn);
			})
				.AddTo(this);
		}
	}

	private void BindButtonEvents()
	{
		_view.Reset_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnResetButtonClick();
		}).AddTo(this);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConfirm();
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCancel();
		}).AddTo(this);
	}

	private void OnBlockOtherAreaToggleChanged(bool isOn)
	{
		_model.BlockOtherAreas = isOn;
	}

	private void OnDifficultyToggleChanged(ERoomDifficulty difficulty, bool isOn)
	{
		if (isOn)
		{
			_model.BlockedDifficulties.Remove(difficulty);
		}
		else if (!_model.BlockedDifficulties.Contains(difficulty))
		{
			_model.BlockedDifficulties.Add(difficulty);
		}
	}

	private void OnMinHostLevelChanged(float value)
	{
		_model.MinHostLevel = (int)value;
		_view.MinHostLevel_LevelText.text = TLocalizedText.LevelFormat.Format(_model.MinHostLevel);
	}

	private void OnPingToggleChanged(int index, bool isOn)
	{
		if (isOn)
		{
			_model.MinPingFilterType = (ERoomPingFilterType)index;
		}
	}

	private void OnResetButtonClick()
	{
		_model.BlockOtherAreas = false;
		_model.BlockedDifficulties.Clear();
		_model.MinHostLevel = 1;
		_model.MinPingFilterType = ERoomPingFilterType.All;
		_model.ShowPlayingLounge = false;
		RefreshUI();
	}

	private void OnConfirm()
	{
		Close(_model);
	}

	private void OnCancel()
	{
		Close();
	}

	private void OnMinHostLevelUpClick()
	{
		_view.MinHostLevel_Slider.value = Math.Min((int)_view.MinHostLevel_Slider.maxValue, (int)_view.MinHostLevel_Slider.value + 1);
	}

	private void OnMinHostLevelDownClick()
	{
		_view.MinHostLevel_Slider.value = Math.Max((int)_view.MinHostLevel_Slider.minValue, (int)_view.MinHostLevel_Slider.value - 1);
	}
}
