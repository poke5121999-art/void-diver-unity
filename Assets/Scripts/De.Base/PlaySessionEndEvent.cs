using GameCommon.Data;

public class PlaySessionEndEvent : IBaseEvent
{
	public int Round { get; }

	public int CharacterId { get; }

	public int PlayerCount { get; }

	public string UnlockTalent { get; }

	public bool WasInterrupted { get; }

	public PlaySessionEndEvent(int round, int characterId, int playerCount = 1, string unlockTalent = null, bool wasInterrupted = false)
	{
		Round = round;
		CharacterId = characterId;
		PlayerCount = playerCount;
		UnlockTalent = unlockTalent;
		WasInterrupted = wasInterrupted;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}
