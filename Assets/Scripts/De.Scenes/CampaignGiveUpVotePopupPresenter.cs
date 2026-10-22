using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CampaignGiveUpVotePopupPresenter : GamePopupPresenter
{
	[View]
	private CampaignGiveUpVotePopupView _view;

	[InjectModel]
	private CampaignGiveUpVotePopupModel _model;

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

	private readonly List<VoteStateModel> _voteStateModels = new List<VoteStateModel>();

	private bool _isPendingClose;

	private float _voteTimeLeft;

	private bool _isVoteTimerRunning;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private ulong ClientId => _gameManager.LocalClientId;

	private bool HasVotedGiveUp => _gameCampaignManager.RxCampaignGiveUpAgreeingClientIds.Contains(_gameManager.LocalClientId);

	private int VoteTimeLimit => TConst.VoteTimeLimit.IntValue;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		if (!_gameCampaignManager.IsVotingCampaignGiveUp)
		{
			Close();
			return;
		}
		InitVoteModels();
		InitCampaignInfo();
		if (_gameManager.IsHost)
		{
			_view.Agree.SetActive(value: false);
			_view.Disagree.SetActive(value: false);
			_view.Voting.gameObject.SetActive(value: true);
		}
		UpdateAgreeingUis();
		_gameCampaignManager.RxCampaignGiveUpAgreeingClientIds.ObserveCountChanged().Subscribe(delegate
		{
			UpdateAgreeingUis();
		}).AddTo(this);
		_gameCampaignManager.OnCampaignClosed.Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_gameChannel.Receive<NtfVoteGiveUpCampaign>().Subscribe(OnVoteGiveUpCampaign).AddTo(this);
		_view.Agree_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnYesButtonClick();
		}).AddTo(this);
		_view.Disagree_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnNoButtonClick();
		}).AddTo(this);
		InitGamePadNav();
		StartVoteTimer();

		void InitCampaignInfo()
		{
			TCampaign tCampaign = TCampaign.Table[_gameContext.CampaignId];
			EDifficulty difficulty = _gameContext.Difficulty;
			_view.Campaign_DifficultyBg.color = difficulty.ToBackgroundColor(_view.Campaign_DifficultyBg.color.a);
			_view.Campaign_Category.text = tCampaign.GetChapterString();
			_view.Campaign_Name.text = tCampaign.Name;
			_view.Campaign_Difficulty_Value.text = difficulty.ToColoredLocalText();
		}
		void InitVoteModels()
		{
			_view.VoteStates.ForEach(delegate(GameObject voteState)
			{
				VoteStateModel voteStateModel = new VoteStateModel();
				_voteStateModels.Add(voteStateModel);
				voteState.GetComponent<VoteStatePresenter>().Inject(voteStateModel);
			});
		}
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
		if (!_isVoteTimerRunning)
		{
			return;
		}
		_voteTimeLeft -= Time.unscaledDeltaTime;
		_view.Timer.fillAmount = Mathf.Clamp01(_voteTimeLeft / (float)VoteTimeLimit);
		if (_voteTimeLeft <= 0f)
		{
			_isVoteTimerRunning = false;
			if (!_gameCampaignManager.HasVotedGiveUp(_gameManager.LocalClientId))
			{
				_gameChannel.SendToHost(new ReqVoteGiveUpCampaign
				{
					IsAgreed = false
				});
			}
		}
	}

	private void StopVoteTimer()
	{
		_isVoteTimerRunning = false;
	}

	private void InitGamePadNav()
	{
		if (!_gameManager.IsHost)
		{
			List<Selectable> items = new List<Selectable> { _view.Agree_Button, _view.Disagree_Button };
			SetNavHorizontal(items);
			SetDefaultNav(_view.Agree_Button);
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnNoButtonClick();
		}).AddTo(InputDisposable);
	}

	private void OnYesButtonClick()
	{
		if (!_gameCampaignManager.HasVotedGiveUp(_gameManager.LocalClientId))
		{
			_gameChannel.SendToHost(new ReqVoteGiveUpCampaign
			{
				IsAgreed = true
			});
		}
	}

	private void OnNoButtonClick()
	{
		if (!_gameCampaignManager.HasVotedGiveUp(_gameManager.LocalClientId))
		{
			_gameChannel.SendToHost(new ReqVoteGiveUpCampaign
			{
				IsAgreed = false
			});
		}
	}

	private void UpdateAgreeingUis()
	{
		int count = _gameCampaignManager.RxCampaignGiveUpAgreeingClientIds.Count;
		if (count == 0)
		{
			Close();
			return;
		}
		for (int i = 0; i < _voteStateModels.Count; i++)
		{
			if (i < _gameManager.PlayerCount)
			{
				_voteStateModels[i].RxELayout.Value = ((i < count) ? EVoteStateLayout.Yes : EVoteStateLayout.Waiting);
			}
			else
			{
				_view.VoteStates[i].SetActive(value: false);
			}
		}
	}

	private void OnVoteGiveUpCampaign(NtfVoteGiveUpCampaign message)
	{
		if (_gameManager.LocalClientId == message.ClientId)
		{
			_view.Agree.SetActive(value: false);
			_view.Disagree.SetActive(value: false);
			_view.Voting.gameObject.SetActive(value: true);
		}
		if (message.IsAgreed && !_voteStateModels.Take(_gameManager.PlayerCount).All((VoteStateModel model) => model.RxELayout.Value == EVoteStateLayout.Yes))
		{
			return;
		}
		Close();
	}

	public override void Close(object result = null)
	{
		if (_isPendingClose)
		{
			return;
		}
		_isPendingClose = true;
		_isVoteTimerRunning = false;
		bool isDisagreed = _voteStateModels.Any((VoteStateModel model) => model.RxELayout.Value == EVoteStateLayout.No);
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
