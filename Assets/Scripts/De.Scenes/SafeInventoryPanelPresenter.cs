using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class SafeInventoryPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private SafeInventoryPanelModel _model;

	[View]
	private SafeInventoryPanelView _view;

	[Inject]
	private GameManager _gameManager;

	private readonly List<InventoryGoodsSlotModel> _safeSlotModels;

	private CompositeDisposable _disposable;

	private GamePlayer Player => null;

	private PlayerInventory Inventory => null;

	private int SafeSlotCounts => 0;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(SafeInventoryPanelModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitSafeSlots()
	{
	}

	private void OnSafeSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSafeSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnTalentUpdated(Talent talent)
	{
	}

	private bool CheckFilter(GoodsSlot goodsSlot)
	{
		return false;
	}
}
