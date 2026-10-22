using NemoLib.Mvp;

public class EmployeeLevelUpPopupModel : Model
{
	public int EmployeeId;

	public int Stress;

	public int PrevLevel;

	public int NewLevel;

	public long Exp;

	public int[] LearnedSkillIds;

	public int[] NewlyLearnedSkillIds;
}
