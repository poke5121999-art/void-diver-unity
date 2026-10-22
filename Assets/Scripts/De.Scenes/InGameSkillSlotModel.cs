public class InGameSkillSlotModel : BaseSlotModel<InGameSkillSlotModel>
{
	public UnitModel UnitModel { get; set; }

	public int SlotIndex { get; set; }

	public int SkillId { get; set; }

	public float RemainCoolTime
	{
		get
		{
			if (SkillId < 1)
			{
				return 0f;
			}
			return UnitModel.GetRemainCoolTime(SkillId);
		}
	}

	public float ChargeAmount
	{
		get
		{
			if (SkillId < 1)
			{
				return 0f;
			}
			return UnitModel.GetSkillChargeAmount(SkillId);
		}
	}
}
