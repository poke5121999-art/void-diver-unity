using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;

public class KeyInteractionPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private KeyInteractionPanelModel _model;

	[View]
	private KeyInteractionPanelView _view;

	private readonly List<InventoryGoodsSlotModel> _slotModels;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	private void OnInject(KeyInteractionPanelModel model)
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitSlots()
	{
	}

	private void OnUpdateSlots()
	{
	}

	private void SetFocusedSlot(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnConfirmClick()
	{
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}
}
