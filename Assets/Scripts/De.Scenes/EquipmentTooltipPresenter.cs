using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class EquipmentTooltipPresenter : BasePanelPresenter
{
	[View]
	private EquipmentTooltipView _view;

	[InjectModel]
	private EquipmentTooltipModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private SeriLogger _logger;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(EquipmentTooltipModel model)
	{
	}

	private void UpdateEquipmentUis()
	{
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
	}

	private void UpdateArtifactUi()
	{
	}

	private void UpdateCorruption()
	{
	}
}
