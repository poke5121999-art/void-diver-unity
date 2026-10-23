using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using TMPro;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
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

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiPadActions UiPadActions => InputManager.UiPadActions;

	private void Start()
	{
		_view.ButtonLayout_ShowDialogButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnShowDialog();
		}).AddTo(this);
		_view.BackButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBackButtonClick();
		}).AddTo(this);
		_view.Scroll.decelerationRate = Const.Scroller.DECELERATION_RATE;
		_view.Scroll.scrollSensitivity = Const.Scroller.SCROLL_SENSITIVITY;
	}

	private void OnDisable()
	{
		_disposable.Dispose();
	}

	private void OnInject(QuestInfoPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.Title_Type.text = model.QuestType.ToLocalText();
		switch (model.QuestType)
		{
		case EQuestType.Campaign:
			UpdateCampaignInfo(model.TableId);
			break;
		case EQuestType.Mission:
			UpdateMissionInfo(model.TableId);
			break;
		case EQuestType.Lounge:
			UpdateLoungeQuestInfo(model.TableId);
			break;
		}
		if (model.QuestType == EQuestType.Lounge)
		{
			if (_loungeQuestManager.TryGetQuest(model.TableId, out var loungeQuest))
			{
				_view.ButtonLayout_DisplayToggle.gameObject.SetActive(value: true);
				_view.ButtonLayout_DisplayToggle.isOn = loungeQuest.DisplayInHud;
				_view.ButtonLayout_DisplayToggle.OnValueChangedWithSfx().Subscribe(OnDisplayToggle).AddTo(_disposable);
				_loungeQuestManager.OnQuestUpdated.Subscribe(OnQuestUpdated).AddTo(_disposable);
			}
		}
		else
		{
			_view.ButtonLayout_DisplayToggle.gameObject.SetActive(value: false);
		}
		bool flag = _dialogReplayService.HasVisibleDialog(_model.QuestType, _model.TableId, _model.DisplayFullDialog);
		_view.ButtonLayout_ShowDialogButton.gameObject.SetActive(flag);
		InitGamePadNav(flag);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnQuestUpdated((LoungeQuest oldValue, LoungeQuest newValue) _)
	{
		if (_.newValue.Id == _model.TableId && _view.ButtonLayout_DisplayToggle.isOn != _.newValue.DisplayInHud)
		{
			_view.ButtonLayout_DisplayToggle.isOn = _.newValue.DisplayInHud;
		}
	}

	private void UpdateCampaignInfo(int campaignId)
	{
		_view.Title_Level.gameObject.SetActive(value: true);
		TCampaign tCampaign = TCampaign.Table[campaignId];
		_view.Title_Text.text = tCampaign.Name;
		_view.Title_Level.text = string.Format("Lv. {0}", tCampaign.RecommendedLevel);
		_view.Desc.text = tCampaign.Desc;
		TCampaignTask[] array = TCampaignTask.GroupTable[campaignId];
		ConditionProgress[] progresses = array?.Select(delegate(TCampaignTask task)
		{
			long progress;
			if (_campaignManager.CampaignTasks.TryGetValue(task.Id, out var value))
			{
				progress = value.Progress;
			}
			else
			{
				progress = (_playerManager.IsClearedCampaign(task.CampaignId) ? task.Goal : 0);
			}
			return new ConditionProgress
			{
				Progress = progress,
				Goal = task.Goal,
				Description = ((task.EventCondition.Type == EEventConditionType.LuaProgress) ? task.Desc : task.EventCondition.ToDescription())
			};
		}).ToArray();
		SetConditions(progresses);
		bool flag = _playerManager.IsClearedCampaign(campaignId);
		Goods[] rewards = tCampaign.GetRewards(!flag)?.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();
		SetRewardUis(rewards);
	}

	private void UpdateMissionInfo(int missionId)
	{
		_view.Title_Level.gameObject.SetActive(value: false);
		TMission tMission = TMission.Table[missionId];
		_view.Title_Text.text = tMission.Name;
		_view.Desc.text = tMission.Desc;
		long progress;
		if (_missionManager.Missions.TryGetValue(missionId, out var value))
		{
			progress = value.Progress;
		}
		else
		{
			progress = (_playerManager.IsClearedMission(missionId) ? tMission.Goal : 0);
		}
		SetConditions(new ConditionProgress[1]
		{
			new ConditionProgress
			{
				Progress = progress,
				Goal = tMission.Goal,
				Description = tMission.Desc
			}
		});
		Goods[] rewards = tMission.Rewards?.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();
		SetRewardUis(rewards);
	}

	private void UpdateLoungeQuestInfo(int questId)
	{
		_view.Title_Level.gameObject.SetActive(value: false);
		TLoungeQuest tLoungeQuest = TLoungeQuest.Table[questId];
		_view.Title_Text.text = tLoungeQuest.Name;
		_view.Desc.text = tLoungeQuest.Desc;
		TLoungeQuestTask[] array = TLoungeQuestTask.GroupTable[questId];
		ConditionProgress[] progresses = array?.Select(delegate(TLoungeQuestTask task)
		{
			long num = _loungeQuestManager.GetQuestTaskProgress(task.Id);
			if (num == 0L && _playerManager.IsClearedLoungeQuest(questId))
			{
				num = task.Goal;
			}
			return new ConditionProgress
			{
				Progress = num,
				Goal = task.Goal,
				Description = task.Desc
			};
		}).ToArray();
		SetConditions(progresses);
		Goods[] rewards = tLoungeQuest.Rewards?.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();
		SetRewardUis(rewards);
	}

	private void SetConditions(ConditionProgress[] progresses)
	{
		GameObject gameObject = _view.Conditions.First().transform.parent.gameObject;
		if (progresses == null || !progresses.Any())
		{
			gameObject.SetActive(value: false);
			return;
		}
		gameObject.SetActive(value: true);
		for (int i = 0; i < _view.Conditions.Length; i++)
		{
			GameObject gameObject2 = _view.Conditions[i];
			if (i < progresses.Length)
			{
				gameObject2.SetActive(value: true);
				ConditionProgress conditionProgress = progresses[i];
				gameObject2.FindChildComponent<TextMeshProUGUI>("Text").text = conditionProgress.Description;
				gameObject2.FindChildComponent<TextMeshProUGUI>("Progress").text = string.Format("{0}/{1}", conditionProgress.Progress, conditionProgress.Goal);
			}
			else
			{
				gameObject2.SetActive(value: false);
			}
		}
	}

	private void SetRewardUis(Goods[] rewards)
	{
		if (rewards == null || !rewards.Any())
		{
			_view.Reward.SetActive(value: false);
			return;
		}
		_view.Reward.SetActive(value: true);
		for (int i = 0; i < _view.Reward_Slots.Length; i++)
		{
			GameObject gameObject = _view.Reward_Slots[i];
			if (i < rewards.Length)
			{
				gameObject.SetActive(value: true);
				InventoryGoodsSlotPresenter component = gameObject.GetComponent<InventoryGoodsSlotPresenter>();
				InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(rewards[i], InventoryGoodsSlotPresenter.ELayout.Normal);
				inventoryGoodsSlotModel.UseTooltip = true;
				inventoryGoodsSlotModel.ShowItemCount = true;
				component.Inject(inventoryGoodsSlotModel);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	private void OnDisplayToggle(bool isOn)
	{
		if (_model != null && _model.QuestType == EQuestType.Lounge)
		{
			_loungeQuestManager.SetDisplayInHud(_model.TableId, isOn);
		}
	}

	private void OnShowDialog()
	{
		_popupManager.Open<ArchiveDialogPopupPresenter>(new ArchiveDialogPopupModel
		{
			QuestType = _model.QuestType,
			TableId = _model.TableId,
			DisplayFullDialog = _model.DisplayFullDialog
		});
	}

	private void OnBackButtonClick()
	{
		_model.OnBack.OnNext(Unit.Default);
	}

	private void InitGamePadNav(bool hasDialog)
	{
		ClearNav();
		bool flag = _model.QuestType == EQuestType.Lounge;
		Selectable to;
		if (flag && hasDialog)
		{
			ConnectNav(_view.ButtonLayout_DisplayToggle, ENavDir.Right, _view.ButtonLayout_ShowDialogButton);
			ConnectNav(_view.ButtonLayout_ShowDialogButton, ENavDir.Left, _view.ButtonLayout_DisplayToggle);
			ConnectNav(_view.ButtonLayout_ShowDialogButton, ENavDir.Right, _view.BackButton);
			to = _view.ButtonLayout_ShowDialogButton;
		}
		else if (!flag)
		{
			if (hasDialog)
			{
				ConnectNav(_view.ButtonLayout_ShowDialogButton, ENavDir.Right, _view.BackButton);
				ConnectNav(_view.BackButton, ENavDir.Left, _view.ButtonLayout_ShowDialogButton);
				to = _view.ButtonLayout_ShowDialogButton;
			}
			else
			{
				to = _view.BackButton;
			}
		}
		else
		{
			ConnectNav(_view.ButtonLayout_DisplayToggle, ENavDir.Right, _view.BackButton);
			to = _view.ButtonLayout_DisplayToggle;
		}
		ConnectNav(_view.BackButton, ENavDir.Left, to);
		SetDefaultNav(_view.ButtonLayout_DisplayToggle);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		Observable.EveryUpdate().Select((long _) => UiPadActions.ScrollRect.ReadValue<Vector2>()).Subscribe(OnUpdateScroll)
			.AddTo(InputDisposable);
	}

	private void OnUpdateScroll(Vector2 vector2)
	{
		if (!(Mathf.Abs(vector2.y) < 0.1f))
		{
			ScrollRect scroll = _view.Scroll;
			scroll.verticalNormalizedPosition = vector2.y * Time.deltaTime + scroll.verticalNormalizedPosition;
		}
	}
}
