using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class StageResultParadoxSlotPresenter : BaseSlotPresenter<StageResultParadoxSlotModel>
{
	[View]
	private StageResultParadoxSlotView _view;

	[InjectModel]
	private StageResultParadoxSlotModel _model;

	[Inject]
	private PopupManager _popupManager;

	protected override StageResultParadoxSlotModel SlotModel => null;

	private void OnInject(StageResultParadoxSlotModel model)
	{
	}

	private void OnSlotClick()
	{
	}
}
