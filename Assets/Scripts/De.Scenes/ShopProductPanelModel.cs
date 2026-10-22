using UniRx;

public class ShopProductPanelModel : BasePanelModel
{
	public int ShopProductGroupId;

	public readonly Subject<ShopProductSlotModel> OnSlotLeftClick = new Subject<ShopProductSlotModel>();

	public readonly Subject<ShopProductSlotModel> OnSlotRightClick = new Subject<ShopProductSlotModel>();

	public readonly ReactiveProperty<ShopProductSlotModel> RxFocusedSlotModel = new ReactiveProperty<ShopProductSlotModel>();

	public ShopProductSlotModel FocusedSlotModel => RxFocusedSlotModel.Value;
}
