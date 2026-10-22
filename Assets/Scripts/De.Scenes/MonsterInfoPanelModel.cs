using UniRx;

public class MonsterInfoPanelModel : BasePanelModel
{
	public int MonsterId;

	public readonly Subject<Unit> OnBack = new Subject<Unit>();
}
