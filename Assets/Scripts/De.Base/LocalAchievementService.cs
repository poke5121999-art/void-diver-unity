using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using VContainer;

public class LocalAchievementService : MonoBase, IAchievementService
{
	[Inject]
	private SeriLogger _logger;

	private const string KeyPrefix = "Achievement_";

	private const string ProgressKeyPrefix = "AchievementProgress_";

	private readonly Dictionary<int, bool> _cache;

	public bool IsAvailable => false;

	public bool IsInitialized { get; private set; }

	protected override void Awake()
	{
	}

	public UniTask<bool> InitializeAsync()
	{
		return default;
	}

	public void SetAchievement(int achievementId)
	{
	}

	public void SetProgress(int achievementId, int current, int max)
	{
	}

	public int GetProgress(int achievementId)
	{
		return 0;
	}

	public bool IsAchieved(int achievementId)
	{
		return false;
	}

	public void StoreStats()
	{
	}

	public void ClearAchievement(int achievementId)
	{
	}
}
