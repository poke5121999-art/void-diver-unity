using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class CampaignVoteStatePresenter : Presenter
{
	[View]
	private CampaignVoteStateView _view;

	[InjectModel]
	private CampaignVoteStateModel _model;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private void OnInject(CampaignVoteStateModel model)
	{
	}

	private void OnDestroy()
	{
	}

	private void OnLayoutChanged(EVoteStateLayout layout)
	{
	}
}
