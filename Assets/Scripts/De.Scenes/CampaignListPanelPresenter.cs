using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class CampaignListPanelPresenter : Presenter
{
	[View]
	private CampaignListPanelView _view;

	[InjectModel]
	private CampaignListPanelModel _model;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameContext _gameContext;

	private CampaignScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	private void OnInject(CampaignListPanelModel model)
	{
	}

	private void OnDestroy()
	{
	}

	private void OnCampaignClick(CampaignSlotModel slotModel)
	{
	}

	private void OnInputEnabled()
	{
	}

	private void OnInputDisabled()
	{
	}
}
