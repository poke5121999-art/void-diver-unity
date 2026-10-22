using GameCommon.Enum;
using UniRx;

public class StashPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public bool IsArtifactShop;

	public bool IsArtifactDeal;

	public EGoodsType[] AllowedSubtypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public bool EnableFilterButtons;

	public readonly Subject<EGoodsType[]> OnFilterChanged = new Subject<EGoodsType[]>();

	public readonly Subject<EGoodsType[]> OnFilterRestoreRequest = new Subject<EGoodsType[]>();

	public StashPanelPresenter.EFocusPanel FocusPanel;
}
