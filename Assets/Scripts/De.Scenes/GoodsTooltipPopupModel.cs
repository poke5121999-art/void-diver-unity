using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;

public class GoodsTooltipPopupModel : Model
{
	public Goods Goods;

	public bool IsCrafting;

	public Subject<Goods> OnCraftingNavigate;
}
