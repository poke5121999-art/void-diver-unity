using GameCommon.Enum;
using UniRx;

public class SafeInventoryPanelModel : BasePanelModel
{
	public PlayerInventory PlayerInventory;

	public EGoodsType[] AllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public EKeyItemType[] KeyItemFilterTypes;

	public bool HideGoods;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSafeSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotEndDrag = new Subject<InventoryGoodsSlotModel>();
}
