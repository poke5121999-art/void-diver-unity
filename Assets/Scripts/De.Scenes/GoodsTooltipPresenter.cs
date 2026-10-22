using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class GoodsTooltipPresenter : BasePanelPresenter
{
	[InjectModel]
	private GoodsTooltipModel _model;

	[View]
	private GoodsTooltipView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	protected void OnInject(GoodsTooltipModel model)
	{
	}
}
