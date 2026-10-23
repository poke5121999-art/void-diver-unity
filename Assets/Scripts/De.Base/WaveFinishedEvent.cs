using GameCommon.Enum;
using System;
using GameCommon.Data;

public class WaveFinishedEvent : IBaseEvent
{
	public int WaveId { get; private set; }

	public ulong NetworkObjectId { get; private set; }

	public bool IsCleared { get; private set; }

	public WaveFinishedEvent(int waveId, ulong networkObjectId, bool isCleared)
	{
		WaveId = waveId;
		NetworkObjectId = networkObjectId;
		IsCleared = isCleared;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.WaveClear && IsCleared && WaveId == eventCondition.Value1)
		{
			currentProgress = Math.Min(currentProgress + 1, goal);
		}
		return currentProgress;
	}
}
