using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;

public class ShopProductPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private ShopProductPanelModel _model;

	[View]
	private ShopProductPanelView _view;

	private ShopProductScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(ShopProductPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_scrollerModel = new ShopProductScrollerModel();
		_scrollerModel.ShopProductGroupId = _model.ShopProductGroupId;
		_scrollerModel.OnSlotLeftClick.Subscribe(_model.OnSlotLeftClick).AddTo(_disposable);
		_scrollerModel.OnSlotRightClick.Subscribe(_model.OnSlotRightClick).AddTo(_disposable);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(_disposable);
		_scrollerModel.OutOfLeft.Subscribe(delegate
		{
			TriggerOutOfLeft();
		}).AddTo(_disposable);
		_scrollerModel.OutOfRight.Subscribe(delegate
		{
			TriggerOutOfRight();
		}).AddTo(_disposable);
		_view.Scroller.GetComponent<ShopProductScrollerPresenter>().Inject(_scrollerModel);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		_scrollerModel.EntryDirection = _model.EntryDirection;
		_scrollerModel.RxIsInputEnabled.Value = true;
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnSlotEnter(ShopProductSlotModel slotModel)
	{
		Goods goods = TShopProduct.Table[slotModel.Id].GainGoodsData.ToGoodsAsBase();
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		_view.GoodsTooltip.GetComponent<GoodsTooltipPresenter>().Inject(new GoodsTooltipModel
		{
			Goods = goods
		});
		_model.RxFocusedSlotModel.Value = slotModel;
	}

	private void OnSlotExit(ShopProductSlotModel slotModel)
	{
		if (slotModel == _model.RxFocusedSlotModel.Value)
		{
			_view.GoodsTooltip.gameObject.SetActive(value: false);
			_model.RxFocusedSlotModel.Value = null;
		}
	}
}
