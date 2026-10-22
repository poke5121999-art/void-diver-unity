using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;

public class TycoonResultPopupModel : Model
{
	public bool IsPanic;

	public bool IsSold;

	public int EmployeeId;

	public int Stress;

	public int Level;

	public long EmployeeExp;

	public int EmployeeExpGain;

	public Goods Artifact;

	public long ResultCoin;

	public long ResultExp;

	public ETycoonFailType FailType;

	public int SlotIndex;

	public int[] StressSkillIds;

	public int[] PriceSkillIds;

	public int SkillPriceBonusPercent;

	public long SkillCoinBonus;
}
