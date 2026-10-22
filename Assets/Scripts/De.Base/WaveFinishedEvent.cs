using GameCommon.Data;

public class WaveFinishedEvent : IBaseEvent
{
	public int WaveId { get; private set; }

	public ulong NetworkObjectId { get; private set; }

	public bool IsCleared { get; private set; }

	public WaveFinishedEvent(int waveId, ulong networkObjectId, bool isCleared)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
