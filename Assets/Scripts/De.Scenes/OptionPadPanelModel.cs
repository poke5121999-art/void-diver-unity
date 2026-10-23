using UniRx;

public class OptionPadPanelModel : BasePanelModel
{
	public readonly Subject<Unit> OnBack = new Subject<Unit>();
}
