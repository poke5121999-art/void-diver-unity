using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using GameCommon.Utility;
using SomeNet.Utility;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameCampaignManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	public readonly Subject<(int id, bool isFirstClear)> OnCampaignCompleted = new Subject<(int id, bool isFirstClear)>();

	public readonly Subject<Unit> OnCampaignUpdated = new Subject<Unit>();

	public readonly Subject<int> OnTaskAchieved = new Subject<int>();

	private readonly ReactiveProperty<ECampaignState> _rxState = new ReactiveProperty<ECampaignState>();

	private readonly ReactiveProperty<int> _rxCampaignStepId = new ReactiveProperty<int>(-1);

	private readonly Dictionary<ulong, ECampaignState> _clientStates = new Dictionary<ulong, ECampaignState>();

	public readonly ReactiveCollection<ulong> RxCampaignAgreeingClientIds = new ReactiveCollection<ulong>();

	private int _votingCampaignId;

	private EDifficulty _votingDifficulty;

	private EAnomalyType _votingAnomalyType;

	public readonly Subject<Unit> OnGiveUpCampaignVoteStarted = new Subject<Unit>();

	public readonly Subject<bool> OnHostWaitOtherPlayerCampaignCompleted = new Subject<bool>();

	public ReactiveCollection<ulong> RxCampaignGiveUpAgreeingClientIds = new ReactiveCollection<ulong>();

	private int[] _playableNormalCampaignIds;

	private readonly Dictionary<int, EAnomalyType> _campaignAnomalyTypes = new Dictionary<int, EAnomalyType>();

	public int CampaignId => _gameContext.RxCampaignId.Value;

	public Dictionary<int, CampaignTask> CampaignTasks { get; private set; } = new Dictionary<int, CampaignTask>();

	public Subject<(int, EDifficulty, EAnomalyType)> OnCampaignVoteStarted { get; } = new Subject<(int, EDifficulty, EAnomalyType)>();

	public Subject<Unit> OnCampaignVoteCancelled { get; } = new Subject<Unit>();

	public Subject<(int, EDifficulty)> OnCampaignStarted { get; } = new Subject<(int, EDifficulty)>();

	public Subject<Unit> OnCampaignClosed { get; } = new Subject<Unit>();

	public IReadOnlyDictionary<ulong, ECampaignState> ClientStates => _clientStates;

	public IReadOnlyReactiveProperty<ECampaignState> RxState => _rxState;

	public ECampaignState State => _rxState.Value;

	public IReadOnlyReactiveProperty<int> RxCampaignStepId => _rxCampaignStepId;

	public int StepId => _rxCampaignStepId.Value;

	public int CampaignAgreeingCount => RxCampaignAgreeingClientIds.Count;

	public bool IsVotingCampaign => RxCampaignAgreeingClientIds.Count > 0;

	private int CampaignGiveUpAgreeingCount => RxCampaignGiveUpAgreeingClientIds.Count;

	public bool IsVotingCampaignGiveUp => RxCampaignGiveUpAgreeingClientIds.Count > 0;

	public long CurrentDialogReplayId { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameChannel.Receive<NtfVoteCampaign>().Subscribe(OnVoteCampaign).AddTo(this);
		_gameChannel.Receive<NtfStartCampaign>().Subscribe(OnStartCampaign).AddTo(this);
		_gameChannel.Receive<NtfCloseCampaign>().Subscribe(OnCloseCampaign).AddTo(this);
		_gameChannel.Receive<NtfUpdateCampaignState>().Subscribe(OnUpdateCampaignState).AddTo(this);
		_gameChannel.Receive<NtfVoteGiveUpCampaign>().Subscribe(OnVoteGiveUpCampaign).AddTo(this);
		_eventSystem.OnEventOccured.Subscribe(OnGameEventOccured).AddTo(this);
		_gameManager.OnPlayerLeft.Subscribe(OnPlayerLeft).AddTo(this);
		_gameChannel.OnStopped.Subscribe(delegate
		{
			OnRoomClosed();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Ended).Subscribe(delegate
		{
			OnStageFinished();
		}).AddTo(this);
		_gameManager.OnGameStarted.Subscribe(OnGameStarted).AddTo(this);
	}

	private void OnGameStarted(Unit _)
	{
		if (_gameManager.IsHost)
		{
			RefreshPlayableNormalCampaignIds();
			RefreshCampaignAnomalies();
		}
	}

	private void OnVoteCampaign(NtfVoteCampaign message)
	{
		if (!message.IsAgreed)
		{
			_votingCampaignId = 0;
			_votingDifficulty = EDifficulty.Easy;
			_votingAnomalyType = EAnomalyType.None;
			RxCampaignAgreeingClientIds.Clear();
			return;
		}
		if (!message.IsVoteStart)
		{
			if (RxCampaignAgreeingClientIds.Count < 1 || RxCampaignAgreeingClientIds.Contains(message.ClientId))
			{
				return;
			}
			RxCampaignAgreeingClientIds.Add(message.ClientId);
		}
		else
		{
			RxCampaignAgreeingClientIds.Clear();
			_votingCampaignId = message.CampaignId;
			_votingDifficulty = message.Difficulty;
			_votingAnomalyType = message.AnomalyType;
			RxCampaignAgreeingClientIds.Add(message.ClientId);
			OnCampaignVoteStarted.OnNext((message.CampaignId, message.Difficulty, message.AnomalyType));
		}
		if (_gameManager.PlayerCount == RxCampaignAgreeingClientIds.Count)
		{
			if (_gameManager.IsHost)
			{
				int votingCampaignId = _votingCampaignId;
				EDifficulty votingDifficulty = _votingDifficulty;
				_gameManager.StartCampaign(votingCampaignId, votingDifficulty, GetCampaignAnomalyType(votingCampaignId));
			}
			ClearVote();
		}
	}

	private void OnStartCampaign(NtfStartCampaign message)
	{
		_gameContext.RxCampaignId.Value = message.CampaignId;
		_gameContext.RxDifficulty.Value = message.Difficulty;
		_gameContext.RxAnomalyType.Value = message.AnomalyType;
		CampaignTasks = TCampaignTask.GroupTable[message.CampaignId].Select((TCampaignTask _) => new CampaignTask
		{
			Id = _.Id,
			Goal = _.Goal
		}).ToDictionary((CampaignTask _) => _.Id, (CampaignTask _) => _);
		_clientStates.Clear();
		foreach (ulong key in _gameContext.Players.Keys)
		{
			_clientStates[key] = ECampaignState.None;
		}
		SetCampaignState(ECampaignState.NotStarted);
		_rxCampaignStepId.Value = 0;
		OnCampaignUpdated.OnNext(Unit.Default);
		OnCampaignStarted.OnNext((message.CampaignId, message.Difficulty));
		_eventSystem.Trigger(new CampaignStartEvent(_gameContext.CampaignId, _gameContext.Difficulty, _gameContext.Players.Count));
		TriggerCampaignStart();
	}

	private void OnCloseCampaign(NtfCloseCampaign message)
	{
		TriggerCampaignEnd(message.Reason);
		_eventSystem.Trigger(new CampaignEndEvent(_gameContext.CampaignId, _gameContext.Difficulty, _gameContext.Players.Count, message.Reason));
		Clear();
	}

	private void OnUpdateCampaignState(NtfUpdateCampaignState message)
	{
		_clientStates[message.ClientId] = message.State;
		if (_gameManager.IsHost)
		{
			CheckOtherPlayerCampaignCompleted();
		}
		CheckAllPlayerCampaignEnd();
	}

	private void CheckOtherPlayerCampaignCompleted()
	{
		if (_gameManager.IsHost && GetPlayerCampaignState(_gameManager.LocalClientId) == ECampaignState.Completed)
		{
			bool value = _clientStates.Where((KeyValuePair<ulong, ECampaignState> _) => _.Key != _gameManager.LocalClientId).Any((KeyValuePair<ulong, ECampaignState> _) => _.Value == ECampaignState.NotCompleted);
			OnHostWaitOtherPlayerCampaignCompleted.OnNext(value);
		}
	}

	private void CheckAllPlayerCampaignEnd()
	{
		if (!_gameManager.IsHost)
		{
			return;
		}
		if (_clientStates.Values.All((ECampaignState _) => _ == ECampaignState.Completed))
		{
			CloseCampaign(ECampaignCloseReason.Clear);
		}
		if (_clientStates.Values.All((ECampaignState _) => _ == ECampaignState.Failed))
		{
			CloseCampaign(ECampaignCloseReason.ClearFail);
		}

		void CloseCampaign(ECampaignCloseReason reason)
		{
			RefreshPlayableNormalCampaignIds();
			RefreshCampaignAnomalies();
			UniTask.DelayFrame(1).ContinueWith(delegate
			{
				if (_gameManager.IsHost)
				{
					_gameManager.CloseCampaign(reason);
				}
			}).Forget();
		}
	}

	private void RefreshPlayableNormalCampaignIds()
	{
		int campaignMaxCount = TConst.CampaignMaxCount.IntValue;
		int recommendedLevelCampaignCount = TConst.RecommendedLevelCampaignCount.IntValue;
		int levelDifferenceWeight = TConst.CampaignLevelDifferenceWeight.IntValue;
		int levelMaxOffset = TConst.CampaignLevelMaxOffset.IntValue;
		List<int> list = new List<int>();
		list.AddRange(PickRecommendedLevelCampaigns(recommendedLevelCampaignCount));
		list.AddRange(PickOtherLevelCampaigns(campaignMaxCount - list.Count));
		_playableNormalCampaignIds = (from _ in list
			orderby TCampaign.Table[_].VariantDifficulty
			select _ into _
			orderby _
			select _).ToArray();

		int[] PickRecommendedLevelCampaigns(int count)
		{
			List<(int, int)> candidates = (from _ in TCampaign.Table.Values
				where _.Category == ECampaignCategory.Normal
				where _.RecommendedLevel == _playerManager.Level
				where _playerManager.CheckStateConditions(_.UnlockConditions)
				select (_.Id, _.Weight)).ToList();
			return PickIds(candidates, count);
		}

		int[] PickOtherLevelCampaigns(int count)
		{
			List<(int, int)> candidates = (from _ in TCampaign.Table.Values
				where _.Category == ECampaignCategory.Normal
				where _.RecommendedLevel != _playerManager.Level
				where _.RecommendedLevel <= _playerManager.Level + levelMaxOffset
				where _playerManager.CheckStateConditions(_.UnlockConditions)
				select (_.Id, _.Weight - Mathf.Abs(_.RecommendedLevel - _playerManager.Level) * levelDifferenceWeight) into _
				where 0 < _.Item2
				select _).ToList();
			return PickIds(candidates, count);
		}

		static int[] PickIds(IList<(int, int)> candidates, int count)
		{
			return (from x in candidates.Select(((int, int) x) => new
				{
					id = x.Item1,
					key = Math.Pow(RandomUtility.GetRandomFloat(), 1f / (float)x.Item2)
				})
				orderby x.key descending
				select x).Take(count).Select(x => x.id).ToArray();
		}
	}

	private void RefreshCampaignAnomalies()
	{
		_campaignAnomalyTypes.Clear();
		foreach (TCampaign value in TCampaign.Table.Values)
		{
			_campaignAnomalyTypes[value.Id] = (value.IsFixedAnomaly ? value.AnomalyType : PickRandomAnomaly());
		}

		static EAnomalyType PickRandomAnomaly()
		{
			return WeightedRandomSelector.SelectWeightedRandom(TAnomaly.Table.Values.Where((TAnomaly _) => 0 < _.Weight).Select((TAnomaly _) => (_.Type, _.Weight)), EAnomalyType.None);
		}
	}

	public bool OverrideAnomalyType(int campaignId, EAnomalyType anomalyType)
	{
		if (_gameContext.CampaignId > 0)
		{
			return false;
		}
		_campaignAnomalyTypes[campaignId] = anomalyType;
		return true;
	}

	public EAnomalyType GetCampaignAnomalyType(int campaignId)
	{
		if (_campaignAnomalyTypes.TryGetValue(campaignId, out var value))
		{
			return value;
		}
		TCampaign tCampaign = TCampaign.Table[campaignId];
		if (!tCampaign.IsFixedAnomaly)
		{
			return EAnomalyType.None;
		}
		return tCampaign.AnomalyType;
	}

	public int[] GetPlayableCampaignIds(ECampaignCategory category)
	{
		if (category == ECampaignCategory.Normal)
		{
			return _playableNormalCampaignIds;
		}
		return (from _ in TCampaign.Table.Values
			where _.Category == category
			where _playerManager.CheckStateConditions(_.UnlockConditions)
			orderby _.Id
			select _.Id).ToArray();
	}

	public bool HasNewStoryCampaign()
	{
		return TCampaign.Table.Values.Where((TCampaign _) => _.Category == ECampaignCategory.Story).Where((TCampaign _) => _playerManager.CheckStateConditions(_.UnlockConditions)).Any((TCampaign _) => !_playerManager.IsClearedCampaign(_.Id));
	}

	public ECampaignState GetPlayerCampaignState(ulong clientId)
	{
		return ((IReadOnlyDictionary<ulong, ECampaignState>)_clientStates).GetValueOrDefault(clientId);
	}

	private void OnPlayerLeft(GamePlayer player)
	{
		if (RxCampaignAgreeingClientIds.Count > 0)
		{
			OnCampaignVoteCancelled.OnNext(Unit.Default);
			ClearVote();
		}
		if (RxCampaignGiveUpAgreeingClientIds.Count > 0)
		{
			RxCampaignGiveUpAgreeingClientIds.Clear();
		}
		if (_clientStates.Remove(player.ClientId) && _gameContext.CampaignId >= 1 && _gameManager.IsHost)
		{
			if (player.ClientId != _gameManager.LocalClientId)
			{
				CheckAllPlayerCampaignEnd();
			}
			else
			{
				_gameManager.CloseCampaign(ECampaignCloseReason.Left);
			}
		}
	}

	private void OnRoomClosed()
	{
		if (_gameContext.CampaignId > 0)
		{
			TriggerCampaignEnd(ECampaignCloseReason.None);
		}
		Clear();
	}

	private void Clear()
	{
		SetCampaignState(ECampaignState.None);
		_gameContext.RxCampaignId.Value = 0;
		_rxCampaignStepId.Value = -1;
		CurrentDialogReplayId = 0L;
		_clientStates.Clear();
		CampaignTasks.Clear();
		ClearVote();
		RxCampaignGiveUpAgreeingClientIds.Clear();
		OnHostWaitOtherPlayerCampaignCompleted.OnNext(value: false);
		OnCampaignClosed.OnNext(Unit.Default);
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
		if (State == ECampaignState.InProgress)
		{
			bool flag = false;
			List<int> list = new List<int>();
			foreach (CampaignTask value in CampaignTasks.Values)
			{
				TCampaignTask tCampaignTask = TCampaignTask.Table[value.Id];
				long num = triggerEvent.CalculateProgress(tCampaignTask.EventCondition, value.TemporaryProgress, tCampaignTask.Goal - value.PermanentProgress);
				if (value.TemporaryProgress != num)
				{
					flag = true;
					value.TemporaryProgress = num;
					if (value.Progress >= value.Goal)
					{
						list.Add(value.Id);
					}
				}
			}
			if (list.Any())
			{
				list.ForEach(delegate(int _)
				{
					OnTaskAchieved.OnNext(_);
				});
			}
			if (flag)
			{
				OnCampaignUpdated.OnNext(Unit.Default);
			}
		}
		if (triggerEvent is LevelEvent && _gameManager.IsHost)
		{
			RefreshPlayableNormalCampaignIds();
		}
	}

	public void SetCampaignState(ECampaignState questState)
	{
		ECampaignState value = _rxState.Value;
		_rxState.Value = questState;
		_gameChannel.SendToHost(new ReqUpdateCampaignState
		{
			State = questState
		});
		_logger.Debug(string.Format("{0} -> {1}", value, questState));
	}

	public void SetQuestStepId(int stepId)
	{
		_rxCampaignStepId.Value = stepId;
	}

	public bool IsAllTaskAchieved()
	{
		if (!CampaignTasks.Any())
		{
			return false;
		}
		return CampaignTasks.Values.All((CampaignTask _) => _.Goal <= _.Progress);
	}

	public bool IsTaskAchieved(int campaignTaskId)
	{
		if (!CampaignTasks.TryGetValue(campaignTaskId, out var value))
		{
			return false;
		}
		return value.Goal <= value.Progress;
	}

	public void CompleteQuest()
	{
		if (_gameContext.CampaignId < 1)
		{
			_logger.Warning("CompleteQuest() called but CampaignId is 0. This should not happen.");
			return;
		}
		_gameManager.SetMissionClear(value: true);
		SetCampaignState(ECampaignState.Completed);
		int campaignId = _gameContext.CampaignId;
		bool flag = !_playerManager.IsClearedCampaign(campaignId);
		TCampaign campaign = TCampaign.Table[_gameContext.CampaignId];
		GoodsData[] rewards = campaign.GetRewards(flag);
		if (rewards != null)
		{
			Goods[] goods = rewards.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();
			_playerManager.AddGoodsToStorage(goods);
		}
		_playerManager.SetClearedCampaign(_gameContext.CampaignId);
		_eventSystem.Trigger(new CampaignClearEvent(_gameContext.CampaignId, flag));
		OnCampaignCompleted.OnNext((_gameContext.CampaignId, flag));
	}

	public void FailQuest()
	{
		SetCampaignState(ECampaignState.Failed);
		OnCampaignUpdated.OnNext(Unit.Default);
	}

	public bool HasVoted(ulong clientId)
	{
		return RxCampaignAgreeingClientIds.Contains(clientId);
	}

	private void ClearVote()
	{
		_votingCampaignId = 0;
		_votingDifficulty = EDifficulty.Easy;
		_votingAnomalyType = EAnomalyType.None;
		RxCampaignAgreeingClientIds.Clear();
	}

	private void OnStageFinished()
	{
		foreach (CampaignTask value in CampaignTasks.Values)
		{
			value.PermanentProgress = value.Progress;
			value.TemporaryProgress = 0L;
		}
	}

	private void ClearVoteGiveUp()
	{
		RxCampaignGiveUpAgreeingClientIds.Clear();
	}

	private void OnVoteGiveUpCampaign(NtfVoteGiveUpCampaign message)
	{
		if (!message.IsAgreed)
		{
			RxCampaignGiveUpAgreeingClientIds.Clear();
		}
		else
		{
			if (RxCampaignGiveUpAgreeingClientIds.Contains(message.ClientId) || _gameContext.CampaignId < 1)
			{
				return;
			}
			RxCampaignGiveUpAgreeingClientIds.Add(message.ClientId);
			if (RxCampaignGiveUpAgreeingClientIds.Count == 1)
			{
				OnGiveUpCampaignVoteStarted.OnNext(Unit.Default);
			}
			if (_gameManager.PlayerCount == RxCampaignGiveUpAgreeingClientIds.Count)
			{
				if (_gameManager.IsHost)
				{
					_gameManager.CloseCampaign(ECampaignCloseReason.GiveUp);
				}
				ClearVoteGiveUp();
			}
		}
	}

	public bool HasVotedGiveUp(ulong clientId)
	{
		return RxCampaignGiveUpAgreeingClientIds.Contains(clientId);
	}

	public void SetCurrentDialogReplayId(long dialogReplayId)
	{
		CurrentDialogReplayId = dialogReplayId;
	}

	private void TriggerCampaignStart()
	{
		int round = GameManager.DifficultyToRound(_gameContext.Difficulty);
		int characterId = ((_gameManager.MyCharacterModel == null) ? _playerManager.SelectedCharacterId : _gameManager.MyCharacterModel.CharacterId);
		_eventSystem.Trigger(new PlayCampaignStartEvent(_gameContext.CampaignId, round, characterId, _gameContext.Players.Count, GetCampaignAnomalyId(), BuildCampaignUseEquipment()));
	}

	private void TriggerCampaignEnd(ECampaignCloseReason reason)
	{
		int round = GameManager.DifficultyToRound(_gameContext.Difficulty);
		int characterId = ((_gameManager.MyCharacterModel == null) ? _playerManager.SelectedCharacterId : _gameManager.MyCharacterModel.CharacterId);
		_eventSystem.Trigger(new PlayCampaignEndEvent(_gameContext.CampaignId, round, characterId, reason, _gameContext.Players.Count, GetCampaignAnomalyId()));
		_gameManager.SetMissionClear(value: false);
	}

	private int? GetCampaignAnomalyId()
	{
		EAnomalyType anomalyType = _gameContext.AnomalyType;
		if (anomalyType == EAnomalyType.None)
		{
			return null;
		}
		return (int)anomalyType;
	}

	private string BuildCampaignUseEquipment()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		if (myCharacterModel.WeaponId > 0)
		{
			list.Add(myCharacterModel.WeaponId);
		}
		list.AddRange(myCharacterModel.AccessoryIds);
		if (list.Count < 1)
		{
			return null;
		}
		return string.Join(",", list);
	}
}
