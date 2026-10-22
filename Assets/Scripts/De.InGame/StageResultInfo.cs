using System.Collections.Generic;

public class StageResultInfo
{
	public StageResultPlayerInfo[] PlayerInfos;

	public int CampaignId;

	public Dictionary<int, (long progress, long goal)> CampaignSubTasks;

	public StageResultLocalInfo[] LocalInfos;
}
