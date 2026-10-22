using NemoLib.Mvp;
using VContainer;

public class TextSlotPresenter : BaseSlotPresenter<TextSlotModel>
{
	[InjectModel]
	private TextSlotModel _model;

	[View]
	private TextSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override TextSlotModel SlotModel => null;

	private void OnInject(TextSlotModel model)
	{
	}
}
