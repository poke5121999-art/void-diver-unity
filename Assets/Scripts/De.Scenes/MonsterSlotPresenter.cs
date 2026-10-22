using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
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

	protected override MonsterSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(MonsterSlotModel model)
	{
	}
}
