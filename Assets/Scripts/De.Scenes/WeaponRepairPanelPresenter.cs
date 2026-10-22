using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class WeaponRepairPanelPresenter : BasePanelPresenter
{
	[View]
	private WeaponRepairPanelView _view;

	[InjectModel]
	private WeaponRepairPanelModel _model;

	private InventoryGoodsSlotModel _goodsSlotModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(WeaponRepairPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_goodsSlotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.None, new GoodsSlot());
		_view.GoodsSlot.Inject(_goodsSlotModel);
		_view.GoodsTooltip.gameObject.SetActive(value: false);
		model.RxSelectedGoods.Subscribe(OnSelectedGoodsChanged).AddTo(_disposable);
		_goodsSlotModel.OnLeftClick.Subscribe(delegate
		{
			_model.OnGoodsSlotLeftClick.OnNext(Unit.Default);
		}).AddTo(_disposable);
		_goodsSlotModel.OnEnter.Subscribe(OnGoodsSlotEnter).AddTo(_disposable);
		_goodsSlotModel.OnExit.Subscribe(delegate
		{
			HideTooltip();
		}).AddTo(_disposable);
		_view.Confirm.OnClickWithSfx().Subscribe(delegate
		{
			OnConfirmClicked();
		}).AddTo(_disposable);
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		SetNavHorizontal(new List<Selectable> { _view.Confirm });
	}

	private void OnSelectedGoodsChanged(Goods goods)
	{
		bool flag = goods != null;
		bool flag2 = goods != null && goods.CanRepair();
		_view.Body.SetActive(flag);
		_view.Buttons.SetActive(flag);
		_view.Confirm.gameObject.SetActive(flag2);
		_view.Locked.gameObject.SetActive(flag && !flag2);
		if (goods != null)
		{
			_goodsSlotModel.GoodsSlot = new GoodsSlot
			{
				Goods = goods
			};
			_view.Confirm_Gold_Value.text = string.Format("{0}", goods.GetRepairCost());
		}
		else
		{
			_goodsSlotModel.GoodsSlot = new GoodsSlot();
			HideTooltip();
		}
	}

	private void OnGoodsSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel.IsSlotEmpty)
		{
			HideTooltip();
		}
		else
		{
			ShowTooltip(slotModel.GoodsSlot.Goods);
		}
	}

	private void ShowTooltip(Goods goods)
	{
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		_view.GoodsTooltip.Inject(new GoodsTooltipModel
		{
			Goods = goods
		});
	}

	private void HideTooltip()
	{
		_view.GoodsTooltip.gameObject.SetActive(value: false);
	}

	private void OnConfirmClicked()
	{
		Goods value = _model.RxSelectedGoods.Value;
		if (value != null && value.CanRepair())
		{
			_model.OnConfirmRepair.OnNext(value);
		}
	}
}
