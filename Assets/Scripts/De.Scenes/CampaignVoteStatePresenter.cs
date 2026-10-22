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
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		GamePlayer gamePlayer = _gameContext.Players[model.ClientId];
		_view.Yes_Host.SetActive(gamePlayer.IsHostPlayer);
		_view.Yes_Icon.SetActive(!gamePlayer.IsHostPlayer);
		_view.CharacterImage.sprite = _assetManager.GetIconUnitCharacter(gamePlayer.Character.Id, "Default");
		model.RxELayout.Subscribe(OnLayoutChanged).AddTo(_disposable);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void OnLayoutChanged(EVoteStateLayout layout)
	{
		_view.Yes.gameObject.SetActive(layout == EVoteStateLayout.Yes);
		_view.No.SetActive(layout == EVoteStateLayout.No);
		_view.Waiting.SetActive(layout == EVoteStateLayout.Waiting);
	}
}
