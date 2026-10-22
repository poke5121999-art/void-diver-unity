using NemoLib.Mvp;

public class SampleSlotPresenter : BaseSlotPresenter<SampleSlotModel>
{
	[InjectModel]
	private SampleSlotModel _model;

	[View]
	private SampleSlotView _view;

	protected override SampleSlotModel SlotModel => _model;

	private void OnInject(SampleSlotModel model)
	{
		InitSlot(model);
	}
}
