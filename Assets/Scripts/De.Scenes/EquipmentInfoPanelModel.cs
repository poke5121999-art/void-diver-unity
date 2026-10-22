using GameCommon.Data;
using UniRx;

public class EquipmentInfoPanelModel : BasePanelModel
{
	public Goods Goods;

	public readonly Subject<Unit> OnBack;
}
