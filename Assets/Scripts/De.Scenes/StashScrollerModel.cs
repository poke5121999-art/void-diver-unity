using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class StashScrollerModel : BaseScrollerModel<InventoryGoodsSlotModel>
{
	public ReactiveProperty<EGoodsType[]> RxAllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public readonly ReactiveProperty<int> RxPageCount;

	public readonly ReactiveProperty<int> RxPageIndex;

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag;

	public readonly ReactiveProperty<bool> RxIsEmpty;

	public readonly Subject<int> OnMovePage;

	public int PageCount => 0;

	public int PageIndex => 0;

	public EGoodsType[] AllowedSubTypes => null;

	public Vector3 SelectedSlotPosition => default;
}
