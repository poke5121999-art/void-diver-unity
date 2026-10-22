using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class StashScrollerPresenter : GridScrollerPresenter<InventoryGoodsSlotModel, InventoryGoodsSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private StashScrollerModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private List<InventoryGoodsSlotModel> _slotModels;

	private GoodsSlot[] _lastSeenStashSlotRefs = Array.Empty<GoodsSlot>();

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<InventoryGoodsSlotModel> ScrollerModel => _model;

	private GamePlayer MyPlayer => _gameManager.MyPlayer;

	private int NumberOfStashSlots => MyPlayer.StashSlotCount;

	private int SlotsPerRow => 6;

	private int RowsPerPage => 5;

	private float PageChangeThreshold => 20f;

	private IDictionary<int, GoodsSlot> StashSlots => _playerManager.StashSlots;

	protected override int GetColumnCount()
	{
		return 6;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(StashScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		base.Scroller.snapping = false;
		base.Scroller.scrollerScrolled = OnScrollerScrolled;
		InitSlots();
		SnapshotStashSlotRefs();
		RefreshScroller(0f);
		_model.RxPageIndex.Value = 0;
		model.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
		model.OnSlotExit.Subscribe(OnSlotExit).AddTo(_disposable);
		_playerManager.OnStashChanged.Subscribe(delegate
		{
			OnStashChanged();
		}).AddTo(_disposable);
		_model.RxAllowedSubTypes.Skip(1).Subscribe(OnAllowedSubTypesChanged).AddTo(_disposable);
		_model.OnMovePage.Subscribe(OnMovePage).AddTo(_disposable);
		MyPlayer.RxTalent.Skip(1).Subscribe(delegate
		{
			OnTalnetUpdated();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitSlots()
	{
		_slotModels = Enumerable.Range(0, MyPlayer.StashSlotCount).Select(delegate(int slotIndex)
		{
			GoodsSlot value;
			GoodsSlot goodsSlot = ((!StashSlots.TryGetValue(slotIndex, out value)) ? new GoodsSlot { SlotIndex = slotIndex } : value.Clone());
			InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.StashSlot, goodsSlot);
			inventoryGoodsSlotModel.CheckMaxItemCount = true;
			inventoryGoodsSlotModel.OnDrag.Subscribe(delegate(InventoryGoodsSlotModel m)
			{
				_model.OnSlotDrag.OnNext(m);
			}).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(delegate(InventoryGoodsSlotModel m)
			{
				_model.OnSlotEndDrag.OnNext(m);
			}).AddTo(_disposable);
			return inventoryGoodsSlotModel;
		}).ToList();
	}

	private void OnTalnetUpdated()
	{
		if (_slotModels.Count != MyPlayer.StashSlotCount)
		{
			InitSlots();
			SnapshotStashSlotRefs();
			RefreshScroller(base.Scroller.ScrollPosition);
		}
	}

	protected override InventoryGoodsSlotModel[] BuildSlotModels()
	{
		return _slotModels.Where((InventoryGoodsSlotModel m) => CheckFilter(m.GoodsSlot)).ToArray();
	}

	protected override InventoryGoodsSlotRowModel[] BuildRowModels(InventoryGoodsSlotModel[] slots)
	{
		_model.RxIsEmpty.Value = slots.Length == 0;
		int num = (int)Math.Ceiling((float)slots.Length / 6f);
		_model.RxPageCount.Value = Math.Max(1, (int)Math.Ceiling((float)num / 5f));
		return (from i in Enumerable.Range(0, num)
			select new InventoryGoodsSlotRowModel
			{
				SlotModels = slots.Skip(i * 6).Take(6).ToArray()
			}).ToArray();
	}

	private void OnScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
	{
		if (base.CellViewModels.Count == 0)
		{
			return;
		}
		float num = Mathf.Max(0f, base.Scroller.ScrollPosition - (float)base.Scroller.padding.top);
		int value = (int)Math.Floor((num + 20f) / ((_cellSize + base.Scroller.spacing) * 5f));
		if (val.y <= 0f)
		{
			value = _model.RxPageCount.Value - 1;
		}
		value = Mathf.Clamp(value, 0, _model.RxPageCount.Value - 1);
		_model.RxPageIndex.Value = value;
	}

	private bool CheckFilter(GoodsSlot goodsSlot)
	{
		EArtifactCategory category = ((goodsSlot.Type == EGoodsType.Artifact) ? TEquipment.Table[goodsSlot.Goods.Id].ArtifactCategory : EArtifactCategory.None);
		if (_model.ArtifactFilterTypes != null && !_model.ArtifactFilterTypes.Contains(EArtifactCategory.All) && !_model.ArtifactFilterTypes.Any((EArtifactCategory t) => t == category))
		{
			return false;
		}
		EGoodsType[] value = _model.RxAllowedSubTypes.Value;
		if (value == null)
		{
			return true;
		}
		return value.Any((EGoodsType t) => t == goodsSlot.Type);
	}

	private void OnAllowedSubTypesChanged(EGoodsType[] allowedSubTypes)
	{
		RefreshScroller(0f);
		_model.RxPageIndex.Value = 0;
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

	private void OnStashChanged()
	{
		if (_lastSeenStashSlotRefs.Length != _slotModels.Count)
		{
			SnapshotStashSlotRefs();
		}
		bool flag = false;
		for (int i = 0; i < _slotModels.Count; i++)
		{
			GoodsSlot value;
			GoodsSlot goodsSlot = (StashSlots.TryGetValue(i, out value) ? value : null);
			if (_lastSeenStashSlotRefs[i] != goodsSlot)
			{
				GoodsSlot goodsSlot2 = _slotModels[i].GoodsSlot;
				GoodsSlot goodsSlot3 = ((goodsSlot != null) ? goodsSlot.Clone() : new GoodsSlot { SlotIndex = i });
				if (!flag && CheckFilter(goodsSlot2) != CheckFilter(goodsSlot3))
				{
					flag = true;
				}
				_slotModels[i].GoodsSlot = goodsSlot3;
				_lastSeenStashSlotRefs[i] = goodsSlot;
			}
		}
		if (flag)
		{
			RefreshScroller(base.Scroller.ScrollPosition);
		}
	}

	private void SnapshotStashSlotRefs()
	{
		if (_lastSeenStashSlotRefs.Length != _slotModels.Count)
		{
			_lastSeenStashSlotRefs = new GoodsSlot[_slotModels.Count];
		}
		for (int i = 0; i < _slotModels.Count; i++)
		{
			GoodsSlot value;
			_lastSeenStashSlotRefs[i] = (StashSlots.TryGetValue(i, out value) ? value : null);
		}
	}

	private void OnMovePage(int index)
	{
		if (base.Scroller == null || !base.Scroller.gameObject.activeInHierarchy)
		{
			return;
		}
		index = Mathf.Clamp(index, 0, _model.RxPageCount.Value - 1);
		base.Scroller.JumpToDataIndex(index * 5);
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(base.CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
