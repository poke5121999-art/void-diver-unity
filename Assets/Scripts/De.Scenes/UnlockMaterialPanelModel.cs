using GameCommon.Data;

public class UnlockMaterialPanelModel : BasePanelModel
{
	public Goods[] Materials;

	public long NeedGold;

	public long NeedCoin;

	public long NeedRealCoin;

	public EventCondition[] UnlockConditions;
}
