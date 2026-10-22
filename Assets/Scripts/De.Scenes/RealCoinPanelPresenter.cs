using NemoLib.Mvp;
using VContainer;

public class RealCoinPanelPresenter : Presenter
{
	[View]
	private RealCoinPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void UpdateRealCoinUi(bool applyImmediately = false)
	{
	}
}
