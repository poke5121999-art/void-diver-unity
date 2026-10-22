using System.Linq;
using GameCommon.Data;
using GameCommon.Table;
using UniRx;

public class CraftingMaterialPanelModel : BasePanelModel
{
	public int CraftingId;

	public long CraftingCount = 1L;

	public bool ShowConfirm;

	public Subject<Goods> OnCraftingNavigate;

	public readonly Subject<Unit> OnConfirm = new Subject<Unit>();

	private TCrafting CraftingData => TCrafting.Table[CraftingId];

	public Goods[] Materials => CraftingData.MaterialItemDatas.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();

	public long NeedGold => CraftingData.NeedGold;

	public long NeedCoin => CraftingData.NeedCoin;

	public long NeedRealCoin => CraftingData.NeedRealCoin;
}
