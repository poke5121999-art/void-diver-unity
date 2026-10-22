using GameCommon.Enum;
using UniRx;

public class StashFilterPanelModel : BasePanelModel
{
	public readonly ReactiveProperty<EGoodsType[]> RxAllowedSubTypes = new ReactiveProperty<EGoodsType[]>();

	public readonly Subject<EGoodsType[]> OnUserToggled = new Subject<EGoodsType[]>();
}
