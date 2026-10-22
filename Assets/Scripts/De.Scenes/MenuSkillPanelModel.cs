using UniRx;

public class MenuSkillPanelModel : BasePanelModel
{
	public ulong ClientId;

	public readonly Subject<SkillSlotModel> OnSlotEnter;

	public readonly Subject<SkillSlotModel> OnSlotExit;
}
