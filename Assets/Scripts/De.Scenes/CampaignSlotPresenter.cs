using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class CampaignSlotPresenter : BaseSlotPresenter<CampaignSlotModel>
{
	[InjectModel]
	private CampaignSlotModel _model;

	[View]
	private CampaignSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameCampaignManager _gameCampaignManager;

	[Inject]
	private AssetManager _assetManager;

	protected override CampaignSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(CampaignSlotModel model)
	{
	}
}
