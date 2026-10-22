using NemoLib.Mvp;
using UniRx;
using VContainer;

public class HudCampaignQuestSlotPresenter : Presenter
{
	[View]
	private HudCampaignQuestSlotView _view;

	[InjectModel]
	private HudCampaignQuestSlotModel _model;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameContext _gameContext;

	private CompositeDisposable _disposable;

	private void OnInject(HudCampaignQuestSlotModel model)
	{
	}

	private void OnDestroy()
	{
	}

	private void UpdateUi()
	{
	}
}
