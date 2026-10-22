using NemoLib.AssetManagement;
using GameCommon.Enum;
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

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	protected void OnInject(GoodsTooltipModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.Item.gameObject.SetActive(model.Goods?.Category == EGoodsCategory.Item);
		_view.Equipment.gameObject.SetActive(model.Goods?.Category == EGoodsCategory.Equipment);
		_view.Bag.gameObject.SetActive(model.Goods?.Category == EGoodsCategory.Bag);
		_view.KeyLayout_KeyGuideN.SetActive(model.ShowKeyPrompt);
		_view.KeyLayout_KeyGuideMoveToCrafting.SetActive(model.IsCraftingAvailable);
		switch (_model.Goods.Category)
		{
		case EGoodsCategory.Item:
			_view.Item.GetComponent<Presenter>().Inject(new ItemTooltipModel
			{
				Goods = _model.Goods
			});
			break;
		case EGoodsCategory.Equipment:
			_view.Equipment.GetComponent<Presenter>().Inject(new EquipmentTooltipModel
			{
				Goods = _model.Goods,
				IsArtifactShop = _model.IsArtifactShop,
				IsArtifactDeal = _model.IsArtifactDeal,
				HideEffectGroup = _model.HideEffectGroup
			});
			break;
		case EGoodsCategory.Bag:
			_view.Bag.GetComponent<Presenter>().Inject(new BagTooltipModel
			{
				Goods = _model.Goods
			});
			break;
		}
	}
}
