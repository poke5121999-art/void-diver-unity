using NemoLib.Mvp;
using VContainer;

public class BgmSlotPresenter : BaseSlotPresenter<BgmSlotModel>
{
	[InjectModel]
	private BgmSlotModel _model;

	[View]
	private BgmSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override BgmSlotModel SlotModel => null;

	private void OnInject(BgmSlotModel model)
	{
	}

	private void OnProfileChanged(Profile profile)
	{
	}
}
