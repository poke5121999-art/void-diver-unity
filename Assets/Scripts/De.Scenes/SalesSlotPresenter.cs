using NemoLib.Mvp;
using VContainer;

public class SalesSlotPresenter : BaseSlotPresenter<SalesSlotModel>
{
	[View]
	private SalesSlotView _view;

	[InjectModel]
	private SalesSlotModel _model;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override SalesSlotModel SlotModel => null;

	private void OnInject(SalesSlotModel model)
	{
	}

	private void Refresh()
	{
	}

	private void SetupEmployee(SalesSlot slot, Employee employee)
	{
	}

	private void SetupArtifact(SalesSlot slot)
	{
	}

	private void SetupSellSlider(SalesSlot slot)
	{
	}
}
