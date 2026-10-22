using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using SomeNet.Collections;
using UniRx;
using UnityEngine;
using VContainer;

public class GameMissionManager : MonoBase
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

	public readonly Dictionary<int, Mission> Missions = new Dictionary<int, Mission>();

	public readonly Subject<(int missionId, int Step)> OnMissionStepChanged = new Subject<(int missionId, int Step)>();

	public readonly Subject<int> OnMissionAchieved = new Subject<int>();

	public readonly Subject<(int missionId, EMissionState state)> OnMissionStateChanged = new Subject<(int missionId, EMissionState state)>();

	public readonly Subject<Unit> OnMissionStarted = new Subject<Unit>();

	public readonly Subject<int> OnMissionUpdated = new Subject<int>();

	public IEnumerable<int> MissionIds => Missions.Keys;

	public bool HasMission => Missions.Any();

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.SceneLoad).Subscribe(delegate
		{
			OnStageSceneLoad();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Closed).Subscribe(delegate
		{
			OnStageClosed();
		}).AddTo(this);
		_gameManager.OnGameClosed.Subscribe(OnGameClosed).AddTo(this);
		_eventSystem.OnEventOccured.Subscribe(OnGameEventOccured).AddTo(this);
		_gameChannel.Receive<NtfStartMission>().Subscribe(OnStartMission).AddTo(this);
		_gameChannel.Receive<NtfEndMission>().Subscribe(OnEndMission).AddTo(this);
	}

	private void OnStageSceneLoad()
	{
		if (_gameManager.IsHost && _gameContext.IsInStage)
		{
			if (TCampaign.Table[_gameContext.CampaignId].Category == ECampaignCategory.Normal)
			{
				NtfStartMission baseMessage = new NtfStartMission
				{
					MissionIds = PickRandomMissionIds()
				};
				_gameHost.SendToAll(baseMessage);
			}
		}
	}

	private void OnStageClosed()
	{
		if (_gameManager.IsHost && Missions.Any())
		{
			_gameHost.SendToAll(new NtfEndMission());
		}
	}

	private void OnStartMission(NtfStartMission message)
	{
		message.MissionIds.ForEach(delegate(int id)
		{
			Missions.Add(id, new Mission
			{
				Id = id,
				Goal = TMission.Table[id].Goal
			});
		});
		OnMissionStarted.OnNext(Unit.Default);
	}

	private void OnEndMission(NtfEndMission message)
	{
		Missions.Clear();
	}

	private void OnGameClosed(Unit _)
	{
		Missions.Clear();
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
		if (!Missions.Any())
		{
			return;
		}
		List<int> achieved = new List<int>();
		foreach (Mission value in Missions.Values)
		{
			if (value.State == EMissionState.InProgress)
			{
				TMission tMission = TMission.Table[value.Id];
				long num = triggerEvent.CalculateProgress(tMission.EventCondition, value.Progress, tMission.Goal);
				if (num != value.Progress)
				{
					value.Progress = num;
					if (value.Goal <= num)
					{
						achieved.Add(value.Id);
					}
					OnMissionUpdated.OnNext(value.Id);
				}
			}
		}
		if (achieved.Any())
		{
			achieved.ForEach(delegate(int id)
			{
				OnMissionAchieved.OnNext(id);
			});
		}
	}

	public int GetMissionStep(int missionId)
	{
		Mission value;
		if (!Missions.TryGetValue(missionId, out value))
		{
			return -1;
		}
		return value.Step;
	}

	public void SetMissionStep(int missionId, int stepId)
	{
		Mission value;
		if (Missions.TryGetValue(missionId, out value))
		{
			value.Step = stepId;
			OnMissionStepChanged.OnNext((missionId, stepId));
		}
	}

	public EMissionState GetMissionState(int missionId)
	{
		Mission value;
		if (!Missions.TryGetValue(missionId, out value))
		{
			return EMissionState.None;
		}
		return value.State;
	}

	public void SetMissionState(int missionId, EMissionState state)
	{
		Mission value;
		if (Missions.TryGetValue(missionId, out value))
		{
			value.State = state;
			OnMissionUpdated.OnNext(missionId);
			OnMissionStateChanged.OnNext((missionId, state));
		}
	}

	public void CompleteMission(int missionId)
	{
		GoodsData[] rewards = TMission.Table[missionId].Rewards;
		if (rewards != null)
		{
			ReqGainGoods baseMessage = new ReqGainGoods
			{
				Goods = rewards.Select((GoodsData g) => g.ToGoodsAsBase()).ToArray()
			};
			_gameChannel.SendToHost(baseMessage);
		}
		_playerManager.SetClearedMission(missionId);
		Mission value;
		if (Missions.TryGetValue(missionId, out value))
		{
			value.State = EMissionState.Completed;
			OnMissionUpdated.OnNext(missionId);
			OnMissionStateChanged.OnNext((missionId, EMissionState.Completed));
		}
		_eventSystem.OnEventOccured.OnNext(new MissionClearEvent(missionId));
	}

	private int[] PickRandomMissionIds()
	{
		int intValue = TConst.MissionCount.IntValue;
		EThemeType themeType = _gameContext.ThemeType;
		if (_gameManager.MissionIds == null || _gameManager.MissionIds.Length < 1)
		{
			return TMission.Table.Values.Where((TMission m) => m.ThemeTypes == null || m.ThemeTypes.Length == 0 || m.ThemeTypes.Any((EThemeType t) => t == themeType)).Where((TMission m) => m.UnlockConditions == null || m.UnlockConditions.Length == 0 || m.UnlockConditions.All(_playerManager.CheckStateCondition)).ToList().Shuffle()
				.Select((TMission m) => m.Id)
				.Take(intValue)
				.ToArray();
		}
		return _gameManager.MissionIds;
	}

	public void SetCurrentDialogReplayId(long dialogReplayId)
	{
		TDialogReplay tDialogReplay = TDialogReplay.Table[dialogReplayId];
		Mission value;
		if (Missions.TryGetValue(tDialogReplay.QuestTableId, out value))
		{
			value.CurrentDialogReplayId = dialogReplayId;
			OnMissionUpdated.OnNext(tDialogReplay.QuestTableId);
		}
	}
}
