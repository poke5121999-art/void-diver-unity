using GameCommon.Data;
using UniRx;

public class BagPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<BagGoods> RxBag;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly ReactiveProperty<bool> RxIsDragAcceptable;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotLeftClick;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;

	public readonly Subject<Unit> OnBack;

	public BagGoods Bag
	{
		get
		{
			return null;
		}
		set
		{
		}
	}
}
