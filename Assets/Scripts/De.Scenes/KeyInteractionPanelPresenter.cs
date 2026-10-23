using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class KeyInteractionPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private KeyInteractionPanelModel _model;

	[View]
	private KeyInteractionPanelView _view;

	private readonly List<InventoryGoodsSlotModel> _slotModels = new List<InventoryGoodsSlotModel>();

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	private void OnInject(KeyInteractionPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		for (int i = 0; i < _view.GoodsSlots.Length; i++)
		{
			if (_model.RequiredKeyCount <= i)
			{
				_view.GoodsSlots[i].SetActive(false);
			}
		}
		InitSlots();
		_model.RxSelectedItemIds.ObserveChange().Subscribe(delegate
		{
			OnUpdateSlots();
		}).AddTo(_disposable);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConfirmClick();
		}).AddTo(_disposable);
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitGamePadNav()
	{
		SetNavVertical(new Selectable[1] { _view.Confirm_Button });
	}

	private void InitSlots()
	{
		_slotModels.Clear();
		for (int i = 0; i < _model.RequiredKeyCount; i++)
		{
			InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.KeySlot, (Goods)null);
			inventoryGoodsSlotModel.OnClick.Subscribe(OnSlotClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
			inventoryGoodsSlotModel.OnExit.Subscribe(OnSlotExit).AddTo(_disposable);
			inventoryGoodsSlotModel.OnDrag.Subscribe(_model.OnSlotDrag).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(_model.OnSlotEndDrag).AddTo(_disposable);
			inventoryGoodsSlotModel.UseTooltip = false;
			inventoryGoodsSlotModel.ShowItemCount = false;
			_view.GoodsSlots[i].GetComponent<InventoryGoodsSlotPresenter>().Inject(inventoryGoodsSlotModel);
			_slotModels.Add(inventoryGoodsSlotModel);
		}
	}

	private void OnUpdateSlots()
	{
		for (int i = 0; i < _model.RequiredKeyCount; i++)
		{
			GoodsSlot goodsSlot;
			if (!_model.RxSelectedItemIds.Any() || _model.RxSelectedItemIds.Count <= i || _model.RxSelectedItemIds[i] < 1)
			{
				goodsSlot = new GoodsSlot
				{
					SlotIndex = i,
					Goods = null
				};
			}
			else
			{
				goodsSlot = new GoodsSlot
				{
					SlotIndex = i,
					Goods = GoodsCreator.MakeItemGoods(_model.RxSelectedItemIds[i])
				};
			}
			_slotModels[i].GoodsSlot = goodsSlot;
		}
	}

	private void SetFocusedSlot(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedSlotModel.Value = slotModel;
	}

	private void OnConfirmClick()
	{
		_model.OnConfirmClick.OnNext(Unit.Default);
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel.GoodsSlot != null)
		{
			_model.OnSlotClick.OnNext(slotModel);
		}
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		SetFocusedSlot(slotModel);
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
		SetFocusedSlot(null);
	}
}
