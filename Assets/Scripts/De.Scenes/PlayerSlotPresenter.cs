using System;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class PlayerSlotPresenter : BaseSlotPresenter<PlayerSlotModel>
{
	[InjectModel]
	private PlayerSlotModel _model;

	[View]
	private PlayerSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override PlayerSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	protected override void Start()
	{
		base.Start();
		_view.Delete_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnDeleteClick();
		}).AddTo(this);
	}

	private void OnInject(PlayerSlotModel model)
	{
		InitSlot(model);
		_playerManager.RxPlayerSlotIndex.Subscribe(OnPlayerSlotIndexChanged).AddTo(this);
		_playerManager.OnPlayerSlotUpdated.Subscribe(OnPlayerSlotUpdated).AddTo(this);
		UpdateUis();
	}

	private void UpdateUis()
	{
		int playerSlotIndex = _model.PlayerSlotIndex;
		PlayerSlot playerSlot = _playerManager.PlayerSlots[playerSlotIndex];
		bool flag = string.IsNullOrEmpty(playerSlot.PlayerName);
		_view.SlotNumberText.text = TLocalizedText.PlayerSlotNumberFormat.Format(playerSlotIndex + 1);
		_view.Version.text = string.Format("Ver. {0}", playerSlot.Version);
		_view.Delete.SetActive(!flag);
		if (flag)
		{
			_view.PalyerName.text = TLocalizedText.EmptySlot.LocalText;
			_view.LevelText.text = string.Empty;
			_view.PlayTimeText.text = "--:--";
			_view.SaveTimeText.text = "---- / -- / -- / --:--";
			return;
		}
		_view.PalyerName.text = playerSlot.PlayerName;
		_view.LevelText.text = string.Format("Lv. {0}", playerSlot.Level);
		string text = new TimeSpan(playerSlot.PlayTimeTicks).ToString("hh\\:mm");
		_view.PlayTimeText.text = TLocalizedText.PlayTimeFormat.Format(text);
		_view.SaveTimeText.text = new DateTime(playerSlot.SaveTimeTicks, DateTimeKind.Utc).ToLocalTime().ToString("yyyy / MM / dd / HH:mm");
	}

	private void OnDeleteClick()
	{
		if (_model != null)
		{
			_model.OnDeleteClick.OnNext(_model);
		}
	}

	private void OnPlayerSlotIndexChanged(int index)
	{
		SlotModel.RxIsSelected.Value = _model.PlayerSlotIndex == index;
	}

	private void OnPlayerSlotUpdated(int slotIndex)
	{
		if (_model.PlayerSlotIndex == slotIndex)
		{
			UpdateUis();
		}
	}
}
