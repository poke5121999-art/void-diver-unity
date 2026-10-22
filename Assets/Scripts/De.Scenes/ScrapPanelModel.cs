using GameCommon.Data;
using GameCommon.Table;
using UniRx;

public class ScrapPanelModel : BasePanelModel
{
	public readonly Subject<InventoryGoodsSlotModel> OnScrapSlotEnter = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnScrapSlotExit = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<Unit> OnScrapButtonClick = new Subject<Unit>();

	public readonly Subject<Unit> OnScrapSlotClick = new Subject<Unit>();

	public readonly ReactiveProperty<TScrap> RxScrapData = new ReactiveProperty<TScrap>();

	public readonly ReactiveProperty<long> RxScrapCount = new ReactiveProperty<long>();

	public readonly ReactiveProperty<bool> RxIsInvalid = new ReactiveProperty<bool>();

	public InventoryGoodsSlotModel ScrapSlotModel { get; set; }

	public Goods RegisteredGoods { get; private set; }

	public void SetScrapInfo(TScrap scrapData, long count, Goods registeredGoods)
	{
		RegisteredGoods = registeredGoods;
		RxScrapData.Value = scrapData;
		RxScrapCount.Value = count;
	}

	public void Clear()
	{
		RegisteredGoods = null;
		RxScrapData.Value = null;
		RxScrapCount.Value = 0L;
		RxIsInvalid.Value = false;
	}
}
