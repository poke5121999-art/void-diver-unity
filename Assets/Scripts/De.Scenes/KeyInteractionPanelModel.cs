using UniRx;

public class KeyInteractionPanelModel : BasePanelModel
{
	public int RequiredKeyCount;

	public readonly ReactiveCollection<int> RxSelectedItemIds = new ReactiveCollection<int>();

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<Unit> OnConfirmClick = new Subject<Unit>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();
}
