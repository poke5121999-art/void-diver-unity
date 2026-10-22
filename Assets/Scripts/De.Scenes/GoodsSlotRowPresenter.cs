using NemoLib.Mvp;
using UnityEngine;

public class GoodsSlotRowPresenter : BaseRowSlotPresenter<GoodsSlotRowModel, GoodsSlotModel>
{
	[InjectModel]
	private GoodsSlotRowModel _model;

	[View]
	private GoodsSlotRowView _view;

	protected override GameObject[] Slots => _view.Slots;

	protected void OnInject(GoodsSlotRowModel model)
	{
		InitRow(model);
	}
}
