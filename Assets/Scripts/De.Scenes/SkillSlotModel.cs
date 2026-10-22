using UniRx;

public class SkillSlotModel : BaseSlotModel<SkillSlotModel>
{
	public int SlotIndex;

	public ESkillSlotType SlotType;

	public bool ShowLockIcon;

	public readonly ReactiveProperty<int> RxSkillId;

	public int SkillId => 0;
}
