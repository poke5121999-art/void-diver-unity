using UniRx;

public class ShopProductPanelModel : BasePanelModel
{
	public int ShopProductGroupId;

	public readonly Subject<ShopProductSlotModel> OnSlotLeftClick;

	public readonly Subject<ShopProductSlotModel> OnSlotRightClick;

	public readonly ReactiveProperty<ShopProductSlotModel> RxFocusedSlotModel;

	public ShopProductSlotModel FocusedSlotModel => null;
}
