using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class DraggingGoodsSlotModel : BaseSlotModel<DraggingGoodsSlotModel>
{
	public readonly ReactiveProperty<GoodsSlot> RxGoodsSlot = new ReactiveProperty<GoodsSlot>();

	public GoodsSlot GoodsSlot
	{
		get
		{
			return RxGoodsSlot.Value;
		}
		set
		{
			RxGoodsSlot.Value = value;
		}
	}

	public EGoodsType GoodsType => RxGoodsSlot.Value?.Goods?.Type ?? EGoodsType.None;

	public int GoodsId => RxGoodsSlot.Value?.Goods?.Id ?? 0;

	public DraggingGoodsSlotModel(Goods goods)
	{
		RxGoodsSlot.Value = new GoodsSlot
		{
			Goods = goods
		};
	}
}
