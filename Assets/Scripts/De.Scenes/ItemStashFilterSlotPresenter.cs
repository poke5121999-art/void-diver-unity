using GameCommon.Enum;
using NemoLib.Mvp;
using UnityEngine.UI;

public class ItemStashFilterSlotPresenter : BaseSlotPresenter<ItemStashFilterSlotModel>
{
	[InjectModel]
	private ItemStashFilterSlotModel _model;

	[View]
	private ItemStashFilterSlotView _view;

	protected override ItemStashFilterSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(ItemStashFilterSlotModel model)
	{
	}

	private void OnButtonLayoutChanged(EButtonState state)
	{
	}
}
