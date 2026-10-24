using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using Tracker;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SafeInventoryPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private SafeInventoryPanelModel _model;

	[View]
	private SafeInventoryPanelView _view;

	[Inject]
	private GameManager _gameManager;

	private readonly List<InventoryGoodsSlotModel> _safeSlotModels = new List<InventoryGoodsSlotModel>();

	private CompositeDisposable _disposable;

	private GamePlayer Player => _gameManager.MyPlayer;

	private PlayerInventory Inventory => _model.PlayerInventory;

	private int SafeSlotCounts => TConst.SafeInventorySlotBaseCount.IntValue + _gameManager.MyPlayer.GetTalentEffectValue(ETalentEffectType.SafeInventorySlotCount);

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable.Dispose();
	}

	private void OnInject(SafeInventoryPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitSafeSlots();
		InitGamePadNav();
		if (!model.HideGoods)
		{
			_model.PlayerInventory.OnSafeInventoryChanged.Subscribe(OnSafeInventoryChanged).AddTo(_disposable);
		}
		_gameManager.MyPlayer.RxTalent.Skip(1).Subscribe(OnTalentUpdated).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		Logger.Debug(new Color(1f, 0.5490196f, 0f, 1f));
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Button> items = _view.SafeSlots.Select((GameObject _) => _.GetComponent<InventoryGoodsSlotView>().Slot).Take(_gameManager.MyPlayer.SafeSlotCount).ToList();
		if (items.Count > 0)
		{
			SetNavHorizontal(items);
		}
	}

	private void InitSafeSlots()
	{
		_safeSlotModels.Clear();
		for (int i = 0; i < _view.SafeSlots.Length; i++)
		{
			InventoryGoodsSlotModel inventoryGoodsSlotModel = CreateSlotModel(i);
			inventoryGoodsSlotModel.UseTooltip = false;
			inventoryGoodsSlotModel.ShowItemCount = true;
			inventoryGoodsSlotModel.OnClick.Subscribe(_model.OnSafeSlotClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnLeftClick.Subscribe(_model.OnSafeSlotLeftClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnRightClick.Subscribe(_model.OnSafeSlotRightClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEnter.Subscribe(OnSafeSlotEnter).AddTo(_disposable);
			inventoryGoodsSlotModel.OnExit.Subscribe(OnSafeSlotExit).AddTo(_disposable);
			inventoryGoodsSlotModel.OnDrag.Subscribe(_model.OnSafeSlotDrag).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(_model.OnSafeSlotEndDrag).AddTo(_disposable);
			_view.SafeSlots[i].GetComponent<InventoryGoodsSlotPresenter>().Inject(inventoryGoodsSlotModel);
			_safeSlotModels.Add(inventoryGoodsSlotModel);
		}
		InventoryGoodsSlotModel CreateSlotModel(int index)
		{
			if (index < _gameManager.MyPlayer.SafeSlotCount)
			{
				if (!_model.HideGoods && _model.PlayerInventory.SafeSlots.TryGetValue(index, out var value) && CheckFilter(value))
				{
					return new InventoryGoodsSlotModel(EGoodsSlotCategory.SafeSlot, value);
				}
				GoodsSlot slot = new GoodsSlot
				{
					SlotIndex = index
				};
				return new InventoryGoodsSlotModel(EGoodsSlotCategory.SafeSlot, slot);
			}
			GoodsSlot slot2 = new GoodsSlot
			{
				SlotIndex = index
			};
			return new InventoryGoodsSlotModel(EGoodsSlotCategory.SafeSlot, slot2, InventoryGoodsSlotPresenter.ELayout.Locked);
		}
	}

	private void OnSafeSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedSafeSlotModel.Value = slotModel;
	}

	private void OnSafeSlotExit(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedSafeSlotModel.Value = null;
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		foreach (GoodsSlotChangeData changeData in changeDatas)
		{
			InventoryGoodsSlotModel inventoryGoodsSlotModel = _safeSlotModels[changeData.Key];
			switch (changeData.Operation)
			{
			case EDataOperation.Add:
			case EDataOperation.Replace:
				inventoryGoodsSlotModel.GoodsSlot = changeData.NewSlot;
				break;
			case EDataOperation.Remove:
				inventoryGoodsSlotModel.GoodsSlot = new GoodsSlot
				{
					SlotIndex = changeData.Key
				};
				break;
			}
		}
	}

	private void OnTalentUpdated(Talent talent)
	{
		if (_model.PlayerInventory == null)
		{
			return;
		}
		for (int i = 0; i < _gameManager.MyPlayer.SafeSlotCount; i++)
		{
			if (_safeSlotModels[i].Layout == InventoryGoodsSlotPresenter.ELayout.Locked)
			{
				_safeSlotModels[i].RxLayout.Value = InventoryGoodsSlotPresenter.ELayout.Normal;
			}
		}
		InitGamePadNav();
	}

	private bool CheckFilter(GoodsSlot goodsSlot)
	{
		return goodsSlot.Goods.CheckFilter(_model.AllowedSubTypes, _model.ArtifactFilterTypes, _model.KeyItemFilterTypes);
	}
}
