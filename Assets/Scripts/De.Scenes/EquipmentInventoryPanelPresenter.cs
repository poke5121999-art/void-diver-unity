using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class EquipmentInventoryPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private EquipmentInventoryPanelModel _model;

	[View]
	private EquipmentInventoryPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	private InventoryGoodsSlotModel[] _weaponSlotModels;

	private InventoryGoodsSlotModel[] _accessorySlotModels;

	private InventoryGoodsSlotModel[] _artifactSlotModels;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	private GamePlayer Player => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(EquipmentInventoryPanelModel model)
	{
	}

	private void OutOfLeft()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitEquipmentSlots()
	{
	}

	private void OnEquipmentSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnEquipmentSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnHighlightCategoryChanged(EGoodsSlotCategory slotCategory)
	{
	}

	private void OnCharacterChanged(Character oldCharacter, Character newCharacter)
	{
	}
}
