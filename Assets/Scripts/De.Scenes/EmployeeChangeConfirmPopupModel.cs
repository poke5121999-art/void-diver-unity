using NemoLib.Mvp;

public class EmployeeChangeConfirmPopupModel : Model
{
	public int BeforeEmployeeId;

	public int BeforeLevel;

	public long BeforeExp;

	public int BeforeStress;

	public bool BeforeIsPanic;

	public int[] BeforeLearnedSkillIds;

	public int AfterEmployeeId;

	public int AfterLevel;

	public long AfterExp;

	public int AfterStress;

	public bool AfterIsPanic;

	public int[] AfterLearnedSkillIds;
}
