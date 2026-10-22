using NemoLib.Mvp;

public class PlayerRestoreSlotPresenter : BaseSlotPresenter<PlayerRestoreSlotModel>
{
	[InjectModel]
	private PlayerRestoreSlotModel _model;

	[View]
	private PlayerRestoreSlotView _view;

	protected override PlayerRestoreSlotModel SlotModel => null;

	private void OnInject(PlayerRestoreSlotModel model)
	{
	}
}
