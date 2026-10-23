using UniRx;

public class OptionKeyPanelModel : BasePanelModel
{
	public readonly Subject<Unit> OnBack = new Subject<Unit>();
}
