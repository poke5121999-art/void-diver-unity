using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class GoodsSlotModel : BaseSlotModel<GoodsSlotModel>
{
	public readonly ReactiveProperty<GoodsSlot> RxGoodsSlot = new ReactiveProperty<GoodsSlot>();

	public bool ShowItemCount = true;

	public bool EnableButtonAnimator = true;

	public bool UseTooltip;

	public bool IsCrafting;

	public Subject<Goods> OnCraftingNavigate;

	public bool ShowUnknownGoods = true;

	public bool IsSlotEmpty
	{
		get
		{
			GoodsSlot value = RxGoodsSlot.Value;
			return value == null || value.Goods == null;
		}
	}

	public GoodsSlot GoodsSlot
	{
		get
		{
			return RxGoodsSlot.Value;
		}
		set
		{
			RxGoodsSlot.SetValueAndForceNotify(value);
		}
	}

	public EGoodsType GoodsType
	{
		get
		{
			GoodsSlot value = RxGoodsSlot.Value;
			if (value == null || value.Goods == null)
			{
				return EGoodsType.None;
			}
			return value.Goods.Type;
		}
	}

	public int GoodsId
	{
		get
		{
			GoodsSlot value = RxGoodsSlot.Value;
			if (value == null || value.Goods == null)
			{
				return 0;
			}
			return value.Goods.Id;
		}
	}

	public GoodsSlotModel(Goods goods)
	{
		RxGoodsSlot.Value = new GoodsSlot
		{
			Goods = goods
		};
	}
}
