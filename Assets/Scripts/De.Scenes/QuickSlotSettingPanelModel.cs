using UniRx;

public class QuickSlotSettingPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly ReactiveProperty<bool> RxIsLocked = new ReactiveProperty<bool>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public InventoryGoodsSlotModel FocusedSlotModel => RxFocusedSlotModel.Value;

	public bool IsLocked => RxIsLocked.Value;
}
