using GameCommon.Data;
using GameCommon.Table;
using UniRx;

public class ScrapPanelModel : BasePanelModel
{
	public readonly Subject<InventoryGoodsSlotModel> OnScrapSlotEnter;

	public readonly Subject<InventoryGoodsSlotModel> OnScrapSlotExit;

	public readonly Subject<Unit> OnScrapButtonClick;

	public readonly Subject<Unit> OnScrapSlotClick;

	public readonly ReactiveProperty<TScrap> RxScrapData;

	public readonly ReactiveProperty<long> RxScrapCount;

	public readonly ReactiveProperty<bool> RxIsInvalid;

	public InventoryGoodsSlotModel ScrapSlotModel { get; set; }

	public Goods RegisteredGoods { get; private set; }

	public void SetScrapInfo(TScrap scrapData, long count, Goods registeredGoods)
	{
	}

	public void Clear()
	{
	}
}
