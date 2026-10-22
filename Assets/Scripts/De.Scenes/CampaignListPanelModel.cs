using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class CampaignListPanelModel : Model
{
	public ECampaignCategory Category;

	public readonly Subject<int> OnSlotClick;

	public readonly ReactiveProperty<bool> RxIsInputEnabled;
}
