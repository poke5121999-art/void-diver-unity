using System.Linq;
using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using GameCommon.Table;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StageResultPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private StageResultPopupModel _model;

	[View]
	private StageResultPopupView _view;

	[Inject]
	private GameCampaignManager _campaignManager;

	private ScrollRect _scrollRect;

	private bool _isAutoScrolling;

	private bool _isFastForward;

	private bool _userScrollEnabled;

	private const float NORMAL_SCROLL_SPEED = 0.5f;

	private const float FAST_SCROLL_SPEED = 20f;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "EscapeResult";

	private void Start()
	{
		_view.ScrollView.decelerationRate = Const.Scroller.DECELERATION_RATE;
		_view.ScrollView.scrollSensitivity = Const.Scroller.SCROLL_SENSITIVITY;
		_view.ScrollView.movementType = ScrollRect.MovementType.Clamped;
	}

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		for (int i = 0; i < _view.Player_List.Length; i++)
		{
			GameObject gameObject = _view.Player_List[i];
			StageResultPlayerInfoPresenter component = gameObject.GetComponent<StageResultPlayerInfoPresenter>();
			if (i < _model.StageResultInfo.PlayerInfos.Length)
			{
				StageResultPlayerInfoModel stageResultPlayerInfoModel = new StageResultPlayerInfoModel();
				stageResultPlayerInfoModel.PlayerInfo = _model.StageResultInfo.PlayerInfos[i];
				component.Inject(stageResultPlayerInfoModel);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
		TCampaignTask[] tasks = TCampaignTask.GroupTable[_model.StageResultInfo.CampaignId];
		for (int j = 0; j < _view.Quest_List.Length; j++)
		{
			GameObject gameObject2 = _view.Quest_List[j];
			if (j < tasks.Length)
			{
				gameObject2.SetActive(value: true);
				StageResultQuestSlotPresenter component2 = gameObject2.GetComponent<StageResultQuestSlotPresenter>();
				StageResultQuestSlotModel stageResultQuestSlotModel = new StageResultQuestSlotModel();
				stageResultQuestSlotModel.CampaignTaskId = tasks[j].Id;
				component2.Inject(stageResultQuestSlotModel);
			}
			else
			{
				gameObject2.SetActive(value: false);
			}
		}
		for (int k = 0; k < _view.Paradox_List.Length; k++)
		{
			GameObject gameObject3 = _view.Paradox_List[k];
			if (_model.StageResultInfo.PlayerInfos == null || k >= _model.StageResultInfo.PlayerInfos.Length)
			{
				gameObject3.SetActive(value: false);
				continue;
			}
			StageResultPlayerInfo stageResultPlayerInfo = _model.StageResultInfo.PlayerInfos[k];
			if ((stageResultPlayerInfo.GainedParadoxIds == null || stageResultPlayerInfo.GainedParadoxIds.Length == 0) && (stageResultPlayerInfo.RemovedParadoxIds == null || stageResultPlayerInfo.RemovedParadoxIds.Length == 0))
			{
				gameObject3.SetActive(value: false);
				continue;
			}
			gameObject3.SetActive(value: true);
			StageResultPlayerParadoxGroupPresenter component3 = gameObject3.GetComponent<StageResultPlayerParadoxGroupPresenter>();
			StageResultPlayerParadoxGroupModel stageResultPlayerParadoxGroupModel = new StageResultPlayerParadoxGroupModel();
			stageResultPlayerParadoxGroupModel.UserName = stageResultPlayerInfo.UserName;
			stageResultPlayerParadoxGroupModel.GainedParadoxIds = stageResultPlayerInfo.GainedParadoxIds ?? System.Array.Empty<int>();
			stageResultPlayerParadoxGroupModel.RemovedParadoxIds = stageResultPlayerInfo.RemovedParadoxIds ?? System.Array.Empty<int>();
			component3.Inject(stageResultPlayerParadoxGroupModel);
		}
		for (int l = 0; l < _view.GainedGoods_List.Length; l++)
		{
			GameObject gameObject4 = _view.GainedGoods_List[l];
			if (_model.StageResultInfo.PlayerInfos == null || l >= _model.StageResultInfo.PlayerInfos.Length)
			{
				gameObject4.SetActive(value: false);
				continue;
			}
			StageResultPlayerInfo stageResultPlayerInfo2 = _model.StageResultInfo.PlayerInfos[l];
			GameCommon.Data.Goods[] items = stageResultPlayerInfo2.HighGradeGoods ?? System.Array.Empty<GameCommon.Data.Goods>();
			if (items.Length == 0)
			{
				gameObject4.SetActive(value: false);
				continue;
			}
			gameObject4.SetActive(value: true);
			StageResultPlayerItemGroupPresenter component4 = gameObject4.GetComponent<StageResultPlayerItemGroupPresenter>();
			StageResultPlayerItemGroupModel stageResultPlayerItemGroupModel = new StageResultPlayerItemGroupModel();
			stageResultPlayerItemGroupModel.UserName = stageResultPlayerInfo2.UserName;
			stageResultPlayerItemGroupModel.Items = items;
			component4.Inject(stageResultPlayerItemGroupModel);
		}
		for (int m = 0; m < _view.LostGoods_List.Length; m++)
		{
			GameObject gameObject5 = _view.LostGoods_List[m];
			if (_model.StageResultInfo.PlayerInfos == null || m >= _model.StageResultInfo.PlayerInfos.Length)
			{
				gameObject5.SetActive(value: false);
				continue;
			}
			StageResultPlayerInfo stageResultPlayerInfo3 = _model.StageResultInfo.PlayerInfos[m];
			GameCommon.Data.Goods[] items2 = stageResultPlayerInfo3.LostGoods ?? System.Array.Empty<GameCommon.Data.Goods>();
			if (items2.Length == 0)
			{
				gameObject5.SetActive(value: false);
				continue;
			}
			gameObject5.SetActive(value: true);
			StageResultPlayerItemGroupPresenter component5 = gameObject5.GetComponent<StageResultPlayerItemGroupPresenter>();
			StageResultPlayerItemGroupModel stageResultPlayerItemGroupModel2 = new StageResultPlayerItemGroupModel();
			stageResultPlayerItemGroupModel2.UserName = stageResultPlayerInfo3.UserName;
			stageResultPlayerItemGroupModel2.Items = items2;
			component5.Inject(stageResultPlayerItemGroupModel2);
		}
		for (int n = 0; n < _view.Exp_List.Length; n++)
		{
			GameObject gameObject6 = _view.Exp_List[n];
			if (_model.StageResultInfo.PlayerInfos != null && n < _model.StageResultInfo.PlayerInfos.Length)
			{
				StageResultPlayerInfo playerInfo = _model.StageResultInfo.PlayerInfos[n];
				StageResultLocalInfo stageResultLocalInfo = _model.StageResultInfo.LocalInfos?.FirstOrDefault((StageResultLocalInfo li) => li.ClientId == playerInfo.ClientId);
				if (stageResultLocalInfo != null)
				{
					StageResultPlayerExpSlotPresenter component6 = gameObject6.GetComponent<StageResultPlayerExpSlotPresenter>();
					StageResultPlayerExpSlotModel stageResultPlayerExpSlotModel = new StageResultPlayerExpSlotModel();
					stageResultPlayerExpSlotModel.UserName = playerInfo.UserName;
					stageResultPlayerExpSlotModel.ExpFromRewardBox = stageResultLocalInfo.BaseExpFromRewardBox;
					stageResultPlayerExpSlotModel.ExpFromMonsterKill = stageResultLocalInfo.BaseExpFromMonsterKill;
					stageResultPlayerExpSlotModel.ExpFromEndStage = stageResultLocalInfo.BaseExpFromEndStage;
					stageResultPlayerExpSlotModel.ExpFromDifficulty = stageResultLocalInfo.DifficultyBonusExp;
					stageResultPlayerExpSlotModel.ExpFromTalent = stageResultLocalInfo.TalentBonusExp;
					component6.Inject(stageResultPlayerExpSlotModel);
					gameObject6.SetActive(value: true);
					continue;
				}
			}
			gameObject6.SetActive(value: false);
		}
		StageResultPlayerInfo[] playerInfos = _model.StageResultInfo?.PlayerInfos;
		bool hasGained = playerInfos != null && playerInfos.Any((StageResultPlayerInfo p) => p.HighGradeGoods != null && p.HighGradeGoods.Length != 0);
		bool hasLost = playerInfos != null && playerInfos.Any((StageResultPlayerInfo p) => p.LostGoods != null && p.LostGoods.Length != 0);
		if (_view.GainedGoods != null)
		{
			_view.GainedGoods.gameObject.SetActive(hasGained);
		}
		if (_view.LostGoods != null)
		{
			_view.LostGoods.gameObject.SetActive(hasLost);
		}
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			OnConfirmButtonClicked();
		}).AddTo(this);
		_view.SkipButton.OnClickWithSfx().Subscribe(delegate
		{
			OnSkipPressed();
		}).AddTo(this);
		StartAutoScrollAsync();
		SetDefaultNav(_view.Confirm_Button);
	}

	private void InitGamePadNav()
	{
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Skip.OnClickAsObservable().Subscribe(delegate
			{
				OnSkipPressed();
			}).AddTo(InputDisposable);
		}
	}

	private void OnConfirmButtonClicked()
	{
		if (_isAutoScrolling)
		{
			if (!_isFastForward)
			{
				_isFastForward = true;
			}
			return;
		}
		Close();
	}

	private void OnSkipPressed()
	{
		if (_isAutoScrolling && !_isFastForward)
		{
			_isFastForward = true;
		}
	}

	private async UniTaskVoid StartAutoScrollAsync()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(_view.ScrollView.transform as RectTransform);
		await UniTask.DelayFrame(1);
		if (_isAutoScrolling)
		{
			return;
		}
		_scrollRect = _view.ScrollView.GetComponent<ScrollRect>();
		if (_scrollRect == null)
		{
			_isAutoScrolling = false;
			_view.Confirm.SetActive(value: true);
			_view.SkipButton.gameObject.SetActive(value: false);
			return;
		}
		if (_scrollRect.content == null || !(_scrollRect.content.rect.height > _scrollRect.viewport.rect.height + 1f))
		{
			_scrollRect.verticalNormalizedPosition = 1f;
			_isAutoScrolling = false;
			OnPresentationComplete();
			return;
		}
		_isAutoScrolling = true;
		_isFastForward = false;
		_userScrollEnabled = false;
		_scrollRect.verticalNormalizedPosition = 1f;
		_scrollRect.enabled = false;
		_view.Confirm.SetActive(value: false);
		_view.SkipButton.gameObject.SetActive(value: true);
		while (true)
		{
			if (_scrollRect.verticalNormalizedPosition <= 0.01f)
			{
				_scrollRect.verticalNormalizedPosition = 0f;
				_isAutoScrolling = false;
				OnPresentationComplete();
				break;
			}
			if (destroyCancellationToken.IsCancellationRequested)
			{
				break;
			}
			await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
			float num = (_isFastForward ? 20f : 0.5f);
			_scrollRect.verticalNormalizedPosition = Mathf.Clamp01(_scrollRect.verticalNormalizedPosition - Time.unscaledDeltaTime * num);
		}
	}

	private bool IsContentOverflow()
	{
		if (_scrollRect == null)
		{
			return false;
		}
		RectTransform content = _scrollRect.content;
		if (content == null)
		{
			return false;
		}
		return _scrollRect.viewport.rect.height + 1f < content.rect.height;
	}

	private void OnPresentationComplete()
	{
		if (_scrollRect == null)
		{
			return;
		}
		if (!_userScrollEnabled)
		{
			_scrollRect.enabled = true;
			_userScrollEnabled = true;
		}
		_view.Confirm.SetActive(value: true);
		_view.SkipButton.gameObject.SetActive(value: false);
	}
}
