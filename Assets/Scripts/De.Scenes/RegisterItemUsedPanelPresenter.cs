using System.Collections.Generic;
using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class RegisterItemUsedPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private RegisterItemUsedPanelModel _model;

	[View]
	private RegisterItemUsedPanelView _view;

	private GoodsSlotModel _slotModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected void OnInject(RegisterItemUsedPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_slotModel = new GoodsSlotModel(null);
		_slotModel.OnEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
		_slotModel.OnExit.Subscribe(OnSlotExit).AddTo(_disposable);
		_slotModel.OnClick.Subscribe(OnSlotClick).AddTo(_disposable);
		_view.GoodsSlot.Inject(_slotModel);
		_model.SlotModel = _slotModel;
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			_model.OnConfirmButtonClick.OnNext(Unit.Default);
		}).AddTo(_disposable);
		_model.RxGoods.Subscribe(Refresh).AddTo(_disposable);
		_model.RxIsInvalid.Subscribe(OnInvalidChanged).AddTo(_disposable);
		_view.GoodsTooltip.gameObject.SetActive(value: false);
		_view.GuideText.gameObject.SetActive(value: true);
		_view.InvalidText.gameObject.SetActive(value: false);
		_view.TitleText.text = model.Title;
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void InitGamePadNav()
	{
		SetNavVertical(new List<Selectable> { _view.Confirm_Button });
	}

	private void Refresh(Goods goods)
	{
		if (goods == null)
		{
			_slotModel.GoodsSlot = new GoodsSlot
			{
				Goods = null
			};
			_view.GoodsTooltip.gameObject.SetActive(value: false);
		}
		else
		{
			_slotModel.GoodsSlot = new GoodsSlot
			{
				Goods = goods
			};
		}
	}

	private void ClearDisplay()
	{
		_slotModel.GoodsSlot = new GoodsSlot
		{
			Goods = null
		};
		_view.GoodsTooltip.gameObject.SetActive(value: false);
	}

	private void OnSlotEnter(GoodsSlotModel slotModel)
	{
		_model.OnSlotEnter.OnNext(slotModel);
		if (slotModel != null && !slotModel.IsSlotEmpty)
		{
			ShowTooltip(slotModel.GoodsSlot.Goods);
		}
		else
		{
			HideTooltip();
		}
	}

	private void OnSlotExit(GoodsSlotModel slotModel)
	{
		_model.OnSlotExit.OnNext(slotModel);
		HideTooltip();
	}

	private void OnSlotClick(GoodsSlotModel slotModel)
	{
		_model.OnSlotClick.OnNext(Unit.Default);
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

	private void OnInvalidChanged(bool isInvalid)
	{
		_view.GuideText.gameObject.SetActive(!isInvalid);
		_view.InvalidText.gameObject.SetActive(isInvalid);
	}
}
