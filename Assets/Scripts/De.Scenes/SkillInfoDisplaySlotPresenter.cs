using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class SkillInfoDisplaySlotPresenter : Presenter
{
	[InjectModel]
	private SkillInfoDisplaySlotModel _model;

	[View]
	private SkillInfoDisplaySlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private void OnInject(SkillInfoDisplaySlotModel model)
	{
	}

	private void Refresh()
	{
	}
}
