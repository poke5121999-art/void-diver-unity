using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;

public class RegisterItemUsedPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private RegisterItemUsedPanelModel _model;

	[View]
	private RegisterItemUsedPanelView _view;

	private GoodsSlotModel _slotModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(RegisterItemUsedPanelModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void Refresh(Goods goods)
	{
	}

	private void ClearDisplay()
	{
	}

	private void OnSlotEnter(GoodsSlotModel slotModel)
	{
	}

	private void OnSlotExit(GoodsSlotModel slotModel)
	{
	}

	private void OnSlotClick(GoodsSlotModel slotModel)
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
