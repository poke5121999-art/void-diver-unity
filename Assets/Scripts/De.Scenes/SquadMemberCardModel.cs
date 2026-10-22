using UniRx;

public class SquadMemberCardModel : BasePanelModel
{
	public int Index;

	public ulong ClientId;

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotEnter = new Subject<ParadoxSimpleSlotModel>();

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotExit = new Subject<ParadoxSimpleSlotModel>();

	public readonly Subject<SkillSlotModel> OnSkillSlotEnter = new Subject<SkillSlotModel>();

	public readonly Subject<SkillSlotModel> OnSkillSlotExit = new Subject<SkillSlotModel>();

	public bool IsEmpty;

	public SquadMemberCardPresenter.EFocusPanel FocusPanel;
}
