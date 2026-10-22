using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;

public class ScrapPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private ScrapPanelModel _model;

	[View]
	private ScrapPanelView _view;

	private InventoryGoodsSlotModel _scrapSlotModel;

	private GoodsSlotModel[] _resultSlotModels;

	private CompositeDisposable _resultDisposable;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(ScrapPanelModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void Refresh(TScrap scrapData, long count)
	{
	}

	private void ClearDisplay()
	{
	}

	private void OnScrapSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnScrapSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnScrapSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnResultSlotEnter(GoodsSlotModel slotModel)
	{
	}

	private void OnResultSlotExit(GoodsSlotModel slotModel)
	{
	}

	private void ShowTooltip(Goods goods)
	{
	}

	private void HideTooltip()
	{
	}

	private void OnInvalidChanged(bool isInvalid)
	{
	}
}
