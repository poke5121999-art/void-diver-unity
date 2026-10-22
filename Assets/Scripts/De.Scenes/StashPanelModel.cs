using GameCommon.Enum;
using UniRx;

public class StashPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;

	public bool IsArtifactShop;

	public bool IsArtifactDeal;

	public EGoodsType[] AllowedSubtypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public bool EnableFilterButtons;

	public readonly Subject<EGoodsType[]> OnFilterChanged;

	public readonly Subject<EGoodsType[]> OnFilterRestoreRequest;

	public StashPanelPresenter.EFocusPanel FocusPanel;
}
