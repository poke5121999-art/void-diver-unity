using UniRx;

public class CharacterInfoPanelModel : BasePanelModel
{
	public Subject<InventoryGoodsSlotModel> OnGoodsSlotLeftClick;

	public Subject<InventoryGoodsSlotModel> OnGoodsSlotRightClick;

	public Subject<InventoryGoodsSlotModel> OnGoodsSlotEntered;

	public Subject<InventoryGoodsSlotModel> OnGoodsSlotExit;
}
