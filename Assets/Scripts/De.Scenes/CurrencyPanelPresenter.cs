using NemoLib.Mvp;
using VContainer;

public class CurrencyPanelPresenter : Presenter
{
	[View]
	private CurrencyPanelView _view;

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
