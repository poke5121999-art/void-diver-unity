using UniRx;

public class QuickSlotSettingPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly ReactiveProperty<bool> RxIsLocked;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotRightClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;

	public InventoryGoodsSlotModel FocusedSlotModel => null;

	public bool IsLocked => false;
}
