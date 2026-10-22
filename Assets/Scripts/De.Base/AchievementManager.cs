using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using VContainer;

public class AchievementManager : NativeBase, IDisposable
{
	public class AchievementProgress
	{
		public int Id { get; set; }

		public long Progress { get; set; }

		public long Goal { get; set; }
	}

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private IAchievementService _achievementService;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private AccountRecordManager _accountRecordManager;

	private List<AchievementProgress> _progresses;

	private bool _isInitialized;

	private CompositeDisposable _disposable;

	public async UniTask<bool> InitializeAsync()
	{
		if (_isInitialized)
		{
			return true;
		}
		if (!(await _achievementService.InitializeAsync()))
		{
			return false;
		}
		_progresses = TAchievement.Table.Values.Where((TAchievement a) => !_achievementService.IsAchieved(a.Id)).Select((TAchievement a) => new AchievementProgress
		{
			Id = a.Id,
			Progress = ((a.EventCondition.Type == GameCommon.Enum.EEventConditionType.AccountRecord) ? Math.Min(_accountRecordManager.GetCount((int)a.EventCondition.Value1), a.Goal) : _achievementService.GetProgress(a.Id)),
			Goal = a.Goal
		}).ToList();
		_logger.Debug(string.Format("[AchievementManager] 초기화 완료 — 미달성 업적 {0}개", _progresses.Count));
		foreach (AchievementProgress progress in _progresses)
		{
			_logger.Debug(string.Format("[AchievementManager] Init | id:{0} progress:{1}/{2}", progress.Id, progress.Progress, progress.Goal));
		}
		_progresses.Where((AchievementProgress p) => p.Goal <= p.Progress).ToList().ForEach(delegate(AchievementProgress p)
		{
			_logger.Debug(string.Format("[AchievementManager] 초기화 시 즉시 달성 | id:{0} progress:{1}/{2}", p.Id, p.Progress, p.Goal));
			_achievementService.SetProgress(p.Id, (int)p.Progress, (int)p.Goal);
			_progresses.Remove(p);
		});
		foreach (AchievementProgress progress2 in _progresses)
		{
			if (TAchievement.Table[progress2.Id].EventCondition.Type == GameCommon.Enum.EEventConditionType.AccountRecord && progress2.Progress > 0)
			{
				_achievementService.SetProgress(progress2.Id, (int)progress2.Progress, (int)progress2.Goal);
			}
		}
		_disposable = new CompositeDisposable();
		_eventSystem.OnEventOccured.Subscribe(OnGameEventOccured).AddTo(_disposable);
		_isInitialized = true;
		return true;
	}

	void IDisposable.Dispose()
	{
		_disposable?.Dispose();
	}

	private long GetProgress(TAchievement achievement)
	{
		if (achievement.EventCondition.Type == GameCommon.Enum.EEventConditionType.AccountRecord)
		{
			return Math.Min(_accountRecordManager.GetCount((int)achievement.EventCondition.Value1), achievement.Goal);
		}
		return _achievementService.GetProgress(achievement.Id);
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
		if (_progresses == null || _progresses.Count == 0)
		{
			return;
		}
		List<AchievementProgress> completed = new List<AchievementProgress>();
		foreach (AchievementProgress progress in _progresses)
		{
			TAchievement tAchievement = TAchievement.Table[progress.Id];
			long progress2 = progress.Progress;
			long goal = tAchievement.Goal;
			long num = triggerEvent.CalculateProgress(tAchievement.EventCondition, progress2, goal);
			if (progress2 != num)
			{
				progress.Progress = num;
				_logger.Debug(string.Format("[AchievementManager] SetProgress | id:{0} {1} → {2}/{3}", tAchievement.Id, progress2, num, tAchievement.Goal));
				_achievementService.SetProgress(tAchievement.Id, (int)num, (int)tAchievement.Goal);
			}
			if (num >= tAchievement.Goal)
			{
				_logger.Debug(string.Format("[AchievementManager] 달성 | id:{0} ({1})", tAchievement.Id, tAchievement.Name));
				completed.Add(progress);
			}
		}
		completed.ForEach(delegate(AchievementProgress p)
		{
			_progresses.Remove(p);
		});
	}
}
