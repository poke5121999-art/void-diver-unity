using UniRx;

public class CampaignDetailPanelModel : BasePanelModel
{
	public int CampaignId;

	public readonly Subject<Unit> OnConfirm = new Subject<Unit>();
}
