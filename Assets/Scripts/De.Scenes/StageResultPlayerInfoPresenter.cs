using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class StageResultPlayerInfoPresenter : BasePanelPresenter
{
	[InjectModel]
	private StageResultPlayerInfoModel _model;

	[View]
	private StageResultPlayerInfoView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameContext _gameContext;

	private const string COLOR_DONE = "#DCDCDC";

	private const string COLOR_NOT_DONE = "#A45646";

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(StageResultPlayerInfoModel model)
	{
	}
}
