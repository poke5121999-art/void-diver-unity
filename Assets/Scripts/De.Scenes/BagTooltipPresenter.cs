using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class BagTooltipPresenter : BasePanelPresenter
{
	[View]
	private BagTooltipView _view;

	[InjectModel]
	private BagTooltipModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(BagTooltipModel model)
	{
	}

	private void UpdateBagUis()
	{
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
	}
}
