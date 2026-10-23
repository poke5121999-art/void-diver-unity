using GameCommon.Enum;
using UniRx;

public class PlayerInventoryPanelModel : BasePanelModel
{
	public PlayerInventory PlayerInventory;

	public EGoodsType[] AllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public EKeyItemType[] KeyItemFilterTypes;

	public EGoodsDisplayScope DisplayScope;

	public readonly ReactiveProperty<EGoodsSlotCategory> RxHighlightCategory = new ReactiveProperty<EGoodsSlotCategory>();

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedEquipmentSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEquipmentSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSafeSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSafeSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public bool IsArtifactShop;

	public bool IsArtifactDeal;

	public PlayerInventoryPanelPresenter.EFocusPanel FocusPanel;
}
