using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class QuickSlotSettingPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private QuickSlotSettingPanelModel _model;

	[View]
	private QuickSlotSettingPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private readonly List<InventoryGoodsSlotModel> _goodsSlotModels;

	protected override BasePanelModel PanelModel => null;

	private PlayerInventory MyInventory => null;

	protected void OnInject(QuickSlotSettingPanelModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitSlots()
	{
	}

	private GoodsSlot CreateGoodsSlot(int index)
	{
		return null;
	}

	private void RefreshSlot(int index)
	{
	}

	private void RefreshSlots()
	{
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnQuickSlotChanged(int index)
	{
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}
}
