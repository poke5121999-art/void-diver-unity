using GameCommon.Data;
using GameCommon.Enum;

public class TycoonFinishResult
{
	public bool IsSold;

	public bool IsPanic;

	public int EmployeeId;

	public int Stress;

	public Goods Artifact;

	public long ResultCoin;

	public long ResultExp;

	public ETycoonFailType FailType;

	public int PrevLevel;

	public int NewLevel;

	public int[] LearnedSkillIds;

	public int[] NewlyLearnedSkillIds;

	public int EmployeeLevel;

	public long EmployeeExp;

	public int EmployeeExpGain;

	public int[] StressSkillIds;

	public int[] PriceSkillIds;

	public int SkillPriceBonusPercent;

	public long SkillCoinBonus;

	public bool IsLevelUp => PrevLevel < NewLevel;
}
