using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class QuestPagePresenter : BasePanelPresenter
{
	[InjectModel]
	private QuestPageModel _model;

	[View]
	private QuestPageView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private DialogReplayService _dialogReplayService;

	private QuestScrollerModel _scrollerModel;

	private QuestInfoPanelModel _infoPanelModel;

	private CompositeDisposable _disposable;

	private QuestSlotModel _selectedSlot;

	private QuestSlotModel _focusedSlot;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiActions UiActions => default;

	private void OnInject(QuestPageModel model)
	{
	}

	private void OnDisable()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void Close()
	{
	}

	private void OnBack()
	{
	}

	private void OnSlotClick(QuestSlotModel slotModel)
	{
	}

	private void OnSlotEnter(QuestSlotModel slotModel)
	{
	}

	private void OnSlotExit(QuestSlotModel slotModel)
	{
	}

	private void OnShowDialog()
	{
	}

	private void OnToggleQuestHudButtonClick()
	{
	}

	private void UpdateInfo(QuestSlotModel slotModel)
	{
	}
}
