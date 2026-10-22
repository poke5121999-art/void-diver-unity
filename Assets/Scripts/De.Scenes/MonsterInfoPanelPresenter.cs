using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class MonsterInfoPanelPresenter : BasePanelPresenter
{
	[View]
	private MonsterInfoPanelView _view;

	[InjectModel]
	private MonsterInfoPanelModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(MonsterInfoPanelModel model)
	{
	}

	private void OnBackButtonClick()
	{
	}

	private void InitGamePadNav()
	{
	}

	private string GetCategoryText()
	{
		return null;
	}

	private string GetResistancePercentText(int percent)
	{
		return null;
	}
}
