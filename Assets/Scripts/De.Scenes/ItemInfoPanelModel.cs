using GameCommon.Data;
using UniRx;

public class ItemInfoPanelModel : BasePanelModel
{
	public Goods Goods;

	public readonly Subject<Unit> OnBack;
}
