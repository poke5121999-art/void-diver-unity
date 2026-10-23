using GameCommon.Table;

public static class TalentExtensions
{
	public static bool HasTalent(this Talent talent, int talentId)
	{
		return talent.BitFlags.IsSet(TTalent.Table[talentId].BitFlagIndex);
	}
}
