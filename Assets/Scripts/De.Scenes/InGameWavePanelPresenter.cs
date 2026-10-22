using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class InGameWavePanelPresenter : Presenter
{
	[View]
	private InGameWavePanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameWaveManager _waveManager;

	[Inject]
	private SeriLogger _logger;

	private GameWave _currentWave;

	private CompositeDisposable _disposable;

	private const string _alertColor = "#c46e5e";

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	private void OnCurrentWaveChange(GameWave wave)
	{
	}

	private void OnExitStateChanged(EExitState state)
	{
	}
}
