using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameLoungeQuestManager : MonoBase
{
	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private PlayerManager _playerManager;

	private bool _isPlayerInitialized;

	private IDictionary<int, LoungeQuest> LoungeQuests => _playerManager.LoungeQuests;

	public Subject<LoungeQuest> OnQuestAdded { get; } = new Subject<LoungeQuest>();

	public Subject<(LoungeQuest oldValue, LoungeQuest newValue)> OnQuestUpdated { get; } = new Subject<(LoungeQuest oldValue, LoungeQuest newValue)>();

	public Subject<LoungeQuest> OnQuestRemoved { get; } = new Subject<LoungeQuest>();

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_playerManager.OnPlayerInitialized.Subscribe(delegate
		{
			_isPlayerInitialized = true;
		}).AddTo(this);
		_eventSystem.OnEventOccured.Subscribe(OnGameEventOccured).AddTo(this);
	}

	public bool TryGetQuest(int questId, out LoungeQuest loungeQuest)
	{
		return _playerManager.LoungeQuests.TryGetValue(questId, out loungeQuest);
	}

	public void AddQuest(int questId)
	{
		if (_playerManager.LoungeQuests.ContainsKey(questId))
		{
			_logger.Error(string.Format("Lounge Quest already exists. QuestId: {0}", questId));
			return;
		}
		LoungeQuestTask[] tasks = TLoungeQuestTask.GroupTable[questId].Select((TLoungeQuestTask _) => new LoungeQuestTask
		{
			Id = _.Id,
			Progress = _playerManager.GetLoungeConditionProgress(_.EventCondition, 0L),
			Goal = _.Goal
		}).ToArray();
		LoungeQuest loungeQuest = new LoungeQuest
		{
			Id = questId,
			Tasks = tasks,
			State = ELoungeQuestState.NotStarted,
			DisplayInHud = TLoungeQuest.Table[questId].Display
		};
		_playerManager.LoungeQuests.Add(loungeQuest.Id, loungeQuest);
		OnQuestAdded.OnNext(loungeQuest);
	}

	public void RemoveQuest(int questId)
	{
		LoungeQuest value;
		if (!_playerManager.LoungeQuests.Remove(questId, out value))
		{
			_logger.Error(string.Format("Lounge Quest not exists. QuestId: {0}", questId));
		}
		else
		{
			OnQuestRemoved.OnNext(value);
		}
	}

	private void SetQuest(LoungeQuest loungeQuest)
	{
		if (loungeQuest == null)
		{
			return;
		}
		if (!TryGetQuest(loungeQuest.Id, out var loungeQuest2))
		{
			_logger.Error(string.Format("Lounge Quest not exists. QuestId: {0}", loungeQuest.Id));
			return;
		}
		_playerManager.LoungeQuests[loungeQuest.Id] = loungeQuest;
		OnQuestUpdated.OnNext((loungeQuest2, loungeQuest));
	}

	public void SetDisplayInHud(int questId, bool displayInHud)
	{
		if (TryGetQuest(questId, out var loungeQuest) && loungeQuest.DisplayInHud != displayInHud)
		{
			LoungeQuest loungeQuest2 = loungeQuest.Clone();
			loungeQuest2.DisplayInHud = displayInHud;
			SetQuest(loungeQuest2);
		}
	}

	public void SetQuestState(int questId, ELoungeQuestState state)
	{
		if (!TryGetQuest(questId, out var loungeQuest))
		{
			_logger.Error(string.Format("Lounge Quest not exists. QuestId: {0}, State: {1}", questId, state));
		}
		else if (loungeQuest.State != state)
		{
			LoungeQuest loungeQuest2 = loungeQuest.Clone();
			loungeQuest2.State = state;
			SetQuest(loungeQuest2);
		}
	}

	public void CompleteQuest(int questId)
	{
		_playerManager.SetClearedLoungeQuest(questId);
		if (TryGetQuest(questId, out var loungeQuest))
		{
			if (loungeQuest.State != ELoungeQuestState.Completed)
			{
				LoungeQuest loungeQuest2 = loungeQuest.Clone();
				loungeQuest2.State = ELoungeQuestState.Completed;
				SetQuest(loungeQuest2);
			}
		}
		else
		{
			_logger.Error(string.Format("Lounge Quest not exists. QuestId: {0}, State: {1}", questId, ELoungeQuestState.Completed));
		}
		GoodsData[] rewards = TLoungeQuest.Table[questId].Rewards;
		if (rewards != null)
		{
			Goods[] goods = rewards.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();
			_playerManager.AddGoodsToStorage(goods);
		}
		_eventSystem.Trigger(new LoungeQuestClearEvent(questId));
		RemoveQuest(questId);
	}

	public int[] GetActiveQuestIds()
	{
		return (from _ in _playerManager.LoungeQuests.Values
			where _.IsActive
			orderby _.Category
			select _.Id).ToArray();
	}

	public int[] GetDisplayLoungeQuestIds()
	{
		return (from _ in _playerManager.LoungeQuests.Values
			where _.DisplayInHud
			where _.IsActive
			orderby _.Category
			select _.Id).ToArray();
	}

	public bool HasAnyActiveQuest()
	{
		return _playerManager.LoungeQuests.Values.Any((LoungeQuest _) => _.IsActive);
	}

	public long GetQuestTaskProgress(int taskId)
	{
		TLoungeQuestTask tLoungeQuestTask = TLoungeQuestTask.Table[taskId];
		if (TryGetQuest(tLoungeQuestTask.LoungeQuestId, out var loungeQuest) && loungeQuest.Tasks != null)
		{
			LoungeQuestTask loungeQuestTask = loungeQuest.Tasks.FirstOrDefault((LoungeQuestTask _) => _.Id == taskId);
			if (loungeQuestTask != null)
			{
				return loungeQuestTask.Progress;
			}
		}
		return 0L;
	}

	public int GetLoungeQuestCompletedTaskCount(int questId)
	{
		if (TryGetQuest(questId, out var loungeQuest) && loungeQuest.Tasks != null)
		{
			return loungeQuest.Tasks.Count((LoungeQuestTask t) => t.IsCompleted);
		}
		return 0;
	}

	public int GetLoungeQuestTotalTaskCount(int questId)
	{
		if (TryGetQuest(questId, out var loungeQuest) && loungeQuest.Tasks != null)
		{
			return loungeQuest.Tasks.Length;
		}
		return 0;
	}

	public bool IsAllQuestTaskCompleted(int questId)
	{
		if (!TryGetQuest(questId, out var loungeQuest))
		{
			return false;
		}
		return loungeQuest.IsAllTaskCompleted;
	}

	public ELoungeQuestState GetQuestState(int questId)
	{
		if (!TryGetQuest(questId, out var loungeQuest))
		{
			return ELoungeQuestState.None;
		}
		return loungeQuest.State;
	}

	public void SetCurrentDialogReplayId(long dialogReplayId)
	{
		TDialogReplay tDialogReplay = TDialogReplay.Table[dialogReplayId];
		if (TryGetQuest(tDialogReplay.QuestTableId, out var loungeQuest))
		{
			LoungeQuest loungeQuest2 = loungeQuest.Clone();
			loungeQuest2.CurrentDialogReplayId = dialogReplayId;
			SetQuest(loungeQuest2);
		}
	}

	private void OnGameEventOccured(IBaseEvent gameEvent)
	{
		if (!_isPlayerInitialized)
		{
			return;
		}
		AddUnlockableQuests();
		UpdateQuestProgress();

		void AddUnlockableQuests()
		{
			foreach (TLoungeQuest value in TLoungeQuest.Table.Values)
			{
				if (!_playerManager.IsClearedLoungeQuest(value.Id) && !_playerManager.GetLoungeQuestIds().Contains(value.Id) && _playerManager.CheckStateConditions(value.UnlockConditions))
				{
					AddQuest(value.Id);
					if (_gameManager.IgnoreLoungeTutorial && value.Id == 80200)
					{
						CompleteQuest(value.Id);
					}
				}
			}
		}

		void UpdateQuestProgress()
		{
			if (_playerManager.GetLoungeQuestCount() == 0)
			{
				return;
			}
			foreach (LoungeQuest loungeQuest in _playerManager.GetLoungeQuests())
			{
				if (loungeQuest.Tasks == null || loungeQuest.Tasks.Length == 0 || !TLoungeQuestTask.GroupTable.TryGetValue(loungeQuest.Id, out var value2))
				{
					continue;
				}
				LoungeQuest loungeQuest2 = loungeQuest.Clone();
				bool flag = false;
				for (int i = 0; i < loungeQuest2.Tasks.Length; i++)
				{
					LoungeQuestTask task = loungeQuest2.Tasks[i];
					TLoungeQuestTask tLoungeQuestTask = value2.FirstOrDefault((TLoungeQuestTask td) => td.Id == task.Id);
					if (tLoungeQuestTask != null && (!task.IsCompleted || tLoungeQuestTask.EventCondition.Type.GetValueType() == EEventConditionValueType.Assign))
					{
						long num = gameEvent.CalculateProgress(tLoungeQuestTask.EventCondition, task.Progress, tLoungeQuestTask.Goal);
						if (task.Progress != num)
						{
							task.Progress = num;
							flag = true;
						}
					}
				}
				if (flag)
				{
					if (loungeQuest2.State == ELoungeQuestState.InProgress && loungeQuest2.IsAllTaskCompleted)
					{
						loungeQuest2.State = ELoungeQuestState.NotCompleted;
					}
					if (loungeQuest2.State == ELoungeQuestState.NotCompleted && !loungeQuest2.IsAllTaskCompleted)
					{
						loungeQuest2.State = ELoungeQuestState.InProgress;
					}
					SetQuest(loungeQuest2);
				}
			}
		}
	}
}
