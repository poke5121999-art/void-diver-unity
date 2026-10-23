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

public class RoomDifficultyPopupPresenter : GamePopupPresenter
{
	[View]
	private RoomDifficultyPopupView _view;

	[InjectModel]
	private RoomDifficultyPopupModel _model;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	private ERoomDifficulty _selectedDifficulty = ERoomDifficulty.Junior;

	private readonly List<RoomDifficultySlotModel> _slotModels = new List<RoomDifficultySlotModel>();

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		for (int i = 0; i < 4; i++)
		{
			RoomDifficultySlotModel roomDifficultySlotModel = new RoomDifficultySlotModel();
			roomDifficultySlotModel.RoomDifficulty = (ERoomDifficulty)i;
			roomDifficultySlotModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
			if (i == (int)_model.Difficulty)
			{
				roomDifficultySlotModel.RxIsSelected.Value = true;
				_selectedDifficulty = _model.Difficulty;
			}
			_view.Slots[i].GetComponent<RoomDifficultySlotPresenter>().Inject(roomDifficultySlotModel);
			_slotModels.Add(roomDifficultySlotModel);
		}
		_view.Difficulty_Desc.text = _selectedDifficulty.GetRoomDifficultyDescription();
		_view.Confirm_Button.OnClickAsObservable().Subscribe(delegate
		{
			OnConfirmButtonClick();
		}).AddTo(this);
		_view.Cancel_Button.OnClickAsObservable().Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.WeaponProtect_Toggle.isOn = _model.PreventMaxDurabilityLoss;
		_view.WeaponProtect_PadButton.OnClickAsObservable().Subscribe(delegate
		{
			_view.WeaponProtect_Toggle.isOn = !_view.WeaponProtect_Toggle.isOn;
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		List<Button> list = _view.Slots.Select((GameObject s) => s.GetComponent<Button>()).ToList();
		SetNavHorizontal(list);
		foreach (Button item in list)
		{
			ConnectNav(item, ENavDir.Down, _view.WeaponProtect_PadButton);
		}
		ConnectNav(_view.WeaponProtect_PadButton, ENavDir.Down, _view.Confirm_Button);
		InitBottomGamePadNav();
		SetDefaultNav(_view.Slots[(int)_selectedDifficulty].GetComponent<Selectable>());
	}

	private void InitBottomGamePadNav()
	{
		Selectable component = _view.Slots[(int)_selectedDifficulty].GetComponent<Selectable>();
		ConnectNav(_view.WeaponProtect_PadButton, ENavDir.Up, component);
		ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.WeaponProtect_PadButton);
		ConnectNav(_view.Confirm_Button, ENavDir.Right, _view.Cancel_Button);
		ConnectNav(_view.Cancel_Button, ENavDir.Up, _view.WeaponProtect_PadButton);
		ConnectNav(_view.Cancel_Button, ENavDir.Left, _view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickAsObservable().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private void OnSlotClick(RoomDifficultySlotModel slotModel)
	{
		if (slotModel == null)
		{
			return;
		}
		ERoomDifficulty roomDifficulty = slotModel.RoomDifficulty;
		if (_selectedDifficulty != roomDifficulty)
		{
			_slotModels[(int)_selectedDifficulty].RxIsSelected.Value = false;
			_slotModels[(int)roomDifficulty].RxIsSelected.Value = true;
			_selectedDifficulty = slotModel.RoomDifficulty;
			_view.Difficulty_Desc.text = _selectedDifficulty.GetRoomDifficultyDescription();
			_soundManager.PlaySfx("select");
			InitBottomGamePadNav();
		}
	}

	private void OnConfirmButtonClick()
	{
		if (_selectedDifficulty != _model.Difficulty)
		{
			_toastManager.Notice(TLocalizedText.RoomDifficultyUpdated.LocalText);
			_soundManager.PlaySfx("RiskLevel_Change");
		}
		Close(new RoomDifficultyPopupResult
		{
			Difficulty = _selectedDifficulty,
			PreventMaxDurabilityLoss = _view.WeaponProtect_Toggle.isOn
		});
	}
}
