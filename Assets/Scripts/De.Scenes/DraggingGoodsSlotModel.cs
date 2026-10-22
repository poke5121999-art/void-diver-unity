using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class DraggingGoodsSlotModel : BaseSlotModel<DraggingGoodsSlotModel>
{
	public readonly ReactiveProperty<GoodsSlot> RxGoodsSlot;

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

	public DraggingGoodsSlotModel(Goods goods)
	{
	}
}
