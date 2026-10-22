using GameCommon.Enum;
using UniRx;

public class EmployeeCardModel : BaseSlotModel<EmployeeCardModel>
{
	public int EmployeeId;

	public int Level;

	public long Exp;

	public int Stress;

	public bool IsPanic;

	public bool IsPlaced;

	public EEmployeeSlotType SlotType;

	public long HireCost;

	public int[] LearnedSkillIds;

	public bool IsLevelUp;

	public int PrevLevel;

	public int[] NewlyLearnedSkillIds;

	public readonly Subject<EmployeeCardModel> OnPointerEnter;

	public readonly Subject<EmployeeCardModel> OnCleansingClick;

	public readonly Subject<EmployeeCardModel> OnFireClick;
}
