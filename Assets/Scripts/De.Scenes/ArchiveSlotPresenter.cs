using NemoLib.Mvp;

public class ArchiveSlotPresenter : BaseSlotPresenter<ArchiveSlotModel>
{
	[InjectModel]
	private ArchiveSlotModel _model;

	[View]
	private ArchiveSlotView _view;

	protected override ArchiveSlotModel SlotModel => _model;

	private void OnInject(ArchiveSlotModel model)
	{
		InitSlot(model);
		_view.Name.text = model.Category.ToLocalText();
		_view.Desc.text = model.Category.ToLocalText("Desc");
	}
}
