using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class InventoryGoodsSlotModel : BaseSlotModel<InventoryGoodsSlotModel>
{
	public Transform Transform;

	public readonly EGoodsSlotCategory SlotCategory;

	public readonly ReactiveProperty<GoodsSlot> RxGoodsSlot;

	public readonly ReactiveProperty<InventoryGoodsSlotPresenter.ELayout> RxLayout;

	public readonly ReactiveProperty<bool> RxHighlightOn;

	public bool CheckMaxItemCount;

	public bool UseTooltip;

	public bool ShowItemCount;

	public bool IsUnknown;

	public bool IsCrafting;

	public Subject<Goods> OnCraftingNavigate;

	public readonly Subject<InventoryGoodsSlotModel> OnDrag;

	public readonly Subject<InventoryGoodsSlotModel> OnEndDrag;

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

	public Vector3 Position => default;

	public InventoryGoodsSlotPresenter.ELayout Layout => InventoryGoodsSlotPresenter.ELayout.Normal;

	public bool IsSlotEmpty => false;

	public bool IsRevealing => false;

	public EGoodsType GoodsType => EGoodsType.None;

	public int GoodsId => 0;

	public int SlotIndex => 0;

	public InventoryGoodsSlotModel(EGoodsSlotCategory category, GoodsSlot slot, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
	}

	public InventoryGoodsSlotModel(EGoodsSlotCategory category, Goods goods, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
	}

	public InventoryGoodsSlotModel(GoodsSlot slot, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
	}

	public InventoryGoodsSlotModel(Goods goods, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
	}
}
