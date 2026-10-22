using GameCommon.Data;

public class AchievementEvent : IBaseEvent
{
	public EAchievementTag Tag { get; }

	public EAchievementCat AchievementCat { get; }

	public string UAchvId { get; }

	public string AchievementName { get; }

	public string AchievementId { get; }

	public int? AchievementLv { get; }

	public EAchievementRslt AchievementRslt { get; }

	public string AchievementRsltDtl { get; }

	public AchievementEvent(EAchievementTag tag, EAchievementCat achievementCat, string uAchvId, string achievementName, string achievementId, EAchievementRslt achievementRslt, int? achievementLv = null, string achievementRsltDtl = null)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
