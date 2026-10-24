using System.Collections.Generic;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CampaignVotePopupPresenter : GamePopupPresenter
{
	[View]
	private CampaignVotePopupView _view;

	[InjectModel]
	private CampaignVotePopupModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _gameCampaignManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	private readonly List<CampaignVoteStateModel> _voteStates = new List<CampaignVoteStateModel>();

	private bool _isClosing;

	private float _voteTimeLeft;

	private bool _isVoteTimerRunning;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private ulong ClientId => _gameManager.LocalClientId;

	private bool HasVoted => _gameCampaignManager.HasVoted(_gameManager.LocalClientId);

	private int VoteTimeLimit => TConst.VoteTimeLimit.IntValue;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		if (!_gameCampaignManager.IsVotingCampaign)
		{
			Close();
			return;
		}
		InitUis();
		_view.Agree_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnYesButtonClick();
		}).AddTo(this);
		_view.Disagree_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnNoButtonClick();
		}).AddTo(this);
		_gameChannel.Receive<NtfVoteCampaign>().Subscribe(OnVoteCampaign).AddTo(this);
		_gameCampaignManager.OnCampaignVoteCancelled.Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		InitGamePadNav();
		StartVoteTimer();
	}

	protected override void Update()
	{
		base.Update();
		UpdateVoteTimer();
	}

	private void StartVoteTimer()
	{
		_voteTimeLeft = VoteTimeLimit;
		_view.Timer.fillAmount = 1f;
		_isVoteTimerRunning = true;
	}

	private void UpdateVoteTimer()
	{
		if (_isVoteTimerRunning)
		{
			_voteTimeLeft -= Time.unscaledDeltaTime;
			_view.Timer.fillAmount = Mathf.Clamp01(_voteTimeLeft / (float)VoteTimeLimit);
			if (_voteTimeLeft <= 0f)
			{
				_isVoteTimerRunning = false;
				if (!HasVoted)
				{
					_gameChannel.SendToHost(new ReqVoteCampaign
					{
						IsAgreed = false
					});
				}
			}
		}
	}

	private void StopVoteTimer()
	{
		_isVoteTimerRunning = false;
	}

	private void InitGamePadNav()
	{
		List<Button> list = _view.Reward_Slots.Where((GameObject _) => _.activeSelf).Select((GameObject _) => _.GetComponent<GoodsSlotView>().Slot).ToList();
		SetNavHorizontal(list);
		if (HasVoted)
		{
			if (list.Count >= 1)
			{
				SetDefaultNav(list[0]);
			}
			return;
		}
		if (list.Count > 0)
		{
			foreach (Button item in list)
			{
				ConnectNav(item, ENavDir.Down, _view.Agree_Button);
			}
			ConnectNav(_view.Agree_Button, ENavDir.Up, list[0]);
			ConnectNav(_view.Disagree_Button, ENavDir.Up, list[list.Count - 1]);
		}
		ConnectNav(_view.Agree_Button, ENavDir.Right, _view.Disagree_Button);
		ConnectNav(_view.Disagree_Button, ENavDir.Left, _view.Agree_Button);
		SetDefaultNav(_view.Agree_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnNoButtonClick();
		}).AddTo(InputDisposable);
	}

	private void InitUis()
	{
		TCampaign campaignData = TCampaign.Table[_model.CampaignId];
		bool isStory = campaignData.Category == ECampaignCategory.Story;
		bool isCleared = _playerManager.IsClearedCampaign(_model.CampaignId);
		InitInfo();
		InitRewardGoods();
		InitVoteStates();

		void InitInfo()
		{
			_view.Difficulty_Bg.color = _model.Difficulty.ToBackgroundColor();
			_view.Difficulty_Text.text = _model.Difficulty.ToColoredLocalText();
			_view.Difficulty_TextHard.SetActive(_model.Difficulty == EDifficulty.Hard);
			_view.Difficulty_TextInsane.SetActive(_model.Difficulty == EDifficulty.Insane);
			_view.NewTag.SetActive(isStory && !isCleared);
			_view.Completed.SetActive(isCleared && isStory);
			_view.TitleLayout_Chapter.text = campaignData.GetChapterString();
			_view.TitleLayout_Name.text = campaignData.Name;
			_view.TitleLayout_Boss.SetActive(campaignData.Category == ECampaignCategory.Normal && campaignData.IsBossCampaign);
			_view.TextRegion.text = ((System.Enum)campaignData.ThemeType).ToLocalText();
			_view.Anomaly_IconAnomaly.sprite = _assetManager.GetIconAnomaly(_model.AnomalyType);
			_view.Anomaly_Desc.text = ((System.Enum)_model.AnomalyType).ToLocalText();
			_view.RecommandedLevel_Value.text = string.Format("Lv. {0}", campaignData.RecommendedLevel);
		}

		void InitRewardGoods()
		{
			bool flag = _playerManager.IsClearedCampaign(campaignData.Id);
			GoodsData[] rewards = campaignData.GetRewards(!flag);
			for (int i = 0; i < _view.Reward_Slots.Length; i++)
			{
				GameObject gameObject = _view.Reward_Slots[i];
				if (rewards == null || i >= rewards.Length)
				{
					gameObject.SetActive(value: false);
					continue;
				}
				Goods goods = rewards[i].ToGoodsAsBase();
				GoodsSlotModel goodsSlotModel = new GoodsSlotModel(goods);
				goodsSlotModel.ShowItemCount = true;
				goodsSlotModel.OnClick.Subscribe(delegate
				{
					_popupManager.Open<GoodsTooltipPopupPresenter>(new GoodsTooltipPopupModel
					{
						Goods = goods
					});
				}).AddTo(this);
				gameObject.GetComponent<GoodsSlotPresenter>().Inject(goodsSlotModel);
				gameObject.SetActive(value: true);
			}
		}

		void InitVoteStates()
		{
			GamePlayer[] array = _gameContext.Players.Values.OrderByDescending((GamePlayer _) => _.IsHostPlayer).ToArray();
			for (int i = 0; i < _view.VoteStates.Length; i++)
			{
				GameObject gameObject = _view.VoteStates[i];
				if (i < array.Length)
				{
					ulong clientId = array[i].ClientId;
					bool flag = _gameCampaignManager.RxCampaignAgreeingClientIds.Contains(clientId);
					CampaignVoteStateModel campaignVoteStateModel = new CampaignVoteStateModel
					{
						ClientId = clientId,
						RxELayout = new ReactiveProperty<EVoteStateLayout>(flag ? EVoteStateLayout.Yes : EVoteStateLayout.Waiting)
					};
					_voteStates.Add(campaignVoteStateModel);
					gameObject.GetComponent<CampaignVoteStatePresenter>().Inject(campaignVoteStateModel);
				}
				else
				{
					gameObject.SetActive(value: false);
				}
			}
			_view.Agree.SetActive(!HasVoted);
			_view.Disagree.SetActive(!HasVoted);
			_view.Voting.SetActive(HasVoted);
		}
	}

	private void OnYesButtonClick()
	{
		if (!HasVoted)
		{
			_gameChannel.SendToHost(new ReqVoteCampaign
			{
				IsAgreed = true
			});
		}
	}

	private void OnNoButtonClick()
	{
		if (!HasVoted)
		{
			_gameChannel.SendToHost(new ReqVoteCampaign
			{
				IsAgreed = false
			});
		}
	}

	private void OnVoteCampaign(NtfVoteCampaign message)
	{
		CampaignVoteStateModel campaignVoteStateModel = _voteStates.FirstOrDefault((CampaignVoteStateModel _) => _.ClientId == message.ClientId);
		if (campaignVoteStateModel != null)
		{
			campaignVoteStateModel.RxELayout.Value = (message.IsAgreed ? EVoteStateLayout.Yes : EVoteStateLayout.No);
		}
		if (_gameManager.LocalClientId == message.ClientId)
		{
			_view.Agree.SetActive(value: false);
			_view.Disagree.SetActive(value: false);
			_view.Voting.SetActive(value: true);
		}
		if (message.IsAgreed && !_voteStates.All((CampaignVoteStateModel _) => _.RxELayout.Value == EVoteStateLayout.Yes))
		{
			return;
		}
		Close();
	}

	public override void Close(object result = null)
	{
		if (_isClosing)
		{
			return;
		}
		_isClosing = true;
		_isVoteTimerRunning = false;
		bool isDisagreed = _voteStates.Any((CampaignVoteStateModel _) => _.RxELayout.Value == EVoteStateLayout.No);
		Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
		{
			if (isDisagreed)
			{
				_toastManager.NoticeFail(TLocalizedText.VoteCancelledByOpposition.LocalText);
			}
			base.Close(result);
		}).AddTo(this);
	}
}
