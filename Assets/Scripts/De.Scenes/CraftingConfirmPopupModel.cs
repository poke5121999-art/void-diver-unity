using GameCommon.Data;
using NemoLib.Mvp;

public class CraftingConfirmPopupModel : Model
{
	public int CraftingId;

	public Goods CraftingGoods => null;

	public Goods[] Materials => null;

	public long NeedGold => 0L;

	public long NeedCoin => 0L;

	public long NeedRealCoin => 0L;
}
