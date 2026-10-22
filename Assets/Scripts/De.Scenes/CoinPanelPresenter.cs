using NemoLib.Mvp;
using VContainer;

public class CoinPanelPresenter : Presenter
{
	[View]
	private CoinPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void UpdateGoldUi(bool applyImmediately = false)
	{
	}
}
