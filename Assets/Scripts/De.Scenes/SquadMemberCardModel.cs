using UniRx;

public class SquadMemberCardModel : BasePanelModel
{
	public int Index;

	public ulong ClientId;

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotEnter;

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotExit;

	public readonly Subject<SkillSlotModel> OnSkillSlotEnter;

	public readonly Subject<SkillSlotModel> OnSkillSlotExit;

	public bool IsEmpty;

	public SquadMemberCardPresenter.EFocusPanel FocusPanel;
}
