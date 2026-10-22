using UniRx;

public class LootingInventoryPanelModel : BasePanelModel
{
	public LootingInventory LootingInventory;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();
}
