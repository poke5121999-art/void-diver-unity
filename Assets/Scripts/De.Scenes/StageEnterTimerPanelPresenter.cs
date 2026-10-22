using NemoLib.Mvp;
using VContainer;

public class StageEnterTimerPanelPresenter : Presenter
{
	[View]
	private StageEnterTimerPanelView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameManager _gameManager;

	private bool _isTimerOn;

	private float _enterTime;

	private bool _isFading;

	private void Start()
	{
	}

	private void StartTimer(NtfAllPlayerReadyToEnterStage message)
	{
	}

	private void CancelTimer()
	{
	}

	private void FixedUpdate()
	{
	}
}
