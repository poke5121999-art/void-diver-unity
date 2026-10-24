using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class BagPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private BagPanelModel _model;

	[View]
	private BagPanelView _view;

	[Inject]
	private SoundManager _soundManager;

	private CompositeDisposable _disposable;

	private readonly List<InventoryGoodsSlotModel> _slotModels = new List<InventoryGoodsSlotModel>();

	private Selectable _lastFocusedSlot;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	public void Open()
	{
		gameObject.SetActive(value: true);
		_soundManager.PlaySfx("Bag_Use");
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
		_soundManager.PlaySfx("Bag_Use");
	}

	private void OnInject(BagPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		model.RxBag.Subscribe(delegate
		{
			InitSlots();
		}).AddTo(_disposable);
		model.RxBag.Subscribe(UpdateTitle).AddTo(_disposable);
		model.RxIsDragAcceptable.Subscribe(OnDragAcceptableChanged).AddTo(_disposable);
		model.RxFocusedSlotModel.Subscribe(UpdateTooltip).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool enabled) => enabled).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool enabled) => !enabled).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnDragAcceptableChanged(bool isAcceptable)
	{
		foreach (InventoryGoodsSlotModel slotModel in _slotModels)
		{
			slotModel.RxHighlightOn.Value = isAcceptable;
		}
	}

	private void UpdateTooltip(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			_view.GoodsTooltip.gameObject.SetActive(value: false);
			return;
		}
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		GoodsTooltipModel goodsTooltipModel = new GoodsTooltipModel();
		goodsTooltipModel.Goods = slotModel.GoodsSlot.Goods;
		_view.GoodsTooltip.Inject(goodsTooltipModel);
	}

	private void InitSlots()
	{
		_slotModels.Clear();
		_model.RxFocusedSlotModel.Value = null;
		BagGoods value = _model.RxBag.Value;
		GoodsSlot[] array = value?.GoodsSlots;
		if (array == null)
		{
			for (int i = 0; i < _view.GoodsSlots.Length; i++)
			{
				_view.GoodsSlots[i].SetActive(value: false);
			}
			return;
		}
		int num = Math.Min(array.Length, _view.GoodsSlots.Length);
		for (int j = 0; j < num; j++)
		{
			InventoryGoodsSlotModel slotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.BagSlot, array[j]);
			slotModel.UseTooltip = false;
			_slotModels.Add(slotModel);
			slotModel.OnEnter.Subscribe(delegate(InventoryGoodsSlotModel focused)
			{
				_model.RxFocusedSlotModel.SetValueAndForceNotify(focused);
			}).AddTo(_disposable);
			slotModel.OnExit.Subscribe(delegate
			{
				_model.RxFocusedSlotModel.SetValueAndForceNotify(null);
			}).AddTo(_disposable);
			slotModel.OnLeftClick.Subscribe(delegate(InventoryGoodsSlotModel clicked)
			{
				_model.OnSlotLeftClick.OnNext(clicked);
			}).AddTo(_disposable);
			slotModel.OnDrag.Subscribe(delegate(InventoryGoodsSlotModel dragged)
			{
				_model.OnSlotDrag.OnNext(dragged);
			}).AddTo(_disposable);
			slotModel.OnEndDrag.Subscribe(delegate(InventoryGoodsSlotModel ended)
			{
				_model.OnSlotEndDrag.OnNext(ended);
			}).AddTo(_disposable);
			_view.GoodsSlots[j].GetComponent<InventoryGoodsSlotPresenter>().Inject(slotModel);
			_view.GoodsSlots[j].SetActive(value: true);
		}
		for (int k = num; k < _view.GoodsSlots.Length; k++)
		{
			_view.GoodsSlots[k].SetActive(value: false);
		}
		InitGamePadNav();
	}

	protected override void DoSelect(Selectable selectable)
	{
		_lastFocusedSlot = selectable;
		base.DoSelect(selectable);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		IReadOnlyList<Selectable> items = _view.GoodsSlots.Take(_slotModels.Count).Select((GameObject go) => go.GetComponent<InventoryGoodsSlotView>().Slot).ToList();
		SetNavHorizontal(items);
		if (_lastFocusedSlot != null && items.Contains(_lastFocusedSlot))
		{
			SetDefaultNav(_lastFocusedSlot);
		}
	}

	private void UpdateTitle(BagGoods bag)
	{
		if (bag == null)
		{
			_view.Title.text = string.Empty;
			return;
		}
		TBag tBag = TBag.Table[bag.Id];
		_view.Title.text = string.Format("{0} ({1}/{2})", tBag.Name, bag.OccupiedSlotCount, bag.MaxSlotCount);
	}

	private void OnBack()
	{
		_model.OnBack.OnNext(Unit.Default);
	}
}
