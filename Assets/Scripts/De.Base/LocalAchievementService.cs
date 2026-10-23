using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using VContainer;

public class LocalAchievementService : MonoBase, IAchievementService
{
	[Inject]
	private SeriLogger _logger;

	private const string KeyPrefix = "Achievement_";

	private const string ProgressKeyPrefix = "AchievementProgress_";

	private readonly Dictionary<int, bool> _cache = new Dictionary<int, bool>();

	public bool IsAvailable => true;

	public bool IsInitialized { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	public UniTask<bool> InitializeAsync()
	{
		IsInitialized = true;
		return UniTask.FromResult(true);
	}

	public void SetAchievement(int achievementId)
	{
		if (!IsAchieved(achievementId))
		{
			_cache[achievementId] = true;
			PlayerPrefs.SetInt("Achievement_" + achievementId, 1);
			PlayerPrefs.Save();
			_logger.Debug(string.Format("[LocalAchievementService] 업적 해금: {0}", achievementId));
		}
	}

	public void SetProgress(int achievementId, int current, int max)
	{
		_logger.Debug(string.Format("[LocalAchievementService] 진행도 갱신: {0} ({1}/{2})", achievementId, current, max));
		PlayerPrefs.SetInt("AchievementProgress_" + achievementId, current);
		if (current >= max)
		{
			SetAchievement(achievementId);
		}
	}

	public int GetProgress(int achievementId)
	{
		return PlayerPrefs.GetInt("AchievementProgress_" + achievementId, 0);
	}

	public bool IsAchieved(int achievementId)
	{
		if (_cache.TryGetValue(achievementId, out var value))
		{
			return value;
		}
		bool flag = PlayerPrefs.GetInt("Achievement_" + achievementId, 0) == 1;
		_cache[achievementId] = flag;
		return flag;
	}

	public void StoreStats()
	{
	}

	public void ClearAchievement(int achievementId)
	{
		_cache[achievementId] = false;
		PlayerPrefs.DeleteKey("Achievement_" + achievementId);
		PlayerPrefs.DeleteKey("AchievementProgress_" + achievementId);
		PlayerPrefs.Save();
		_logger.Debug(string.Format("[LocalAchievementService] 업적 초기화: {0}", achievementId));
	}
}
