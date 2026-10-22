using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

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

	protected override BasePanelModel PanelModel => _model;

	protected void OnInject(ScrapPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_scrapSlotModel = new InventoryGoodsSlotModel((Goods)null);
		_scrapSlotModel.OnEnter.Subscribe(OnScrapSlotEnter).AddTo(_disposable);
		_scrapSlotModel.OnExit.Subscribe(OnScrapSlotExit).AddTo(_disposable);
		_scrapSlotModel.OnClick.Subscribe(OnScrapSlotClick).AddTo(_disposable);
		_view.ScrapBox_GoodsSlot.Inject(_scrapSlotModel);
		_model.ScrapSlotModel = _scrapSlotModel;
		_view.ScrapBox_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			_model.OnScrapButtonClick.OnNext(Unit.Default);
		}).AddTo(_disposable);
		Observable.CombineLatest(_model.RxScrapData, _model.RxScrapCount, (TScrap data, long count) => (data, count)).Subscribe(delegate((TScrap, long) t)
		{
			Refresh(t.Item1, t.Item2);
		}).AddTo(_disposable);
		_model.RxIsInvalid.Subscribe(OnInvalidChanged).AddTo(_disposable);
		_view.GoodsTooltip.gameObject.SetActive(value: false);
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
		_resultDisposable?.Dispose();
	}

	private void InitGamePadNav()
	{
		ClearNav();
		Selectable scrapSlot = _view.ScrapBox_GoodsSlot.GetComponent<InventoryGoodsSlotView>().Slot;
		ConnectNav(_view.ScrapBox_Button, ENavDir.Up, scrapSlot);
		ConnectNav(scrapSlot, ENavDir.Down, _view.ScrapBox_Button);
		List<Selectable> resultSelectables = new List<Selectable>();
		GameObject[] slots = _view.ResultBox_Slots;
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].activeSelf)
			{
				resultSelectables.Add(slots[i].GetComponent<GoodsSlotView>().Slot);
			}
		}
		SetNavHorizontal(resultSelectables);
		if (resultSelectables.Count > 0)
		{
			ConnectNav(_view.ScrapBox_Button, ENavDir.Down, resultSelectables[0]);
			foreach (Selectable selectable in resultSelectables)
			{
				ConnectNav(selectable, ENavDir.Up, _view.ScrapBox_Button);
			}
		}
		SetDefaultNav(_view.ScrapBox_Button);
	}

	private void Refresh(TScrap scrapData, long count)
	{
		if (scrapData == null || count < 1)
		{
			ClearDisplay();
			return;
		}
		_resultDisposable?.Dispose();
		_resultDisposable = new CompositeDisposable();
		GoodsData insertGoodsData = scrapData.InsertGoodsData;
		Goods scrapGoods = ((_model.RegisteredGoods.Category != EGoodsCategory.Item) ? _model.RegisteredGoods.Clone() : GoodsCreator.MakeFromGoodsData(insertGoodsData, insertGoodsData.Count * count, 0L));
		_scrapSlotModel.GoodsSlot = new GoodsSlot { Goods = scrapGoods };
		GoodsData[] resultGoodsData = scrapData.ResultGoodsData;
		_resultSlotModels = new GoodsSlotModel[resultGoodsData.Length];
		GameObject[] slots = _view.ResultBox_Slots;
		for (int i = 0; i < slots.Length; i++)
		{
			if (i < resultGoodsData.Length)
			{
				slots[i].SetActive(value: true);
				GoodsSlotModel resultModel = new GoodsSlotModel(GoodsCreator.MakeFromGoodsData(resultGoodsData[i], count * resultGoodsData[i].Count, 0L));
				resultModel.OnEnter.Subscribe(OnResultSlotEnter).AddTo(_resultDisposable);
				resultModel.OnExit.Subscribe(OnResultSlotExit).AddTo(_resultDisposable);
				slots[i].GetComponent<GoodsSlotPresenter>().Inject(resultModel);
				_resultSlotModels[i] = resultModel;
			}
			else
			{
				slots[i].SetActive(value: false);
			}
		}
		_view.ResultBox.SetActive(value: true);
		InitGamePadNav();
	}

	private void ClearDisplay()
	{
		_resultDisposable?.Dispose();
		_scrapSlotModel.GoodsSlot = new GoodsSlot { Goods = null };
		_resultSlotModels = null;
		GameObject[] slots = _view.ResultBox_Slots;
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].SetActive(value: false);
		}
		_view.ResultBox.SetActive(value: false);
		_view.GoodsTooltip.gameObject.SetActive(value: false);
		InitGamePadNav();
	}

	private void OnScrapSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		_model.OnScrapSlotEnter.OnNext(slotModel);
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			HideTooltip();
		}
		else
		{
			ShowTooltip(slotModel.GoodsSlot.Goods);
		}
	}

	private void OnScrapSlotExit(InventoryGoodsSlotModel slotModel)
	{
		_model.OnScrapSlotExit.OnNext(slotModel);
		HideTooltip();
	}

	private void OnScrapSlotClick(InventoryGoodsSlotModel slotModel)
	{
		_model.OnScrapSlotClick.OnNext(Unit.Default);
	}

	private void OnResultSlotEnter(GoodsSlotModel slotModel)
	{
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			HideTooltip();
		}
		else
		{
			ShowTooltip(slotModel.GoodsSlot.Goods);
		}
	}

	private void OnResultSlotExit(GoodsSlotModel slotModel)
	{
		HideTooltip();
	}

	private void ShowTooltip(Goods goods)
	{
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		_view.GoodsTooltip.GetComponent<GoodsTooltipPresenter>().Inject(new GoodsTooltipModel
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
