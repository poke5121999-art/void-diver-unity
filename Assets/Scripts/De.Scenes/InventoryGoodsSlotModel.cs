using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class InventoryGoodsSlotModel : BaseSlotModel<InventoryGoodsSlotModel>
{
	public Transform Transform;

	public readonly EGoodsSlotCategory SlotCategory;

	public readonly ReactiveProperty<GoodsSlot> RxGoodsSlot = new ReactiveProperty<GoodsSlot>();

	public readonly ReactiveProperty<InventoryGoodsSlotPresenter.ELayout> RxLayout;

	public readonly ReactiveProperty<bool> RxHighlightOn = new ReactiveProperty<bool>();

	public bool CheckMaxItemCount = true;

	public bool UseTooltip;

	public bool ShowItemCount = true;

	public bool IsUnknown;

	public bool IsCrafting;

	public Subject<Goods> OnCraftingNavigate;

	public readonly Subject<InventoryGoodsSlotModel> OnDrag = new Subject<InventoryGoodsSlotModel>();

	public readonly Subject<InventoryGoodsSlotModel> OnEndDrag = new Subject<InventoryGoodsSlotModel>();

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

	public Vector3 Position => Transform.position;

	public InventoryGoodsSlotPresenter.ELayout Layout => RxLayout.Value;

	public bool IsSlotEmpty
	{
		get
		{
			if (RxLayout.Value != InventoryGoodsSlotPresenter.ELayout.Normal)
			{
				return false;
			}
			GoodsSlot value = RxGoodsSlot.Value;
			if (value == null)
			{
				return true;
			}
			return value.Goods == null;
		}
	}

	public bool IsRevealing => RxLayout.Value == InventoryGoodsSlotPresenter.ELayout.Revealing;

	public EGoodsType GoodsType => RxGoodsSlot.Value?.Goods?.Type ?? EGoodsType.None;

	public int GoodsId => RxGoodsSlot.Value?.Goods?.Id ?? 0;

	public int SlotIndex => RxGoodsSlot.Value?.SlotIndex ?? (-1);

	public InventoryGoodsSlotModel(EGoodsSlotCategory category, GoodsSlot slot, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
		SlotCategory = category;
		RxLayout = new ReactiveProperty<InventoryGoodsSlotPresenter.ELayout>(layout);
		RxGoodsSlot.Value = slot;
	}

	public InventoryGoodsSlotModel(EGoodsSlotCategory category, Goods goods, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
		SlotCategory = category;
		RxLayout = new ReactiveProperty<InventoryGoodsSlotPresenter.ELayout>(layout);
		RxGoodsSlot.Value = new GoodsSlot
		{
			Goods = goods
		};
	}

	public InventoryGoodsSlotModel(GoodsSlot slot, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
		SlotCategory = (EGoodsSlotCategory)0;
		RxLayout = new ReactiveProperty<InventoryGoodsSlotPresenter.ELayout>(layout);
		RxGoodsSlot.Value = slot;
	}

	public InventoryGoodsSlotModel(Goods goods, InventoryGoodsSlotPresenter.ELayout layout = InventoryGoodsSlotPresenter.ELayout.Normal)
	{
		SlotCategory = (EGoodsSlotCategory)0;
		RxLayout = new ReactiveProperty<InventoryGoodsSlotPresenter.ELayout>(layout);
		RxGoodsSlot.Value = new GoodsSlot
		{
			Goods = goods
		};
	}
}
