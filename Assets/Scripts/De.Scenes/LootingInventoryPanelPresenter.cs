using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using SomeNet.Collections;
using Tracker;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LootingInventoryPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private LootingInventoryPanelModel _model;

	[View]
	private LootingInventoryPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private readonly List<InventoryGoodsSlotModel> _goodsSlotModels = new List<InventoryGoodsSlotModel>();

	private const int _numberOfSlotsInRow = 6;

	protected override BasePanelModel PanelModel => _model;

	private int SlotCount => _model.LootingInventory.SlotCount;

	private int FilledSlots => _model.LootingInventory.Slots.Values.Count((GoodsSlot _) => !_.IsEmpty);

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	protected void OnInject(LootingInventoryPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.Caption.text = TLocalizedText.LootingInventory.LocalText;
		_view.GoodsCountText.text = string.Format("{0}/{1}", FilledSlots, SlotCount);
		InitSlots();
		InitGamePadNav();
		_model.LootingInventory.OnInventoryChanged.Subscribe(OnInventoryChanged).AddTo(_disposable);
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
		ClearNav();
		List<Button> items = _view.Slots.Take(_model.LootingInventory.SlotCount).Select((GameObject _) => _.GetComponent<Button>()).ToList();
		SetNavGrid(items, 6);
	}

	private void InitSlots()
	{
		_goodsSlotModels.Clear();
		int num = (int)Math.Ceiling((float)_model.LootingInventory.SlotCount / 6f) * 6;
		for (int i = 0; i < num; i++)
		{
			_goodsSlotModels.Add((i < _model.LootingInventory.SlotCount) ? CreateSlotModel(i) : CreateLockedSlotModel());
		}
		for (int j = 0; j < _view.Slots.Length; j++)
		{
			GameObject gameObject = _view.Slots[j];
			if (j < _goodsSlotModels.Count)
			{
				gameObject.SetActive(value: true);
				gameObject.GetComponent<InventoryGoodsSlotPresenter>().Inject(_goodsSlotModels[j]);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
		static InventoryGoodsSlotModel CreateLockedSlotModel()
		{
			return new InventoryGoodsSlotModel(EGoodsSlotCategory.LootingSlot, new GoodsSlot(), InventoryGoodsSlotPresenter.ELayout.Locked);
		}
		InventoryGoodsSlotModel CreateSlotModel(int index)
		{
			if (!_model.LootingInventory.Slots.TryGetValue(index, out var value))
			{
				value = new GoodsSlot();
			}
			InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.LootingSlot, value, GetSlotLayout(value));
			inventoryGoodsSlotModel.OnClick.Subscribe(OnSlotClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnLeftClick.Subscribe(OnSlotLeftClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnRightClick.Subscribe(OnSlotRightClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
			inventoryGoodsSlotModel.OnExit.Subscribe(OnSlotExit).AddTo(_disposable);
			inventoryGoodsSlotModel.OnDrag.Subscribe(OnSlotDrag).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(OnSlotEndDrag).AddTo(_disposable);
			return inventoryGoodsSlotModel;
		}
	}

	private int GetRevealingSlotIndex()
	{
		GoodsSlot goodsSlot = _model.LootingInventory.Slots.Values.OrderBy((GoodsSlot _) => _.SlotIndex).FirstOrDefault((GoodsSlot _) => !_.IsRevealed);
		if (goodsSlot == null)
		{
			return -1;
		}
		return goodsSlot.SlotIndex;
	}

	private InventoryGoodsSlotPresenter.ELayout GetSlotLayout(GoodsSlot slot)
	{
		if (slot.IsEmpty)
		{
			return InventoryGoodsSlotPresenter.ELayout.Normal;
		}
		if (slot.IsRevealed)
		{
			return InventoryGoodsSlotPresenter.ELayout.Normal;
		}
		if (slot.SlotIndex == GetRevealingSlotIndex())
		{
			return InventoryGoodsSlotPresenter.ELayout.Revealing;
		}
		return InventoryGoodsSlotPresenter.ELayout.Unrevealed;
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
		_model.OnSlotClick.OnNext(slotModel);
	}

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		_model.OnSlotLeftClick.OnNext(slotModel);
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		_model.OnSlotRightClick.OnNext(slotModel);
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedSlotModel.SetValueAndForceNotify(slotModel);
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

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
		if (_model.RxFocusedSlotModel.Value != null && !(_model.RxFocusedSlotModel.Value != slotModel))
		{
			_model.RxFocusedSlotModel.Value = null;
			_view.GoodsTooltip.gameObject.SetActive(value: false);
		}
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
		_model.OnSlotDrag.OnNext(slotModel);
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
		_model.OnSlotEndDrag.OnNext(slotModel);
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		_view.GoodsCountText.text = string.Format("{0}/{1}", FilledSlots, SlotCount);
		changeDatas.ForEach(delegate(GoodsSlotChangeData _)
		{
			InventoryGoodsSlotModel inventoryGoodsSlotModel = _goodsSlotModels[_.Key];
			switch (_.Operation)
			{
			case EDataOperation.Remove:
				inventoryGoodsSlotModel.GoodsSlot = new GoodsSlot
				{
					SlotIndex = _.Key
				};
				break;
			case EDataOperation.Add:
			case EDataOperation.Replace:
				inventoryGoodsSlotModel.GoodsSlot = _.NewSlot;
				break;
			}
		});
		int revealingSlotIndex = GetRevealingSlotIndex();
		if (revealingSlotIndex >= 0)
		{
			_goodsSlotModels[revealingSlotIndex].RxLayout.Value = InventoryGoodsSlotPresenter.ELayout.Revealing;
		}
	}
}
