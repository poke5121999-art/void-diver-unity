using NemoLib.Mvp;
using UniRx;
using VContainer;

public class UnlockMaterialPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private UnlockMaterialPanelModel _model;

	[View]
	private UnlockMaterialPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(UnlockMaterialPanelModel model)
	{
	}

	private void OnUpdateUis()
	{
	}

	private void InitGamePadNav()
	{
	}

	protected override void OnDestroy()
	{
	}
}
