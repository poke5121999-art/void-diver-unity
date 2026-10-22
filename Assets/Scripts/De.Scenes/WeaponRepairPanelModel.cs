using GameCommon.Data;
using UniRx;

public class WeaponRepairPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<Goods> RxSelectedGoods;

	public readonly Subject<Goods> OnConfirmRepair;

	public readonly Subject<Unit> OnGoodsSlotLeftClick;
}
