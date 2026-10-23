using GameCommon.Enum;
using UniRx;

public class PlayerInventoryScrollerModel : BaseScrollerModel<InventoryGoodsSlotModel>
{
	public PlayerInventory PlayerInventory;

	public EGoodsType[] AllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public EKeyItemType[] KeyItemFilterTypes;

	public EGoodsDisplayScope DisplayScope;

	public readonly ReactiveProperty<int> RxPageCount = new ReactiveProperty<int>(-1);

	public readonly ReactiveProperty<int> RxPageIndex = new ReactiveProperty<int>(-1);

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly ReactiveProperty<bool> RxIsEmpty = new ReactiveProperty<bool>();

	public readonly Subject<int> OnMovePage = new Subject<int>();

	public int PageCount => RxPageCount.Value;

	public int PageIndex => RxPageIndex.Value;
}
