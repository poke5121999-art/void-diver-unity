using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class ItemTooltipPresenter : BasePanelPresenter
{
	[View]
	private ItemTooltipView _view;

	[InjectModel]
	private ItemTooltipModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private const string ITEM_MAX_COLOR = "#a74200";

	protected override BasePanelModel PanelModel => null;

	private GamePlayer Player => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(ItemTooltipModel model)
	{
	}

	private void UpdateItemUis()
	{
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
	}
}
