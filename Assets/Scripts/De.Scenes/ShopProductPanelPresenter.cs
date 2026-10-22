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
	}

	protected override void OnDestroy()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnSlotEnter(ShopProductSlotModel slotModel)
	{
	}

	private void OnSlotExit(ShopProductSlotModel slotModel)
	{
	}
}
