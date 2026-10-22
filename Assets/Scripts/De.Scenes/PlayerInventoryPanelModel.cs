using GameCommon.Enum;
using UniRx;

public class PlayerInventoryPanelModel : BasePanelModel
{
	public PlayerInventory PlayerInventory;

	public EGoodsType[] AllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public EKeyItemType[] KeyItemFilterTypes;

	public EGoodsDisplayScope DisplayScope;

	public readonly ReactiveProperty<EGoodsSlotCategory> RxHighlightCategory;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedEquipmentSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotEndDrag;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSafeSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotEndDrag;

	public bool IsArtifactShop;

	public bool IsArtifactDeal;

	public PlayerInventoryPanelPresenter.EFocusPanel FocusPanel;
}
