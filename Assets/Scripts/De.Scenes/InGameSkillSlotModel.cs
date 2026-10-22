public class InGameSkillSlotModel : BaseSlotModel<InGameSkillSlotModel>
{
	public UnitModel UnitModel { get; set; }

	public int SlotIndex { get; set; }

	public int SkillId { get; set; }

	public float RemainCoolTime => 0f;

	public float ChargeAmount => 0f;
}
