using NemoLib.Mvp;
using VContainer;

public class PlayerSaveSlotPresenter : BaseSlotPresenter<PlayerSaveSlotModel>
{
	[InjectModel]
	private PlayerSaveSlotModel _model;

	[View]
	private PlayerSaveSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override PlayerSaveSlotModel SlotModel => null;

	private void OnInject(PlayerSaveSlotModel model)
	{
	}

	private void UpdateUis()
	{
	}

	private void OnPlayerSlotUpdated(int slotIndex)
	{
	}
}
