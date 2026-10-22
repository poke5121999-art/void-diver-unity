using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class EquipmentInfoPanelPresenter : BasePanelPresenter
{
	[View]
	private EquipmentInfoPanelView _view;

	[InjectModel]
	private EquipmentInfoPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(EquipmentInfoPanelModel model)
	{
	}

	private void UpdateEquipmentUis()
	{
	}

	private void UpdateArtifactUi()
	{
	}

	private void UpdateCorruption()
	{
	}

	private void OnBackButtonClick()
	{
	}

	private void InitGamePadNav()
	{
	}
}
