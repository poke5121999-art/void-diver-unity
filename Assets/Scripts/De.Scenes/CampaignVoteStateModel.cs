using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class CampaignVoteStateModel : Model
{
	public ulong ClientId;

	public ReactiveProperty<EVoteStateLayout> RxELayout;
}
