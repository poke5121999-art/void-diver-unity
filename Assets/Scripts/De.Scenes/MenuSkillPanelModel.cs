using UniRx;

public class MenuSkillPanelModel : BasePanelModel
{
	public ulong ClientId;

	public readonly Subject<SkillSlotModel> OnSlotEnter = new Subject<SkillSlotModel>();

	public readonly Subject<SkillSlotModel> OnSlotExit = new Subject<SkillSlotModel>();
}
