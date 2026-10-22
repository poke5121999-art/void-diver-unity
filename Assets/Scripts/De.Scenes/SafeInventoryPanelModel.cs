using GameCommon.Enum;
using UniRx;

public class SafeInventoryPanelModel : BasePanelModel
{
	public PlayerInventory PlayerInventory;

	public EGoodsType[] AllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public EKeyItemType[] KeyItemFilterTypes;

	public bool HideGoods;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSafeSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotEndDrag;
}
