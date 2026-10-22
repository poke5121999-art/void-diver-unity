using GameCommon.Enum;
using UniRx;

public class ShopProductSlotModel : BaseSlotModel<ShopProductSlotModel>
{
	public int Id;

	public readonly ReactiveProperty<EButtonState> RxButtonState;
}
