using GameCommon.Enum;
using UniRx;

public class StashFilterPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<EGoodsType[]> RxAllowedSubTypes;

	public readonly Subject<EGoodsType[]> OnUserToggled;
}
