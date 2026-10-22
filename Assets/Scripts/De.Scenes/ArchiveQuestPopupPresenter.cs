using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class ArchiveQuestPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private ArchiveQuestPopupModel _model;

	[View]
	private ArchiveQuestPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private DialogReplayService _dialogReplayService;

	private QuestScrollerModel _scrollerModel;

	private QuestInfoPanelModel _panelModel;

	private QuestSlotModel _selectedSlot;

	private QuestSlotModel _focusedSlot;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Title.text = EArchiveCategory.Quest.ToLocalText();
		_view.TitleSub.text = GetSubTitle(_model.FilterType);
		UpdateInfo(null);
		_scrollerModel = new QuestScrollerModel(_model.FilterType);
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(this);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(this);
		_view.Scroller.GetComponent<QuestScrollerPresenter>().Inject(_scrollerModel);
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
		_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
		if (_panelModel != null)
		{
			_panelModel.RxIsInputEnabled.Value = _selectedSlot != null;
		}
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_scrollerModel.RxIsInputEnabled.Value = false;
		if (_panelModel != null)
		{
			_panelModel.RxIsInputEnabled.Value = false;
		}
	}

	private void OnBack()
	{
		if (_selectedSlot == null)
		{
			Close();
			return;
		}
		UpdateSelectedObject(null);
		UpdateInfo(_focusedSlot);
		Logger.Debug(Color.red);
	}

	private void OnSlotClick(QuestSlotModel slotModel)
	{
		UpdateSelectedObject(slotModel);
		UpdateInfo(slotModel);
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
		if (questSlotModel != null)
		{
			EQuestType questType = questSlotModel.QuestType;
			int tableId = questSlotModel.TableId;
			if (_playerManager.IsQuestCleared(questType, tableId) && _dialogReplayService.HasVisibleDialog(questType, tableId, true))
			{
				_popupManager.Open<ArchiveDialogPopupPresenter>(new ArchiveDialogPopupModel
				{
					QuestType = questType,
					TableId = tableId,
					DisplayFullDialog = true
				});
			}
		}
	}

	private void UpdateSelectedObject(QuestSlotModel slotModel)
	{
		if (_selectedSlot != null)
		{
			_selectedSlot.RxIsSelected.Value = false;
		}
		if (slotModel != null)
		{
			slotModel.RxIsSelected.Value = true;
		}
		_selectedSlot = slotModel;
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
			if (_panelModel != null)
			{
				_panelModel.RxIsInputEnabled.Value = _selectedSlot != null;
			}
		}
	}

	private void UpdateInfo(QuestSlotModel slotModel)
	{
		if (slotModel == null)
		{
			_view.QuestInfoPanel.gameObject.SetActive(false);
			_view.Empty.SetActive(true);
			_view.KeyGuide_ShowDialog.SetActive(false);
			return;
		}
		_view.QuestInfoPanel.gameObject.SetActive(true);
		_view.Empty.SetActive(false);
		bool active = _dialogReplayService.HasVisibleDialog(slotModel.QuestType, slotModel.TableId, false);
		_view.KeyGuide_ShowDialog.SetActive(active);
		_panelModel = new QuestInfoPanelModel
		{
			QuestType = slotModel.QuestType,
			TableId = slotModel.TableId,
			DisplayFullDialog = true
		};
		_panelModel.OnBack.Subscribe(delegate
		{
			OnBack();
		}).AddTo(this);
		if (IsInputEnable)
		{
			_panelModel.RxIsInputEnabled.Value = _selectedSlot != null;
		}
		_view.QuestInfoPanel.Inject(_panelModel);
	}

	private string GetSubTitle(QuestScrollerPresenter.EFilterType filterType)
	{
		switch (filterType)
		{
		case QuestScrollerPresenter.EFilterType.ClearedCampaign:
			return EQuestType.Campaign.ToLocalText();
		case QuestScrollerPresenter.EFilterType.ClearedMission:
			return EQuestType.Mission.ToLocalText();
		case QuestScrollerPresenter.EFilterType.ClearedLounge:
			return EQuestType.Lounge.ToLocalText();
		default:
			return string.Empty;
		}
	}
}
