using UniRx;

public class LootingInventoryPanelModel : BasePanelModel
{
	public LootingInventory LootingInventory;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;
}
