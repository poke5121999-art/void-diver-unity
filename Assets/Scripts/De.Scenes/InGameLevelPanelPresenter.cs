using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using VContainer;

public class InGameLevelPanelPresenter : Presenter
{
	[View]
	private InGameLevelPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameUiEventSystem _uiEventSystem;

	private void Start()
	{
	}

	private void OnStageStateChanged(EStageState state)
	{
	}

	private void OnRoomDifficultyChanged(ERoomDifficulty difficulty)
	{
	}

	private void UpdateLevelAndExp(int level, long exp)
	{
	}
}
