using GameCommon.Enum;
using UniRx;

public class EquipmentInventoryPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<EGoodsSlotCategory> RxHighlightCategory = new ReactiveProperty<EGoodsSlotCategory>();

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedEquipmentSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotEndDrag = new Subject<InventoryGoodsSlotModel>();
}
