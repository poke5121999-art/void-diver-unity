using UniRx;

public class ParadoxPanelModel : BasePanelModel
{
	public GamePlayer Player;

	public bool IsPositive;

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotEnter = new Subject<ParadoxSimpleSlotModel>();

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotExit = new Subject<ParadoxSimpleSlotModel>();
}
