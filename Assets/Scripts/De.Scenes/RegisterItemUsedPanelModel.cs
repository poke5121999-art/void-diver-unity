using GameCommon.Data;
using UniRx;

public class RegisterItemUsedPanelModel : BasePanelModel
{
	public readonly Subject<GoodsSlotModel> OnSlotEnter = new Subject<GoodsSlotModel>();

	public readonly Subject<GoodsSlotModel> OnSlotExit = new Subject<GoodsSlotModel>();

	public readonly Subject<Unit> OnConfirmButtonClick = new Subject<Unit>();

	public readonly Subject<Unit> OnSlotClick = new Subject<Unit>();

	public readonly ReactiveProperty<Goods> RxGoods = new ReactiveProperty<Goods>();

	public readonly ReactiveProperty<bool> RxIsInvalid = new ReactiveProperty<bool>();

	public string Title;

	public GoodsSlotModel SlotModel { get; set; }

	public void SetGoods(Goods goods)
	{
		RxGoods.Value = goods;
	}

	public void Clear()
	{
		RxGoods.Value = null;
		RxIsInvalid.Value = false;
	}
}
