using GameCommon.Data;

public class PlaySessionStartEvent : IBaseEvent
{
	public int Round { get; }

	public int CharacterId { get; }

	public int PlayerCount { get; }

	public PlaySessionStartEvent(int round, int characterId, int playerCount = 1)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
