using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class GoodsSlotModel : BaseSlotModel<GoodsSlotModel>
{
	public readonly ReactiveProperty<GoodsSlot> RxGoodsSlot;

	public bool ShowItemCount;

	public bool EnableButtonAnimator;

	public bool UseTooltip;

	public bool IsCrafting;

	public Subject<Goods> OnCraftingNavigate;

	public bool ShowUnknownGoods;

	public bool IsSlotEmpty => false;

	public GoodsSlot GoodsSlot
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public EGoodsType GoodsType => EGoodsType.None;

	public int GoodsId => 0;

	public GoodsSlotModel(Goods goods)
	{
	}
}
