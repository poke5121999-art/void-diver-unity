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

	protected override TextSlotModel SlotModel => _model;

	private void OnInject(TextSlotModel model)
	{
		InitSlot(model);
		_view.Text.text = model.Text;
	}
}
