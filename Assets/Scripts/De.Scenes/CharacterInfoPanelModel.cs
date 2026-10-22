using UniRx;

public class CharacterInfoPanelModel : BasePanelModel
{
	public Subject<InventoryGoodsSlotModel> OnGoodsSlotLeftClick = new Subject<InventoryGoodsSlotModel>();

	public Subject<InventoryGoodsSlotModel> OnGoodsSlotRightClick = new Subject<InventoryGoodsSlotModel>();

	public Subject<InventoryGoodsSlotModel> OnGoodsSlotEntered = new Subject<InventoryGoodsSlotModel>();

	public Subject<InventoryGoodsSlotModel> OnGoodsSlotExit = new Subject<InventoryGoodsSlotModel>();
}
