using NemoLib.Mvp;
using UnityEngine.UI;

public class SelectionSlotPresenter : BaseSlotPresenter<SelectionSlotModel>
{
	[View]
	private SelectionSlotView _view;

	[InjectModel]
	private SelectionSlotModel _model;

	protected override SelectionSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(SelectionSlotModel model)
	{
	}
}
