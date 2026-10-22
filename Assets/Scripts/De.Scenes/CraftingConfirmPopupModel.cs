using System.Linq;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;

public class CraftingConfirmPopupModel : Model
{
	public int CraftingId;

	public Goods CraftingGoods => TCrafting.Table[CraftingId].ResultGoodsData.ToGoodsAsBase();

	public Goods[] Materials => TCrafting.Table[CraftingId].MaterialItemDatas.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();

	public long NeedGold => TCrafting.Table[CraftingId].NeedGold;

	public long NeedCoin => TCrafting.Table[CraftingId].NeedCoin;

	public long NeedRealCoin => TCrafting.Table[CraftingId].NeedRealCoin;
}
