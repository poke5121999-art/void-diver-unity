using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class QuickSlotSettingPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private QuickSlotSettingPanelModel _model;

	[View]
	private QuickSlotSettingPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private readonly List<InventoryGoodsSlotModel> _goodsSlotModels = new List<InventoryGoodsSlotModel>();

	protected override BasePanelModel PanelModel => _model;

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	protected void OnInject(QuickSlotSettingPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitSlots();
		InitGamePadNav();
		PlayerInventory myInventory = _gameManager.MyInventory;
		if (myInventory != null)
		{
			myInventory.OnInventoryChanged.Subscribe(OnInventoryChanged).AddTo(_disposable);
			myInventory.OnSafeInventoryChanged.Subscribe(OnSafeInventoryChanged).AddTo(_disposable);
		}
		(_playerManager.QuickSlotItemIds as IReadOnlyReactiveCollection<int>).ObserveReplace().Subscribe(delegate(CollectionReplaceEvent<int> _)
		{
			RefreshSlot(_.Index);
		}).AddTo(_disposable);
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
	}

	private void InitGamePadNav()
	{
		ClearNav();
		// <>c.<InitGamePadNav>b__12_0 @0x180922040: GameObject.GetComponent<InventoryGoodsSlotView> (method spec), then field +0x28 (slot); null object or component goes to the NullReferenceException throw, so no null-conditionals
		SetNavHorizontal(_view.GoodsSlots.Select((GameObject _) => _.GetComponent<InventoryGoodsSlotView>().Slot).ToList());
	}

	private void InitSlots()
	{
		_goodsSlotModels.Clear();
		for (int i = 0; i < _view.GoodsSlots.Length; i++)
		{
			GoodsSlot slot = CreateGoodsSlot(i);
			InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(EGoodsSlotCategory.QuickSlot, slot, InventoryGoodsSlotPresenter.ELayout.Normal);
			inventoryGoodsSlotModel.OnExit.Subscribe(OnSlotExit).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEnter.Subscribe(OnSlotEnter).AddTo(_disposable);
			inventoryGoodsSlotModel.OnLeftClick.Subscribe(OnSlotLeftClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnRightClick.Subscribe(OnSlotRightClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnDrag.Subscribe(_model.OnSlotDrag).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(_model.OnSlotEndDrag).AddTo(_disposable);
			_view.GoodsSlots[i].GetComponentInChildren<InventoryGoodsSlotPresenter>().Inject(inventoryGoodsSlotModel);
			_goodsSlotModels.Add(inventoryGoodsSlotModel);
		}
	}

	private GoodsSlot CreateGoodsSlot(int index)
	{
		Goods goods = null;
		if (_playerManager.TryGetQuickSlotItemId(index, out var itemId))
		{
			long itemCount = _gameManager.MyInventory.GetItemCount(itemId);
			goods = GoodsCreator.MakeItemGoods(itemId, itemCount);
		}
		return new GoodsSlot
		{
			SlotIndex = index,
			Goods = goods
		};
	}

	private void RefreshSlot(int index)
	{
		_goodsSlotModels[index].GoodsSlot = CreateGoodsSlot(index);
	}

	private void RefreshSlots()
	{
		for (int i = 0; i < _goodsSlotModels.Count; i++)
		{
			_goodsSlotModels[i].GoodsSlot = CreateGoodsSlot(i);
		}
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		RefreshSlots();
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		RefreshSlots();
	}

	private void OnQuickSlotChanged(int index)
	{
		RefreshSlot(index);
	}

	private void OnSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedSlotModel.SetValueAndForceNotify(slotModel);
		if (slotModel.GoodsSlot.Id < 1)
		{
			_view.GoodsTooltip.gameObject.SetActive(value: false);
			return;
		}
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		_view.GoodsTooltip.GetComponent<GoodsTooltipPresenter>().Inject(new GoodsTooltipModel
		{
			Goods = slotModel.GoodsSlot.Goods
		});
	}

	private void OnSlotExit(InventoryGoodsSlotModel slotModel)
	{
		if (_model.RxFocusedSlotModel.Value != null && _model.RxFocusedSlotModel.Value == slotModel)
		{
			_model.RxFocusedSlotModel.Value = null;
			_view.GoodsTooltip.gameObject.SetActive(value: false);
		}
	}

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		_model.OnSlotLeftClick.OnNext(slotModel);
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		_model.OnSlotRightClick.OnNext(slotModel);
	}
}
