using GameCommon.Enum;
using UniRx;

public class EquipmentInventoryPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<EGoodsSlotCategory> RxHighlightCategory;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedEquipmentSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotEndDrag;
}
