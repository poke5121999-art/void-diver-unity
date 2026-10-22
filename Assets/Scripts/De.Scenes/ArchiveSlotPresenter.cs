using NemoLib.Mvp;

public class ArchiveSlotPresenter : BaseSlotPresenter<ArchiveSlotModel>
{
	[InjectModel]
	private ArchiveSlotModel _model;

	[View]
	private ArchiveSlotView _view;

	protected override ArchiveSlotModel SlotModel => null;

	private void OnInject(ArchiveSlotModel model)
	{
	}
}
