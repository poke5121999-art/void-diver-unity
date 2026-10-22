using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class CampaignFirstRewardSlotPresenter : Presenter
{
	[View]
	private CampaignFirstRewardSlotView _view;

	[InjectModel]
	private CampaignFirstRewardSlotModel _model;

	[Inject]
	private PopupManager _popupManager;

	private CompositeDisposable _disposable;

	public Selectable Selectable => _view.GoodsSlot.GetComponent<InventoryGoodsSlotView>().Slot;

	private void OnInject(CampaignFirstRewardSlotModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InventoryGoodsSlotModel slotModel = new InventoryGoodsSlotModel(_model.Goods)
		{
			UseTooltip = false,
			ShowItemCount = true
		};
		slotModel.OnClick.Subscribe(delegate
		{
			_popupManager.Open<GoodsTooltipPopupPresenter>(new GoodsTooltipPopupModel
			{
				Goods = _model.Goods
			});
		}).AddTo(_disposable);
		_view.GoodsSlot.Inject(slotModel);
		_view.Receipted.SetActive(model.IsReceipted);
	}

	private void OnDestroy()
	{
		_disposable.Dispose();
	}
}
