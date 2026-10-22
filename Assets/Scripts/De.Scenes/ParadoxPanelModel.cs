using UniRx;

public class ParadoxPanelModel : BasePanelModel
{
	public GamePlayer Player;

	public bool IsPositive;

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotEnter;

	public readonly Subject<ParadoxSimpleSlotModel> OnParadoxSlotExit;
}
