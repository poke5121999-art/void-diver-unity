using NemoLib.Mvp;
using VContainer;

public class MaterialSlotPresenter : Presenter
{
	[View]
	private MaterialSlotView _view;

	[InjectModel]
	private MaterialSlotModel _model;

	[Inject]
	private PlayerManager _playerManager;

	private void OnInject(MaterialSlotModel model)
	{
	}

	private void UpdateUI()
	{
	}

	private void UpdateUnknownUI()
	{
	}
}
