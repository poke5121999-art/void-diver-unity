using NemoLib.Mvp;
using UnityEngine;

public class InventoryGoodsSlotRowPresenter : BaseRowSlotPresenter<InventoryGoodsSlotRowModel, InventoryGoodsSlotModel>
{
	[InjectModel]
	private InventoryGoodsSlotRowModel _model;

	[View]
	private InventoryGoodsSlotRowView _view;

	protected override GameObject[] Slots => null;

	protected void OnInject(InventoryGoodsSlotRowModel model)
	{
	}
}
