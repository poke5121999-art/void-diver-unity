using GameCommon.Data;
using NemoLib.Mvp;

public class GoodsQuantityPopupModel : Model
{
	public string Message;

	public Goods Goods;

	public long Count;

	public long CountMax;

	public long GoldPrice;

	public bool ShowGoldPrice;
}
