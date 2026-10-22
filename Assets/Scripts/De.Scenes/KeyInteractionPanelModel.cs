using UniRx;

public class KeyInteractionPanelModel : BasePanelModel
{
	public int RequiredKeyCount;

	public readonly ReactiveCollection<int> RxSelectedItemIds;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly Subject<Unit> OnConfirmClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;
}
