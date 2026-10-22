using NemoLib.Mvp;

public class SampleSlotPresenter : BaseSlotPresenter<SampleSlotModel>
{
	[InjectModel]
	private SampleSlotModel _model;

	[View]
	private SampleSlotView _view;

	protected override SampleSlotModel SlotModel => null;

	private void OnInject(SampleSlotModel model)
	{
	}
}
