using GameCommon.Data;
using UniRx;

public class WeaponRepairPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<Goods> RxSelectedGoods = new ReactiveProperty<Goods>();

	public readonly Subject<Goods> OnConfirmRepair = new Subject<Goods>();

	public readonly Subject<Unit> OnGoodsSlotLeftClick = new Subject<Unit>();
}
