using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class CampaignListPanelModel : Model
{
	public ECampaignCategory Category;

	public readonly Subject<int> OnSlotClick = new Subject<int>();

	public readonly ReactiveProperty<bool> RxIsInputEnabled = new ReactiveProperty<bool>(false);
}
