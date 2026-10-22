using NemoLib.Mvp;
using UnityEngine.UI;

public class LostItemSlotPresenter : BaseSlotPresenter<LostItemSlotModel>
{
	[InjectModel]
	private LostItemSlotModel _model;

	[View]
	private LostItemSlotView _view;

	protected override LostItemSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(LostItemSlotModel model)
	{
	}
}
