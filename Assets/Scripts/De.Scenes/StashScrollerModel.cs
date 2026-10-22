using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class StashScrollerModel : BaseScrollerModel<InventoryGoodsSlotModel>
{
	public ReactiveProperty<EGoodsType[]> RxAllowedSubTypes;

	public EArtifactCategory[] ArtifactFilterTypes;

	public readonly ReactiveProperty<int> RxPageCount = new ReactiveProperty<int>();

	public readonly ReactiveProperty<int> RxPageIndex = new ReactiveProperty<int>();

	public readonly ReactiveProperty<InventoryGoodsSlotModel> RxFocusedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnSlotEndDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly ReactiveProperty<bool> RxIsEmpty = new ReactiveProperty<bool>();

	public readonly Subject<int> OnMovePage = new Subject<int>();

	public int PageCount => RxPageCount.Value;

	public int PageIndex => RxPageIndex.Value;

	public EGoodsType[] AllowedSubTypes => RxAllowedSubTypes.Value;

	public Vector3 SelectedSlotPosition => RxFocusedSlotModel.Value?.Position ?? Vector3.zero;
}
