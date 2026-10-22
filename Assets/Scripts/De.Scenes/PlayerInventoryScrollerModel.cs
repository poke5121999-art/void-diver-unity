using GameCommon.Enum;
using UniRx;

public class PlayerInventoryScrollerModel : BaseScrollerModel<InventoryGoodsSlotModel>
{
	public PlayerInventory PlayerInventory;

	public EGoodsType[] AllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public EKeyItemType[] KeyItemFilterTypes;

	public EGoodsDisplayScope DisplayScope;

	public readonly ReactiveProperty<int> RxPageCount;

	public readonly ReactiveProperty<int> RxPageIndex;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;

	public readonly ReactiveProperty<bool> RxIsEmpty;

	public readonly Subject<int> OnMovePage;

	public int PageCount => 0;

	public int PageIndex => 0;
}
