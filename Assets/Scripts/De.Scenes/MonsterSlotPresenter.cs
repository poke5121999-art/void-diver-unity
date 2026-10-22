using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MonsterSlotPresenter : BaseSlotPresenter<MonsterSlotModel>
{
	[InjectModel]
	private MonsterSlotModel _model;

	[View]
	private MonsterSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override MonsterSlotModel SlotModel => _model;

	protected override Button Button => _view.Slot.GetComponent<Button>();

	private void OnInject(MonsterSlotModel model)
	{
		InitSlot(model);
		bool flag = _playerManager.HasMonsterRecordFlag(model.MonsterId, EMonsterRecordFlags.BaseInfo);
		_view.Icon.sprite = _assetManager.GetIconMonster(model.MonsterId);
		_view.Icon.color = (flag ? Color.white : GameCommon.Utility.ColorUtility.ParseOrWhite("#1B1B1B"));
	}
}
