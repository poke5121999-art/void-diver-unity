using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;

public class WeaponRepairPanelPresenter : BasePanelPresenter
{
	[View]
	private WeaponRepairPanelView _view;

	[InjectModel]
	private WeaponRepairPanelModel _model;

	private InventoryGoodsSlotModel _goodsSlotModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(WeaponRepairPanelModel model)
	{
	}

	private void InitGamePadNav()
	{
	}

	private void OnSelectedGoodsChanged(Goods goods)
	{
	}

	private void OnGoodsSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void ShowTooltip(Goods goods)
	{
	}

	private void HideTooltip()
	{
	}

	private void OnConfirmClicked()
	{
	}
}
