using UnityEngine;
using UniRx;
using GameCommon.Table;
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
		UpdateLevelAndExp(_playerManager.Level, _playerManager.Exp);
		_uiEventSystem.OnUpdateLevelAndExp.Subscribe(delegate((int level, long exp) _)
		{
			UpdateLevelAndExp(_.level, _.exp);
		}).AddTo(this);
		_gameManager.RxStageState.Subscribe(OnStageStateChanged).AddTo(this);
		_gameContext.RxRoomDifficulty.Subscribe(OnRoomDifficultyChanged).AddTo(this);
	}

	private void OnStageStateChanged(EStageState state)
	{
		bool active = state == EStageState.Playing && _gameManager.IsInLounge;
		_view.Contents.SetActive(active);
	}

	private void OnRoomDifficultyChanged(ERoomDifficulty difficulty)
	{
		_view.IconStory.gameObject.SetActive(difficulty == ERoomDifficulty.Story);
		_view.IconJunior.gameObject.SetActive(difficulty == ERoomDifficulty.Junior);
		_view.IconClassic.gameObject.SetActive(difficulty == ERoomDifficulty.Classic);
		_view.IconVeteran.gameObject.SetActive(difficulty == ERoomDifficulty.Veteran);
	}

	private void UpdateLevelAndExp(int level, long exp)
	{
		int intValue = TConst.LevelMax.IntValue;
		long num = 0L;
		bool active = false;
		if (level < intValue)
		{
			num = TLevel.Table[level + 1].NeedExp;
			active = exp >= num;
		}
		_view.LevelText.text = string.Format("Lv. {0}", level);
		_view.ExpText.text = ((level < intValue) ? string.Format("Exp {0} / {1}", exp, num) : "Max");
		_view.ExpSlider.value = ((level < intValue) ? Mathf.Min((float)exp / (float)num, 1f) : 1f);
		_view.LevelUpTag.SetActive(active);
	}
}
