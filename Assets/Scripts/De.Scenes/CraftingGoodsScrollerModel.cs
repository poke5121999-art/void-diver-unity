using GameCommon.Enum;
using UniRx;

public class CraftingGoodsScrollerModel : BaseScrollerModel<CraftingGoodsSlotModel>
{
	public int CraftingGroupId;

	public EGoodsType FilterType;

	public readonly ReactiveProperty<bool> RxCraftableOnly;

	public int FocusedIndex;

	public float ScrollPosition;
}
