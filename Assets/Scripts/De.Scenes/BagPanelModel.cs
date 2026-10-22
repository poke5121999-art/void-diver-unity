using GameCommon.Data;
using UniRx;

public class BagPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<BagGoods> RxBag = new ReactiveProperty<BagGoods>();

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly ReactiveProperty<bool> RxIsDragAcceptable = new ReactiveProperty<bool>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<Unit> OnBack = new Subject<Unit>();

	public BagGoods Bag
	{
		get
		{
			return RxBag.Value;
		}
		set
		{
			RxBag.SetValueAndForceNotify(value);
		}
	}
}
