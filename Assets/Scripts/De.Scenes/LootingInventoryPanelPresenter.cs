using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class LootingInventoryPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private LootingInventoryPanelModel _model;

	[View]
	private LootingInventoryPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private readonly List<InventoryGoodsSlotModel> _goodsSlotModels;

	private const int _numberOfSlotsInRow = 6;

	protected override BasePanelModel PanelModel => null;

	private int SlotCount => 0;

	private int FilledSlots => 0;

	protected override void OnDestroy()
	{
	}

	protected void OnInject(LootingInventoryPanelModel model)
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitSlots()
	{
	}

	private int GetRevealingSlotIndex()
	{
		return 0;
	}

	private InventoryGoodsSlotPresenter.ELayout GetSlotLayout(GoodsSlot slot)
	{
		return InventoryGoodsSlotPresenter.ELayout.Normal;
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}
}
