using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using Tracker;
using UniRx;
using GameCommon.Data;
using GameCommon.Enum;
using UnityEngine;

public class PlayerInventoryScrollerPresenter : GridScrollerPresenter<InventoryGoodsSlotModel, InventoryGoodsSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private PlayerInventoryScrollerModel _model;

	private List<InventoryGoodsSlotModel> _inventorySlotModels;

	private readonly List<InventoryGoodsSlotModel> _appendedSlotModels = new List<InventoryGoodsSlotModel>();

	private CompositeDisposable _disposable;

	private CompositeDisposable _appendedSlotDisposable;

	protected override BaseScrollerModel<InventoryGoodsSlotModel> ScrollerModel => _model;

	private int NumberOfInventorySlots => _model.PlayerInventory.SlotCount;

	private int RowsPerPage => 5;

	private int SlotsPerRow => 6;

	private float PageChangeThreshold => 20f;

	private IDictionary<int, GoodsSlot> InventorySlots => _model.PlayerInventory.Slots;

	private IDictionary<int, GoodsSlot> SafeSlots => _model.PlayerInventory.SafeSlots;

	protected override int GetColumnCount()
	{
		return 6;
	}

	private void OnInject(PlayerInventoryScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		Scroller.snapping = false;
		Scroller.scrollerScrolled = OnScrollerScrolled;
		InitSlots();
		if (_model.DisplayScope == EGoodsDisplayScope.AllOwned)
		{
			RebuildAppendedSlots();
		}
		RefreshScroller();
		_model.RxPageIndex.Value = 0;
		model.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
		model.OnSlotExit.Subscribe(OnSlotExit).AddTo(_disposable);
		model.PlayerInventory.OnInventoryChanged.Subscribe(OnInventoryChanged).AddTo(_disposable);
		if (_model.DisplayScope == EGoodsDisplayScope.AllOwned)
		{
			model.PlayerInventory.OnSafeInventoryChanged.Subscribe(OnSafeInventoryChanged).AddTo(_disposable);
		}
		model.OnMovePage.Subscribe(OnMovePage).AddTo(_disposable);
		model.PlayerInventory.Player.RxTalent.Skip(1).Subscribe(delegate
		{
			OnTalnetUpdated();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool enabled) => enabled).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool enabled) => !enabled).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		_appendedSlotDisposable?.Dispose();
		base.OnDestroy();
	}

	private void InitSlots()
	{
		_inventorySlotModels = Enumerable.Range(0, _model.PlayerInventory.SlotCount).Select(delegate(int slotIndex)
		{
			GoodsSlot value;
			GoodsSlot slot = ((!InventorySlots.TryGetValue(slotIndex, out value)) ? new GoodsSlot
			{
				SlotIndex = slotIndex
			} : value.Clone());
			InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.InventorySlot, slot, InventoryGoodsSlotPresenter.ELayout.Normal);
			inventoryGoodsSlotModel.CheckMaxItemCount = true;
			inventoryGoodsSlotModel.OnDrag.Subscribe(delegate(InventoryGoodsSlotModel _)
			{
				_model.OnSlotDrag.OnNext(_);
			}).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(delegate(InventoryGoodsSlotModel _)
			{
				_model.OnSlotEndDrag.OnNext(_);
			}).AddTo(_disposable);
			return inventoryGoodsSlotModel;
		}).ToList();
	}

	private void OnTalnetUpdated()
	{
		if (_inventorySlotModels.Count != _model.PlayerInventory.SlotCount)
		{
			InitSlots();
			if (_model.DisplayScope == EGoodsDisplayScope.AllOwned)
			{
				RebuildAppendedSlots();
			}
			RefreshScroller(Scroller.ScrollPosition);
		}
	}

	private void RebuildAppendedSlots()
	{
		_appendedSlotDisposable?.Dispose();
		_appendedSlotDisposable = new CompositeDisposable();
		_appendedSlotModels.Clear();
		foreach (GoodsSlot value in _model.PlayerInventory.Slots.Values)
		{
			CollectBagContents(value);
		}
		foreach (GoodsSlot value2 in _model.PlayerInventory.SafeSlots.Values)
		{
			if (value2 != null && !value2.IsEmpty && !(value2.Goods is BagGoods) && CheckFilter(value2))
			{
				AddAppendedSlot(value2);
			}
		}
		foreach (GoodsSlot value3 in _model.PlayerInventory.SafeSlots.Values)
		{
			CollectBagContents(value3);
		}

		void AddAppendedSlot(GoodsSlot goodsSlot)
		{
			InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.InventorySlot, goodsSlot.Clone(), InventoryGoodsSlotPresenter.ELayout.Normal);
			inventoryGoodsSlotModel.CheckMaxItemCount = true;
			inventoryGoodsSlotModel.OnDrag.Subscribe(delegate(InventoryGoodsSlotModel _)
			{
				_model.OnSlotDrag.OnNext(_);
			}).AddTo(_appendedSlotDisposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(delegate(InventoryGoodsSlotModel _)
			{
				_model.OnSlotEndDrag.OnNext(_);
			}).AddTo(_appendedSlotDisposable);
			_appendedSlotModels.Add(inventoryGoodsSlotModel);
		}

		void CollectBagContents(GoodsSlot holderSlot)
		{
			if (holderSlot?.Goods is BagGoods bagGoods && bagGoods.GoodsSlots != null)
			{
				for (int i = 0; i < bagGoods.GoodsSlots.Length; i++)
				{
					GoodsSlot goodsSlot = bagGoods.GoodsSlots[i];
					if (goodsSlot != null && goodsSlot.Goods != null && CheckFilter(goodsSlot))
					{
						AddAppendedSlot(goodsSlot);
					}
				}
			}
		}
	}

	protected override InventoryGoodsSlotModel[] BuildSlotModels()
	{
		IEnumerable<InventoryGoodsSlotModel> source = _inventorySlotModels.Where((InventoryGoodsSlotModel slotModel) => CheckFilter(slotModel.GoodsSlot));
		if (_model.DisplayScope != EGoodsDisplayScope.AllOwned)
		{
			return source.ToArray();
		}
		List<InventoryGoodsSlotModel> list = source.ToList();
		HashSet<int> hashSet = new HashSet<int>(list.Where((InventoryGoodsSlotModel _) => !_.GoodsSlot.IsEmpty).Select((InventoryGoodsSlotModel _) => _.GoodsSlot.Id));
		foreach (InventoryGoodsSlotModel appendedSlotModel in _appendedSlotModels)
		{
			if (hashSet.Add(appendedSlotModel.GoodsSlot.Id))
			{
				list.Add(appendedSlotModel);
			}
		}
		foreach (InventoryGoodsSlotModel item in list)
		{
			ApplyTotalOwnedCount(item);
		}
		return list.ToArray();
	}

	private void ApplyTotalOwnedCount(InventoryGoodsSlotModel slotModel)
	{
		GoodsSlot goodsSlot = slotModel.GoodsSlot;
		if (!goodsSlot.IsEmpty && goodsSlot.Goods.IsItem())
		{
			long itemCount = _model.PlayerInventory.GetItemCount(goodsSlot.Id);
			if (itemCount != goodsSlot.Count)
			{
				GoodsSlot goodsSlot2 = goodsSlot.Clone();
				goodsSlot2.Goods = GoodsCreator.MakeGoods(goodsSlot.Type, goodsSlot.Id, itemCount, 0L);
				slotModel.GoodsSlot = goodsSlot2;
			}
		}
	}

	protected override InventoryGoodsSlotRowModel[] BuildRowModels(InventoryGoodsSlotModel[] slots)
	{
		_model.RxIsEmpty.Value = slots.Length == 0;
		int rows = (int)Math.Ceiling((float)slots.Length / 6f);
		_model.RxPageCount.Value = Math.Max(1, (int)Math.Ceiling((float)rows / 5f));
		return Enumerable.Range(0, rows).Select((int i) => new InventoryGoodsSlotRowModel
		{
			SlotModels = slots.Skip(i * 6).Take(6).ToArray()
		}).ToArray();
	}

	private void OnScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
	{
		if (CellViewModels.Count == 0)
		{
			return;
		}
		float num = Mathf.Max(0f, Scroller.ScrollPosition - (float)Scroller.padding.top);
		int num2 = (int)Math.Floor((num + 20f) / ((_cellSize + Scroller.spacing) * 5f));
		if (val.y <= 0f)
		{
			num2 = _model.RxPageCount.Value - 1;
		}
		num2 = Mathf.Clamp(num2, 0, _model.RxPageCount.Value - 1);
		_model.RxPageIndex.Value = num2;
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedSlotModel.SetValueAndForceNotify(slotModel);
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
		if (_model.RxFocusedSlotModel.Value != null && _model.RxFocusedSlotModel.Value == slotModel)
		{
			_model.RxFocusedSlotModel.Value = null;
		}
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		bool flag = HasFilter();
		bool flag2 = false;
		foreach (GoodsSlotChangeData changeData in changeDatas)
		{
			InventoryGoodsSlotModel inventoryGoodsSlotModel = _inventorySlotModels[changeData.Key];
			GoodsSlot goodsSlot;
			switch (changeData.Operation)
			{
			case EDataOperation.Remove:
				goodsSlot = new GoodsSlot
				{
					SlotIndex = changeData.Key
				};
				break;
			case EDataOperation.Add:
			case EDataOperation.Replace:
				goodsSlot = changeData.NewSlot;
				break;
			default:
				continue;
			}
			if (flag && !flag2 && CheckFilter(inventoryGoodsSlotModel.GoodsSlot) != CheckFilter(goodsSlot))
			{
				flag2 = true;
			}
			inventoryGoodsSlotModel.GoodsSlot = goodsSlot;
		}
		if (_model.DisplayScope == EGoodsDisplayScope.AllOwned)
		{
			RebuildAppendedSlots();
		}
		else if (!flag2)
		{
			return;
		}
		RefreshScroller(Scroller.ScrollPosition);
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		RebuildAppendedSlots();
		RefreshScroller(Scroller.ScrollPosition);
	}

	private void OnMovePage(int index)
	{
		if (!(Scroller == null) && Scroller.gameObject.activeInHierarchy)
		{
			index = Mathf.Clamp(index, 0, _model.RxPageCount.Value - 1);
			Scroller.JumpToDataIndex(index * 5, 0f, 0f, true, EnhancedScroller.TweenType.immediate, 0f, null, EnhancedScroller.LoopJumpDirectionEnum.Closest, false);
		}
	}

	private bool HasFilter()
	{
		if (_model.ArtifactFilterTypes != null && !_model.ArtifactFilterTypes.Contains(EArtifactCategory.All))
		{
			return true;
		}
		if (_model.AllowedSubTypes != null)
		{
			return true;
		}
		if (_model.KeyItemFilterTypes != null)
		{
			return _model.KeyItemFilterTypes.Length > 0;
		}
		return false;
	}

	private bool CheckFilter(GoodsSlot goodsSlot)
	{
		if (goodsSlot.IsEmpty)
		{
			return !HasFilter();
		}
		return goodsSlot.Goods.CheckFilter(_model.AllowedSubTypes, _model.ArtifactFilterTypes, _model.KeyItemFilterTypes);
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
