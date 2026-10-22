using GameCommon.Data;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class QuestInfoPanelPresenter : BasePanelPresenter
{
	private class ConditionProgress
	{
		public long Progress;

		public long Goal;

		public string Description;
	}

	[View]
	private QuestInfoPanelView _view;

	[InjectModel]
	private QuestInfoPanelModel _model;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private DialogReplayService _dialogReplayService;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiPadActions UiPadActions => default;

	private void Start()
	{
	}

	private void OnDisable()
	{
	}

	private void OnInject(QuestInfoPanelModel model)
	{
	}

	private void OnQuestUpdated((LoungeQuest oldValue, LoungeQuest newValue) _)
	{
	}

	private void UpdateCampaignInfo(int campaignId)
	{
	}

	private void UpdateMissionInfo(int missionId)
	{
	}

	private void UpdateLoungeQuestInfo(int questId)
	{
	}

	private void SetConditions(ConditionProgress[] progresses)
	{
	}

	private void SetRewardUis(Goods[] rewards)
	{
	}

	private void OnDisplayToggle(bool isOn)
	{
	}

	private void OnShowDialog()
	{
	}

	private void OnBackButtonClick()
	{
	}

	private void InitGamePadNav(bool hasDialog)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void OnUpdateScroll(Vector2 vector2)
	{
	}
}
