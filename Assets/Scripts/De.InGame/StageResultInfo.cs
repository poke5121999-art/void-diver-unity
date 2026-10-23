using System;
using System.Collections.Generic;

public class StageResultInfo
{
	public StageResultPlayerInfo[] PlayerInfos = Array.Empty<StageResultPlayerInfo>();

	public int CampaignId;

	public Dictionary<int, (long progress, long goal)> CampaignSubTasks = new Dictionary<int, (long progress, long goal)>();

	public StageResultLocalInfo[] LocalInfos = Array.Empty<StageResultLocalInfo>();
}
