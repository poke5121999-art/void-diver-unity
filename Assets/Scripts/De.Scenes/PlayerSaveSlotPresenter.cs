using System;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class PlayerSaveSlotPresenter : BaseSlotPresenter<PlayerSaveSlotModel>
{
	[InjectModel]
	private PlayerSaveSlotModel _model;

	[View]
	private PlayerSaveSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override PlayerSaveSlotModel SlotModel => _model;

	private void OnInject(PlayerSaveSlotModel model)
	{
		InitSlot(model);
		_playerManager.OnPlayerSlotUpdated.Subscribe(OnPlayerSlotUpdated).AddTo(this);
		UpdateUis();
	}

	private void UpdateUis()
	{
		int playerSlotIndex = _model.PlayerSlotIndex;
		PlayerSlot playerSlot = _playerManager.PlayerSlots[playerSlotIndex];
		bool flag = string.IsNullOrEmpty(playerSlot.PlayerName);
		_view.SlotNumber.text = TLocalizedText.PlayerSlotNumberFormat.Format(playerSlotIndex + 1);
		_view.LevelText.text = (flag ? string.Empty : string.Format("Lv. {0}", playerSlot.Level));
		_view.PlayerName.text = (flag ? TLocalizedText.EmptySlot.LocalText : playerSlot.PlayerName);
	}

	private void OnPlayerSlotUpdated(int slotIndex)
	{
		if (_model.PlayerSlotIndex == slotIndex)
		{
			UpdateUis();
		}
	}
}
