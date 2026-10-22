using UniRx;

public class SamplePanelModel : BasePanelModel
{
	public readonly Subject<Unit> OnBack = new Subject<Unit>();
}
