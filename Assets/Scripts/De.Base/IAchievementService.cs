using Cysharp.Threading.Tasks;

public interface IAchievementService
{
	bool IsAvailable { get; }

	bool IsInitialized { get; }

	UniTask<bool> InitializeAsync();

	void SetAchievement(int achievementId);

	bool IsAchieved(int achievementId);

	void SetProgress(int achievementId, int current, int max);

	int GetProgress(int achievementId);

	void ClearAchievement(int achievementId);

	void StoreStats();
}
