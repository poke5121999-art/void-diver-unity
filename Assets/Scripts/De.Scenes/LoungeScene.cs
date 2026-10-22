using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class LoungeScene : InGameScene
{
	[View]
	private LoungeSceneView _view;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	private bool _isTutorialStarting;

	private bool IsHost => NetworkManager.IsHost;

	protected override void Start()
	{
		base.Start();
		CampaignManager.OnCampaignStarted.Subscribe(OnCampaignStarted).AddTo(this);
		CampaignManager.OnCampaignCompleted.Subscribe(delegate((int id, bool isFirstClear) _)
		{
			OnCampaignCompleted(_.id, _.isFirstClear);
		}).AddTo(this);
		CampaignManager.RxState.Where((ECampaignState _) => _ == ECampaignState.Failed).Subscribe(delegate
		{
			OnQuestFailed();
		}).AddTo(this);
		GameManager.RxStageState.Where((EStageState _) => _ == EStageState.LoadCompleted).Subscribe(delegate
		{
			OnStageLoadCompleted();
		}).AddTo(this);
		GameManager.RxStageState.Where((EStageState _) => _ == EStageState.Playing).Subscribe(delegate
		{
			OnPlaying().Forget();
		}).AddTo(this);
		TryStartTutorialCampaignAsync().Forget();
		RoomMatchmaker.OnCreateLobby.Subscribe(delegate
		{
			TryShowHostLoungeCode().Forget();
		}).AddTo(this);
		GameHost.OnClientCaughtUp.Subscribe(OnClientCaughtUp).AddTo(this);
	}

	private async UniTask TryStartTutorialCampaignAsync()
	{
		if (GameContext.IsTutorial && NetworkManager.IsHost && RoomContext.RoomState != ERoomState.Playing)
		{
			_isTutorialStarting = true;
			await UniTask.WaitUntil(() => RoomContext.RoomState == ERoomState.Ready || GameHost.IsAllPlayersReady);
			EAnomalyType campaignAnomalyType = CampaignManager.GetCampaignAnomalyType(1100);
			GameManager.StartCampaign(1100, EDifficulty.Normal, campaignAnomalyType);
		}
	}

	private async UniTask TryShowHostLoungeCode()
	{
		if (NetworkManager.IsHost && Settings.Platform == EPlatform.Steam && !RoomContext.IsSingleMode && !GameContext.IsTutorial && GameContext.CampaignId <= 0)
		{
			await UniTask.WaitUntil(() => RoomContext.RoomState == ERoomState.Ready);
			Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
			{
				PopupManager.OpenAsync<RoomCodeDisplayPopupPresenter>().Forget();
			}).AddTo(this);
		}
	}

	private void OnStageLoadCompleted()
	{
		if (GameManager.StageResultInfo != null)
		{
			GameManager.StageResultInfo.CampaignSubTasks = CampaignManager.CampaignTasks.Values.ToDictionary((CampaignTask task) => task.Id, (CampaignTask task) => (task.Progress, task.Goal));
		}
	}

	private async UniTask OpenStageResultPopupAsync()
	{
		await UniTask.WaitUntil(() => !PopupManager.ExistPopup());
		if (!(this == null) && !GameManager.IsInStage && GameManager.StageResultInfo != null)
		{
			StageResultPopupPresenter stageResultPopupPresenter = await PopupManager.OpenAsync<StageResultPopupPresenter>(new StageResultPopupModel
			{
				StageResultInfo = GameManager.StageResultInfo
			});
			await stageResultPopupPresenter.Task;
			UiEventSystem.FlushLevelAndExp();
			GameManager.ClearStageResult();
		}
	}

	private async UniTaskVoid OnPlaying()
	{
		if (GameManager.IsUnexpectedBackToSinglePlay)
		{
			PopupManager.ShowTextPopup(TLocalizedText.NetworkErrorBackToLounge.LocalText);
			GameManager.ClearUnexpectedBackToSinglePlay();
		}
		if (GameManager.CachedConnectionResultCode != 0)
		{
			PopupManager.ShowConnectionFailPopup(GameManager.CachedConnectionResultCode);
			GameManager.CachedConnectionResultCode = 0;
		}
		if (!GameManager.IsTutorial)
		{
			await OpenStageResultPopupAsync();
		}
		if (GameContext.CampaignId > 0)
		{
			_tycoonManager.RefreshRecruitableEmployees();
			_tycoonManager.OnTurn();
			_artifactDealManager.ClearInitialInfos();
			_artifactDealManager.ClearDoneStates();
		}
		_view.LoungeLoadingPanel.DOFade(0f, 0.25f);
		await UniTask.Delay(300);
		_view.LoungeLoadingPanel.gameObject.SetActive(value: false);
	}

	private void OnCampaignStarted((int campaignId, EDifficulty difficulty) campaignData)
	{
		if (!GameContext.IsTutorial)
		{
			OpenCampaignStartPopupAsync().Forget();
		}
		async UniTask OpenCampaignStartPopupAsync()
		{
			CampaignVotePopupPresenter campaignVotePopupPresenter = PopupManager.FindPopup<CampaignVotePopupPresenter>();
			if (campaignVotePopupPresenter != null)
			{
				await campaignVotePopupPresenter.Task;
			}
			PopupManager.Open<CampaignStartPopupPresenter>(new CampaignStartPopupModel
			{
				CampaignId = campaignData.campaignId,
				Difficulty = campaignData.difficulty
			});
		}
	}

	private void OnCampaignCompleted(int campaignId, bool isFirstClear)
	{
		PopupManager.OpenAsync<CampaignCompletePopupPresenter>(new CampaignCompletePopupModel
		{
			CampaignId = campaignId,
			IsFirstClear = isFirstClear
		}).ContinueWith(delegate
		{
			UiEventSystem.FlushLevelAndExp();
		}).Forget();
	}

	private void OnQuestFailed()
	{
		PopupManager.Open<CampaignFailPopupPresenter>();
	}

	private void OnClientCaughtUp(ulong clientId)
	{
		ToastManager.Notice(TLocalizedText.NewPlayerJoinCancel.LocalText);
	}
}
