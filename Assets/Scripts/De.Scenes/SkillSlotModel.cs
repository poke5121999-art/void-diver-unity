using UniRx;

public class SkillSlotModel : BaseSlotModel<SkillSlotModel>
{
	public int SlotIndex;

	public ESkillSlotType SlotType;

	public bool ShowLockIcon;

	public readonly ReactiveProperty<int> RxSkillId = new ReactiveProperty<int>();

	public int SkillId => RxSkillId.Value;
}
