using GameCommon.Data;
using UniRx;

public class RegisterItemUsedPanelModel : BasePanelModel
{
	public readonly Subject<GoodsSlotModel> OnSlotEnter;

	public readonly Subject<GoodsSlotModel> OnSlotExit;

	public readonly Subject<Unit> OnConfirmButtonClick;

	public readonly Subject<Unit> OnSlotClick;

	public readonly ReactiveProperty<Goods> RxGoods;

	public readonly ReactiveProperty<bool> RxIsInvalid;

	public string Title;

	public GoodsSlotModel SlotModel { get; set; }

	public void SetGoods(Goods goods)
	{
	}

	public void Clear()
	{
	}
}
