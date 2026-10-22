using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using VContainer;

public class InGameWatchingPanelPresenter : Presenter
{
	[View]
	private InGameWatchingPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameInputManager _inputManager;

	private bool _isWatching;

	private InputActions.InGameActions InGameActions => default;

	private void Start()
	{
	}

	private void SubscribeCharacterState(CharacterModel characterModel)
	{
	}

	private void OnMyCharacterStateChanged(EActionState state)
	{
	}

	private void OnStageEnded()
	{
	}

	private void OnChangeViewTarget()
	{
	}
}
