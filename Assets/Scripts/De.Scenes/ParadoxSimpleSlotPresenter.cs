using NemoLib.Mvp;

public class ParadoxSimpleSlotPresenter : BaseSlotPresenter<ParadoxSimpleSlotModel>
{
	[InjectModel]
	private ParadoxSimpleSlotModel _model;

	[View]
	private ParadoxSimpleSlotView _view;

	protected override ParadoxSimpleSlotModel SlotModel => null;

	private void OnInject(ParadoxSimpleSlotModel model)
	{
	}
}
