using GameCommon.Data;
using GameCommon.Table;
using UniRx;

public class CraftingMaterialPanelModel : BasePanelModel
{
	public int CraftingId;

	public long CraftingCount;

	public bool ShowConfirm;

	public Subject<Goods> OnCraftingNavigate;

	public readonly Subject<Unit> OnConfirm;

	private TCrafting CraftingData => null;

	public Goods[] Materials => null;

	public long NeedGold => 0L;

	public long NeedCoin => 0L;

	public long NeedRealCoin => 0L;
}
