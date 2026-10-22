using System;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;

public class CampaignTask
{
	private const string COLOR_DONE = "#eeeeee";

	private const string COLOR_NOT_DONE = "#b5b5b5";

	public int Id { get; set; }

	public long PermanentProgress { get; set; }

	public long TemporaryProgress { get; set; }

	public long Goal { get; set; }

	public long Progress => TemporaryProgress + PermanentProgress;

	public bool IsCompleted => TemporaryProgress + PermanentProgress >= Goal;

	public bool IsPermanentlyCompleted => PermanentProgress >= Goal;

	public string ToLocalString()
	{
		TCampaignTask tCampaignTask = TCampaignTask.Table[Id];
		long progress = TemporaryProgress + PermanentProgress;
		if (tCampaignTask.EventCondition.Type == EEventConditionType.LuaProgress)
		{
			return tCampaignTask.Desc.WithColor((progress >= Goal) ? COLOR_DONE : COLOR_NOT_DONE);
		}
		if (progress >= Goal)
		{
			return tCampaignTask.EventCondition.ToDescription() + ">" + string.Format(" {0}/{1}", progress, Goal).WithColor(COLOR_DONE);
		}
		string text = (tCampaignTask.EventCondition.ToDescription() ?? "").WithColor(COLOR_NOT_DONE);
		return text + string.Format(" {0}", progress).WithColor(COLOR_DONE) + string.Format("/{0}", Goal).WithColor(COLOR_NOT_DONE);
	}
}
