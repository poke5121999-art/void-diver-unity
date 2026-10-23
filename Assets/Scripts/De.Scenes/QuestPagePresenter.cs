using GameCommon.Enum;
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

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiActions UiActions => _inputManager.UiActions;

	private void OnInject(QuestPageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		if (_selectedSlot != null)
		{
			_selectedSlot.RxIsSelected.Value = false;
		}
		_selectedSlot = null;
		_focusedSlot = null;
		UpdateInfo(null);
		_scrollerModel = new QuestScrollerModel(QuestScrollerPresenter.EFilterType.AllQuestsInProgress);
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(_disposable);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(_disposable);
		_view.Scroller.Inject(_scrollerModel);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnDisable()
	{
		_disposable.Dispose();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		UiActions.ShowDialog.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnShowDialog();
		}).AddTo(InputDisposable);
		UiActions.ToggleQuestHud.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnToggleQuestHudButtonClick();
		}).AddTo(InputDisposable);
		_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
		if (_infoPanelModel != null)
		{
			_infoPanelModel.RxIsInputEnabled.Value = _selectedSlot != null;
		}
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		if (_infoPanelModel != null)
		{
			_infoPanelModel.RxIsInputEnabled.Value = false;
		}
		base.OnInputDisabled();
	}

	private void Close()
	{
		_model.OnEscape.OnNext(Unit.Default);
	}

	private void OnBack()
	{
		if (_selectedSlot == null)
		{
			_model.OnEscape.OnNext(Unit.Default);
			return;
		}
		_selectedSlot.RxIsSelected.Value = false;
		_selectedSlot = null;
		UpdateInfo(_focusedSlot);
		if (IsInputEnabled)
		{
			_scrollerModel.RxIsInputEnabled.Value = true;
			_infoPanelModel.RxIsInputEnabled.Value = false;
		}
	}

	private void OnSlotClick(QuestSlotModel slotModel)
	{
		if (_selectedSlot != null)
		{
			_selectedSlot.RxIsSelected.Value = false;
		}
		_selectedSlot = slotModel;
		slotModel.RxIsSelected.Value = true;
		UpdateInfo(slotModel);
		if (IsInputEnabled)
		{
			_scrollerModel.RxIsInputEnabled.Value = false;
			_infoPanelModel.RxIsInputEnabled.Value = true;
		}
	}

	private void OnSlotEnter(QuestSlotModel slotModel)
	{
		_focusedSlot = slotModel;
		if (_selectedSlot == null)
		{
			UpdateInfo(_focusedSlot);
		}
	}

	private void OnSlotExit(QuestSlotModel slotModel)
	{
		_focusedSlot = null;
		if (_selectedSlot == null)
		{
			UpdateInfo(null);
		}
	}

	private void OnShowDialog()
	{
		QuestSlotModel questSlotModel = _selectedSlot ?? _focusedSlot;
		if (questSlotModel != null && _dialogReplayService.HasVisibleDialog(questSlotModel.QuestType, questSlotModel.TableId, false))
		{
			_popupManager.Open<ArchiveDialogPopupPresenter>(new ArchiveDialogPopupModel
			{
				QuestType = questSlotModel.QuestType,
				TableId = questSlotModel.TableId,
				DisplayFullDialog = false
			});
		}
	}

	private void OnToggleQuestHudButtonClick()
	{
		if (_model == null)
		{
			return;
		}
		QuestSlotModel questSlotModel = _selectedSlot ?? _focusedSlot;
		if (questSlotModel != null && questSlotModel.QuestType == EQuestType.Lounge)
		{
			int tableId = questSlotModel.TableId;
			if (!_playerManager.IsClearedLoungeQuest(tableId) && _loungeQuestManager.TryGetQuest(tableId, out var loungeQuest))
			{
				_loungeQuestManager.SetDisplayInHud(tableId, !loungeQuest.DisplayInHud);
			}
		}
	}

	private void UpdateInfo(QuestSlotModel slotModel)
	{
		if (slotModel == null)
		{
			_view.QuestInfoPanel.gameObject.SetActive(value: false);
			_view.Empty.SetActive(value: true);
			_view.KeyGuide_DisplayHud.SetActive(value: false);
			_view.KeyGuide_ShowDialog.SetActive(value: false);
			return;
		}
		bool flag = false;
		if (slotModel.QuestType == EQuestType.Lounge && _loungeQuestManager.TryGetQuest(slotModel.TableId, out var loungeQuest))
		{
			flag = loungeQuest.DisplayInHud;
		}
		bool active = _dialogReplayService.HasVisibleDialog(slotModel.QuestType, slotModel.TableId, false);
		_view.KeyGuide_DisplayHud.SetActive(flag);
		_view.KeyGuide_ShowDialog.SetActive(active);
		_view.QuestInfoPanel.gameObject.SetActive(value: true);
		_view.Empty.SetActive(value: false);
		_infoPanelModel = new QuestInfoPanelModel
		{
			QuestType = slotModel.QuestType,
			TableId = slotModel.TableId,
			DisplayFullDialog = false
		};
		_infoPanelModel.OnBack.Subscribe(delegate
		{
			OnBack();
		}).AddTo(_disposable);
		_view.QuestInfoPanel.Inject(_infoPanelModel);
	}
}
