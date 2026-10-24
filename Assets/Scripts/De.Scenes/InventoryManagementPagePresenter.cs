using System;
using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using GameCommon.Table;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class InventoryManagementPagePresenter : BasePanelPresenter
{
	public enum ELayout
	{
		None = 0,
		InventoryOnly = 1,
		InventoryLooting = 2,
		InventoryStash = 3
	}

	[InjectModel]
	private InventoryManagementPageModel _model;

	[View]
	private InventoryManagementPageView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	private PlayerInventoryPanelModel _myInventoryPanelModel;

	private LootingInventoryPanelModel _lootingInventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private QuickSlotSettingPanelModel _quickSlotPanelModel;

	private BagPanelModel _bagPanelModel;

	private EGoodsSlotCategory _bagHolderCategory;

	private int _bagHolderSlotIndex;

	private ulong _bagHolderNetworkObjectId;

	private bool _isDragging;

	private bool _isPadDragging;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private InventoryGoodsSlotModel _targetSlotModel;

	private DraggingGoodsSlotModel _draggingSlotModel;

	private CompositeDisposable _disposable = new CompositeDisposable();

	private CompositeDisposable _bagHolderSubscription;

	private EGoodsType[] _cachedStashFilter;

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiActions UiActions => InputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => InputManager.UiKeyboardActions;

	private InputActions.UiPadActions UiPadActions => InputManager.UiPadActions;

	private PlayerInventory PlayerInventory => _gameManager.MyInventory;

	private LootingInventory LootingInventory => _model.LootingInventory;

	private GamePlayer Player => _gameManager.MyPlayer;

	private int SourceSlotIndex => _selectedSlotModel?.GoodsSlot?.SlotIndex ?? (-1);

	private int TargetSlotIndex => _targetSlotModel?.GoodsSlot?.SlotIndex ?? (-1);

	private bool IsInventorySelectedOnePressed => InputManager.UiActions.InventorySelectOne.IsPressed();

	private bool IsInventorySelectHalfPressed => InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed();

	private bool IsBagPanelOpen
	{
		get
		{
			if (_view != null && _view.BagPanel != null)
			{
				return _view.BagPanel.gameObject.activeSelf;
			}
			return false;
		}
	}

	private bool CanPushDraggingGoodsToOpenedBag(GoodsSlot sourceSlot)
	{
		if (sourceSlot == null || sourceSlot.IsEmpty)
		{
			return false;
		}
		BagGoods bag = _bagPanelModel.Bag;
		if (bag == null)
		{
			return false;
		}
		if (sourceSlot.Category == EGoodsCategory.Bag)
		{
			return false;
		}
		TBag tBag = TBag.Table[bag.Id];
		if (tBag == null)
		{
			return false;
		}
		return tBag.Type == sourceSlot.Goods.PushableBagType;
	}

	private bool CheckEquipable(GoodsSlot slot, EGoodsSlotCategory targetCategory, out string failMessage)
	{
		failMessage = null;
		TLocalizedText failText;
		if (targetCategory == slot.Goods.Type.GetSlotCategory())
		{
			if (targetCategory == EGoodsSlotCategory.WeaponSlot && TCharacter.Table[_playerManager.SelectedCharacterId].WeaponType != TEquipment.Table[slot.Id].WeaponType)
			{
				failText = TLocalizedText.NotEquipableWeaponType;
			}
			else
			{
				int equipPartGroup = TEquipment.Table[slot.Id].EquipPartGroup;
				Character character;
				if (equipPartGroup > 0 && (character = _gameManager.MyPlayer.Character) != null)
				{
					int targetSlotIndex = TargetSlotIndex;
					if (!CheckSlots(character.WeaponSlots, (targetCategory == EGoodsSlotCategory.WeaponSlot) ? targetSlotIndex : (-1)) && !CheckSlots(character.AccessorySlots, (targetCategory == EGoodsSlotCategory.AccessorySlot) ? targetSlotIndex : (-1)) && !CheckSlots(character.ArtifactSlots, (targetCategory == EGoodsSlotCategory.ArtifactSlot) ? targetSlotIndex : (-1)))
					{
						failMessage = string.Empty;
						return true;
					}
					failText = TLocalizedText.DuplicateEquipPartGroup;
					failMessage = failText.LocalText;
					return false;
					bool CheckSlots(GoodsSlot[] slots, int skipSlotIndex)
					{
						if (slots != null)
						{
							for (int i = 0; i < slots.Length; i++)
							{
								GoodsSlot goodsSlot = slots[i];
								if (goodsSlot.SlotIndex != skipSlotIndex && goodsSlot.Goods != null && TEquipment.Table[goodsSlot.Goods.Id].EquipPartGroup == equipPartGroup)
								{
									return true;
								}
							}
						}
						return false;
					}
				}
				failMessage = string.Empty;
				return true;
			}
		}
		else
		{
			failText = TLocalizedText.NotEquipableEquipmentType;
		}
		failMessage = failText.LocalText;
		return false;
	}

	private bool HasDuplicateEquipPartGroup(Character character, int equipPartGroup, EGoodsSlotCategory replaceCategory, int replaceSlotIndex)
	{
		return CheckSlots(character.WeaponSlots, (replaceCategory == EGoodsSlotCategory.WeaponSlot) ? replaceSlotIndex : (-1)) || CheckSlots(character.AccessorySlots, (replaceCategory == EGoodsSlotCategory.AccessorySlot) ? replaceSlotIndex : (-1)) || CheckSlots(character.ArtifactSlots, (replaceCategory == EGoodsSlotCategory.ArtifactSlot) ? replaceSlotIndex : (-1));
		bool CheckSlots(GoodsSlot[] slots, int skipSlotIndex)
		{
			if (slots != null)
			{
				for (int i = 0; i < slots.Length; i++)
				{
					GoodsSlot goodsSlot = slots[i];
					if (goodsSlot.SlotIndex != skipSlotIndex && goodsSlot.Goods != null && TEquipment.Table[goodsSlot.Goods.Id].EquipPartGroup == equipPartGroup)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	private IDictionary<int, GoodsSlot> GetBagHolderSlotsForRefresh()
	{
		switch (_bagHolderCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			return _gameManager.MyInventory.Slots;
		case EGoodsSlotCategory.SafeSlot:
			return _gameManager.MyInventory.SafeSlots;
		case EGoodsSlotCategory.LootingSlot:
			return _model.LootingInventory?.Slots;
		case EGoodsSlotCategory.StashSlot:
			return _playerManager.StashSlots;
		default:
			return null;
		}
	}

	private long GetExistingStackInBag(BagGoods bag, int goodsId)
	{
		if (bag == null || bag.GoodsSlots == null)
		{
			return 0L;
		}
		long num = 0L;
		GoodsSlot[] goodsSlots = bag.GoodsSlots;
		foreach (GoodsSlot goodsSlot in goodsSlots)
		{
			if (goodsSlot != null && goodsSlot.Goods != null && goodsSlot.Goods.Id == goodsId)
			{
				num += goodsSlot.Goods.Count;
			}
		}
		return num;
	}


	private long? ResolveAddableForTarget(EGoodsSlotCategory targetCategory, int itemId)
	{
		int inventoryItemCountMax = _gameManager.MyPlayer.GetInventoryItemCountMax(itemId);
		IDictionary<int, GoodsSlot> slots;
		switch (targetCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			slots = _gameManager.MyInventory.Slots;
			break;
		case EGoodsSlotCategory.SafeSlot:
			slots = _gameManager.MyInventory.SafeSlots;
			break;
		case EGoodsSlotCategory.LootingSlot:
			if (_model.LootingInventory == null)
			{
				return null;
			}
			slots = _model.LootingInventory.Slots;
			break;
		default:
			return null;
		}
		GoodsSlot goodsSlot = slots.FindItemSlot(itemId);
		long num = ((goodsSlot == null) ? 0 : goodsSlot.Count);
		return Math.Max(inventoryItemCountMax - num, 0L);
	}

	private void Close()
	{
		_model.OnEscape.OnNext(Unit.Default);
	}

	public void CloseBagPanel()
	{
		if (_bagHolderSubscription != null)
		{
			_bagHolderSubscription.Dispose();
		}
		_bagHolderSubscription = null;
		if (_view == null || _view.BagPanel == null)
		{
			return;
		}
		bool isInputEnabled = _bagPanelModel.IsInputEnabled;
		_bagPanelModel.RxIsInputEnabled.Value = false;
		_view.BagPanel.Close();
		_view.QuickSlotSettingPanel.gameObject.SetActive(value: true);
		if (!isInputEnabled)
		{
			return;
		}
		RestoreFocusAfterBagClose();
	}

	private void DisableAllPanelInput()
	{
		_myInventoryPanelModel.RxIsInputEnabled.Value = false;
		if (_lootingInventoryPanelModel != null)
		{
			_lootingInventoryPanelModel.RxIsInputEnabled.Value = false;
		}
		if (_stashPanelModel != null)
		{
			_stashPanelModel.RxIsInputEnabled.Value = false;
		}
		_quickSlotPanelModel.RxIsInputEnabled.Value = false;
		if (_bagPanelModel != null)
		{
			_bagPanelModel.RxIsInputEnabled.Value = false;
		}
	}

	private void DropInventoryGoods(long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty)
		{
			_gameChannel.SendToHost(new ReqDropInventorySlot
			{
				InventorySlotIndex = goodsSlot.SlotIndex,
				Count = Math.Min(count, goodsSlot.Count)
			});
			_soundManager.PlaySfx("ItemDrop");
		}
	}

	private void DropSafeGoods(long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty)
		{
			_gameChannel.SendToHost(new ReqDropSafeSlot
			{
				SafeSlotIndex = goodsSlot.SlotIndex,
				Count = Math.Min(count, goodsSlot.Count)
			});
			_soundManager.PlaySfx("ItemDrop");
		}
	}

	private void EquipInventorySlot()
	{
		string failMessage = null;
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		EGoodsSlotCategory eGoodsSlotCategory = ((_targetSlotModel == null) ? goodsSlot.Type.GetSlotCategory() : _targetSlotModel.SlotCategory);
		if (CheckEquipable(goodsSlot, eGoodsSlotCategory, out failMessage))
		{
			_gameChannel.SendToHost(new ReqEquipInventorySlot
			{
				EquipmentSlotCategory = eGoodsSlotCategory,
				EquipmentSlotIndex = TargetSlotIndex,
				InventorySlotIndex = goodsSlot.SlotIndex
			});
			PlayEquipSound(eGoodsSlotCategory);
		}
		else
		{
			_toastManager.NoticeFail(failMessage);
		}
	}

	private void EquipLootingSlot()
	{
		string failMessage = null;
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		EGoodsSlotCategory eGoodsSlotCategory = ((_targetSlotModel == null) ? goodsSlot.Type.GetSlotCategory() : _targetSlotModel.SlotCategory);
		if (CheckEquipable(goodsSlot, eGoodsSlotCategory, out failMessage))
		{
			_gameChannel.SendToHost(new ReqEquipLootingSlot
			{
				EquipmentSlotCategory = eGoodsSlotCategory,
				EquipmentSlotIndex = TargetSlotIndex,
				LootingNetworkObjectId = _model.LootingInventory.NetworkObjectId,
				LootingSlotIndex = goodsSlot.SlotIndex
			});
			PlayEquipSound(eGoodsSlotCategory);
		}
		else
		{
			_toastManager.NoticeFail(failMessage);
		}
	}

	private void EquipSafeSlot()
	{
		string failMessage = null;
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		EGoodsSlotCategory eGoodsSlotCategory = ((_targetSlotModel == null) ? goodsSlot.Type.GetSlotCategory() : _targetSlotModel.SlotCategory);
		if (CheckEquipable(goodsSlot, eGoodsSlotCategory, out failMessage))
		{
			_gameChannel.SendToHost(new ReqEquipSafeSlot
			{
				EquipmentSlotCategory = eGoodsSlotCategory,
				EquipmentSlotIndex = TargetSlotIndex,
				SafeSlotIndex = goodsSlot.SlotIndex
			});
			PlayEquipSound(eGoodsSlotCategory);
		}
		else
		{
			_toastManager.NoticeFail(failMessage);
		}
	}

	private void EquipStashSlot()
	{
		string failMessage = null;
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		EGoodsSlotCategory eGoodsSlotCategory = ((_targetSlotModel == null) ? goodsSlot.Type.GetSlotCategory() : _targetSlotModel.SlotCategory);
		if (CheckEquipable(goodsSlot, eGoodsSlotCategory, out failMessage))
		{
			_gameChannel.SendToHost(new ReqEquipStashSlot
			{
				EquipmentSlotCategory = eGoodsSlotCategory,
				EquipmentSlotIndex = TargetSlotIndex,
				StashSlotIndex = goodsSlot.SlotIndex,
				EquipmentGoods = goodsSlot.Goods.Clone()
			});
			PlayEquipSound(eGoodsSlotCategory);
		}
		else
		{
			_toastManager.NoticeFail(failMessage);
		}
	}


	private void FocusBag(ENavDir dir)
	{
		DisableAllPanelInput();
		_bagPanelModel.EntryDirection = dir;
		_bagPanelModel.RxIsInputEnabled.Value = true;
	}

	private void FocusLooting(ENavDir dir)
	{
		DisableAllPanelInput();
		_lootingInventoryPanelModel.EntryDirection = dir;
		_lootingInventoryPanelModel.RxIsInputEnabled.Value = true;
	}

	private void FocusMyInventory(ENavDir dir, PlayerInventoryPanelPresenter.EFocusPanel focusPanel)
	{
		DisableAllPanelInput();
		_myInventoryPanelModel.EntryDirection = dir;
		_myInventoryPanelModel.FocusPanel = focusPanel;
		_myInventoryPanelModel.RxIsInputEnabled.Value = true;
	}

	private void FocusQuickSlot(ENavDir dir)
	{
		DisableAllPanelInput();
		_quickSlotPanelModel.EntryDirection = dir;
		_quickSlotPanelModel.RxIsInputEnabled.Value = true;
	}

	private void FocusStash(ENavDir dir, StashPanelPresenter.EFocusPanel focusPanel)
	{
		DisableAllPanelInput();
		_stashPanelModel.EntryDirection = dir;
		_stashPanelModel.FocusPanel = focusPanel;
		_stashPanelModel.RxIsInputEnabled.Value = true;
	}

	private void HandleBagSlotToAnyTarget()
	{
		long count;
		if (InputManager.UiActions.InventorySelectOne.IsPressed())
		{
			count = 1L;
		}
		else if (InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed())
		{
			count = (_selectedSlotModel.GoodsSlot.Count + 1) / 2;
		}
		else
		{
			count = long.MaxValue;
		}
		TransferBagSlotToAnyTarget(_selectedSlotModel, _targetSlotModel, count);
	}

	private void HandleEquipSlot()
	{
		switch (_selectedSlotModel.SlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			EquipInventorySlot();
			break;
		case EGoodsSlotCategory.SafeSlot:
			EquipSafeSlot();
			break;
		case EGoodsSlotCategory.LootingSlot:
			EquipLootingSlot();
			break;
		case EGoodsSlotCategory.StashSlot:
			EquipStashSlot();
			break;
		}
	}

	private void HandleInteraction()
	{
		if (_selectedSlotModel == null)
		{
			return;
		}
		EGoodsSlotCategory slotCategory = _selectedSlotModel.SlotCategory;
		EGoodsSlotCategory eGoodsSlotCategory = ((_targetSlotModel == null) ? EGoodsSlotCategory.None : _targetSlotModel.SlotCategory);
		if (_selectedSlotModel == _targetSlotModel)
		{
			_targetSlotModel = null;
			return;
		}
		if (slotCategory.IsInventoryCategory())
		{
			if (eGoodsSlotCategory.IsInventoryCategory())
			{
				EGoodsSlotCategory slotCategory2 = _targetSlotModel.SlotCategory;
				int slotIndex = _targetSlotModel.SlotIndex;
				long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((_selectedSlotModel.GoodsSlot.Count + 1) / 2)));
				switch (_selectedSlotModel.SlotCategory)
				{
				case EGoodsSlotCategory.InventorySlot:
					switch (slotCategory2)
					{
					case EGoodsSlotCategory.InventorySlot:
						SwapInventorySlots();
						break;
					case EGoodsSlotCategory.SafeSlot:
						TransferInventorySlotToSafeSlot();
						break;
					case EGoodsSlotCategory.LootingSlot:
						TransferInventorySlotToLootingSlot(slotIndex, num);
						break;
					case EGoodsSlotCategory.StashSlot:
						TransferInventorySlotToStashSlot(slotIndex, num);
						break;
					}
					break;
				case EGoodsSlotCategory.SafeSlot:
					switch (slotCategory2)
					{
					case EGoodsSlotCategory.SafeSlot:
						SwapSafeSlots();
						break;
					case EGoodsSlotCategory.InventorySlot:
						TransferSafeSlotToInventorySlot();
						break;
					case EGoodsSlotCategory.LootingSlot:
						TransferSafeSlotToLootingSlot(slotIndex, num);
						break;
					case EGoodsSlotCategory.StashSlot:
						TransferSafeSlotToStashSlot(slotIndex, num);
						break;
					}
					break;
				case EGoodsSlotCategory.LootingSlot:
					switch (slotCategory2)
					{
					case EGoodsSlotCategory.LootingSlot:
						SwapLootingSlots();
						break;
					case EGoodsSlotCategory.InventorySlot:
						TransferLootingSlotToInventorySlot(slotIndex, num);
						break;
					case EGoodsSlotCategory.SafeSlot:
						TransferLootingSlotToSafeSlot(slotIndex, num);
						break;
					}
					break;
				case EGoodsSlotCategory.StashSlot:
					switch (slotCategory2)
					{
					case EGoodsSlotCategory.StashSlot:
						SwapStashSlots();
						break;
					case EGoodsSlotCategory.InventorySlot:
						TransferStashSlotToInventorySlot(slotIndex, num);
						break;
					case EGoodsSlotCategory.SafeSlot:
						TransferStashSlotToSafeSlot(slotIndex, num);
						break;
					}
					break;
				}
			}
			else if (eGoodsSlotCategory.IsEquipmentCategory())
			{
				HandleEquipSlot();
			}
			else if (eGoodsSlotCategory.IsQuickSlotCategory())
			{
				SetQuickSlot(_targetSlotModel?.GoodsSlot?.SlotIndex ?? (-1));
			}
			else if (eGoodsSlotCategory == EGoodsSlotCategory.BagSlot)
			{
				HandleTransferToBagSlot();
			}
		}
		else if (slotCategory.IsEquipmentCategory())
		{
			if (eGoodsSlotCategory.IsInventoryCategory())
			{
				switch (_targetSlotModel.SlotCategory)
				{
				case EGoodsSlotCategory.InventorySlot:
					UnequipToInventorySlot();
					break;
				case EGoodsSlotCategory.SafeSlot:
					UnequipToSafeSlot();
					break;
				case EGoodsSlotCategory.LootingSlot:
					UnequipToLootingSlot();
					break;
				case EGoodsSlotCategory.StashSlot:
					UnequipToStashSlot();
					break;
				}
			}
			else if (eGoodsSlotCategory.IsEquipmentCategory())
			{
				HandleSwapEquipmentSlots();
			}
			else if (eGoodsSlotCategory.IsQuickSlotCategory())
			{
				_toastManager.NoticeFail(TLocalizedText.CannotRegisterToQuickSlot.LocalText);
			}
			else if (eGoodsSlotCategory == EGoodsSlotCategory.BagSlot)
			{
				TransferEquipmentSlotToBagSlot(_targetSlotModel?.GoodsSlot?.SlotIndex ?? (-1));
			}
		}
		else if (slotCategory.IsQuickSlotCategory())
		{
			if (eGoodsSlotCategory.IsQuickSlotCategory())
			{
				SwapQuickSlot(_targetSlotModel?.GoodsSlot?.SlotIndex ?? (-1));
			}
			else if (eGoodsSlotCategory == EGoodsSlotCategory.None)
			{
				_playerManager.SetQuickSlot(SourceSlotIndex, 0);
			}
		}
		if (slotCategory == EGoodsSlotCategory.BagSlot && (eGoodsSlotCategory.IsInventoryCategory() || eGoodsSlotCategory.IsEquipmentCategory() || eGoodsSlotCategory == EGoodsSlotCategory.BagSlot))
		{
			HandleBagSlotToAnyTarget();
		}
		_selectedSlotModel = _targetSlotModel ?? _selectedSlotModel;
		_targetSlotModel = null;
	}

	private void HandleSwapEquipmentSlots()
	{
		if (_selectedSlotModel == null || _targetSlotModel == null)
		{
			return;
		}
		if (_selectedSlotModel.SlotCategory == _targetSlotModel.SlotCategory)
		{
			GoodsSlot goodsSlot = _selectedSlotModel.GoodsSlot;
			GoodsSlot goodsSlot2 = _targetSlotModel.GoodsSlot;
			if (goodsSlot.Type != EGoodsType.Weapon && !goodsSlot.IsEmpty && goodsSlot.SlotIndex != goodsSlot2.SlotIndex && goodsSlot2.SlotIndex >= 0)
			{
				_gameChannel.SendToHost(new ReqSwapEquipmentSlots
				{
					EquipmentSlotCategory = _selectedSlotModel.SlotCategory,
					SourceSlotIndex = goodsSlot.SlotIndex,
					TargetSlotIndex = goodsSlot2.SlotIndex
				});
			}
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.EquipementSlotTypeMismatch.LocalText);
		}
	}

	private void SetQuickSlot(int quickSlotIndex)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && goodsSlot.Category == EGoodsCategory.Item && TItem.Table[goodsSlot.Id].CanUseFromQuickSlot)
		{
			_playerManager.SetQuickSlot(quickSlotIndex, goodsSlot.Id);
			_soundManager.PlaySfx("ItemRelease");
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.CannotRegisterToQuickSlot.LocalText);
		}
	}

	private void SwapQuickSlot(int targetSlotIndex)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty && goodsSlot.SlotIndex != targetSlotIndex)
		{
			_playerManager.SwapQuickSlot(goodsSlot.SlotIndex, targetSlotIndex);
			_soundManager.PlaySfx("ItemRelease");
		}
	}


	private void HandleTransferSlots()
	{
		if (_targetSlotModel == null)
		{
			return;
		}
		EGoodsSlotCategory slotCategory = _targetSlotModel.SlotCategory;
		int slotIndex = _targetSlotModel.SlotIndex;
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((_selectedSlotModel.GoodsSlot.Count + 1) / 2)));
		switch (_selectedSlotModel.SlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			switch (slotCategory)
			{
			case EGoodsSlotCategory.InventorySlot:
				SwapInventorySlots();
				break;
			case EGoodsSlotCategory.SafeSlot:
				TransferInventorySlotToSafeSlot();
				break;
			case EGoodsSlotCategory.LootingSlot:
				TransferInventorySlotToLootingSlot(slotIndex, num);
				break;
			case EGoodsSlotCategory.StashSlot:
				TransferInventorySlotToStashSlot(slotIndex, num);
				break;
			}
			break;
		case EGoodsSlotCategory.SafeSlot:
			switch (slotCategory)
			{
			case EGoodsSlotCategory.SafeSlot:
				SwapSafeSlots();
				break;
			case EGoodsSlotCategory.InventorySlot:
				TransferSafeSlotToInventorySlot();
				break;
			case EGoodsSlotCategory.LootingSlot:
				TransferSafeSlotToLootingSlot(slotIndex, num);
				break;
			case EGoodsSlotCategory.StashSlot:
				TransferSafeSlotToStashSlot(slotIndex, num);
				break;
			}
			break;
		case EGoodsSlotCategory.LootingSlot:
			switch (slotCategory)
			{
			case EGoodsSlotCategory.LootingSlot:
				SwapLootingSlots();
				break;
			case EGoodsSlotCategory.InventorySlot:
				TransferLootingSlotToInventorySlot(slotIndex, num);
				break;
			case EGoodsSlotCategory.SafeSlot:
				TransferLootingSlotToSafeSlot(slotIndex, num);
				break;
			}
			break;
		case EGoodsSlotCategory.StashSlot:
			switch (slotCategory)
			{
			case EGoodsSlotCategory.StashSlot:
				SwapStashSlots();
				break;
			case EGoodsSlotCategory.InventorySlot:
				TransferStashSlotToInventorySlot(slotIndex, num);
				break;
			case EGoodsSlotCategory.SafeSlot:
				TransferStashSlotToSafeSlot(slotIndex, num);
				break;
			}
			break;
		}
	}

	private void HandleTransferToBagSlot()
	{
		long count = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((_selectedSlotModel.GoodsSlot.Count + 1) / 2)));
		int innerSlotIndex = _targetSlotModel?.GoodsSlot?.SlotIndex ?? (-1);
		if (_selectedSlotModel != null)
		{
			switch (_selectedSlotModel.SlotCategory)
			{
			case EGoodsSlotCategory.InventorySlot:
				TransferInventorySlotToBagSlot(count, innerSlotIndex);
				break;
			case EGoodsSlotCategory.SafeSlot:
				TransferSafeSlotToBagSlot(count, innerSlotIndex);
				break;
			case EGoodsSlotCategory.LootingSlot:
				TransferLootingSlotToBagSlot(count, innerSlotIndex);
				break;
			case EGoodsSlotCategory.StashSlot:
				TransferStashSlotToBagSlot(count, innerSlotIndex);
				break;
			}
		}
	}

	private void HandleUnequipSlot()
	{
		switch (_targetSlotModel.SlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			UnequipToInventorySlot();
			break;
		case EGoodsSlotCategory.SafeSlot:
			UnequipToSafeSlot();
			break;
		case EGoodsSlotCategory.LootingSlot:
			UnequipToLootingSlot();
			break;
		case EGoodsSlotCategory.StashSlot:
			UnequipToStashSlot();
			break;
		}
	}

	private void SwapInventorySlots()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty)
		{
			_gameChannel.SendToHost(new ReqSwapInventorySlots
			{
				SourceSlotIndex = SourceSlotIndex,
				TargetSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void SwapSafeSlots()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty)
		{
			_gameChannel.SendToHost(new ReqSwapSafeSlots
			{
				SourceSlotIndex = SourceSlotIndex,
				TargetSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void SwapLootingSlots()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty)
		{
			_gameChannel.SendToHost(new ReqSwapLootingSlots
			{
				LootingNetworkObjectId = _model.LootingInventory.NetworkObjectId,
				SourceSlotIndex = SourceSlotIndex,
				TargetSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void SwapStashSlots()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty)
		{
			_playerManager.StashSlots.SwapIndex(SourceSlotIndex, TargetSlotIndex);
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void TransferEquipmentSlotToBagSlot(int innerSlotIndex = -1)
	{
		if (_selectedSlotModel != null && _selectedSlotModel.SlotCategory.IsEquipmentCategory())
		{
			TransferToBagSlot(1L, innerSlotIndex);
		}
	}

	private void TransferInventorySlotToBagSlot(long count, int innerSlotIndex = -1)
	{
		if (_selectedSlotModel != null && _selectedSlotModel.SlotCategory == EGoodsSlotCategory.InventorySlot)
		{
			TransferToBagSlot(count, innerSlotIndex);
		}
	}

	private void TransferLootingSlotToBagSlot(long count, int innerSlotIndex = -1)
	{
		if (_selectedSlotModel != null && _selectedSlotModel.SlotCategory == EGoodsSlotCategory.LootingSlot)
		{
			TransferToBagSlot(count, innerSlotIndex);
		}
	}

	private void TransferSafeSlotToBagSlot(long count, int innerSlotIndex = -1)
	{
		if (_selectedSlotModel != null && _selectedSlotModel.SlotCategory == EGoodsSlotCategory.SafeSlot)
		{
			TransferToBagSlot(count, innerSlotIndex);
		}
	}


	private void InitBagPanel()
	{
		_bagPanelModel = new BagPanelModel();
		_bagPanelModel.OnBack.Subscribe(delegate
		{
			CloseBagPanel();
		}).AddTo(_disposable);
		_view.BagPanel.gameObject.SetActive(value: false);
		_view.QuickSlotSettingPanel.gameObject.SetActive(value: true);
	}

	private void InitGamePadNav()
	{
		bool isSafeInventoryUnlocked = _playerManager.CheckStateConditions(TFeatureUnlock.Table[EFeatureUnlock.SafeInventory].UnlockConditions);
		_myInventoryPanelModel.OutOfRight.Subscribe(OnMyInventoryOutOfRight).AddTo(_disposable);
		_myInventoryPanelModel.OutOfBottom.Subscribe(OnMyInventoryOutOfBottom).AddTo(_disposable);
		_quickSlotPanelModel.OutOfLeft.Subscribe(OnQuickSlotPanelModelOutOfLeft).AddTo(_disposable);
		_quickSlotPanelModel.OutOfTop.Subscribe(OnQuickSlotPanelModelOutOfTop).AddTo(_disposable);
		if (_lootingInventoryPanelModel != null)
		{
			_lootingInventoryPanelModel.OutOfLeft.Subscribe(OnLootingInventoryOutOfLeft).AddTo(_disposable);
		}
		if (_stashPanelModel != null)
		{
			_stashPanelModel.OutOfLeft.Subscribe(OnStashOutOfLeft).AddTo(_disposable);
		}
		if (_bagPanelModel != null)
		{
			_bagPanelModel.OutOfLeft.Subscribe(OnBagOutOfLeft).AddTo(_disposable);
			_bagPanelModel.OutOfRight.Subscribe(OnBagOutOfRight).AddTo(_disposable);
		}
		void OnBagOutOfLeft(Unit _)
		{
			FocusMyInventory(ENavDir.Left, PlayerInventoryPanelPresenter.EFocusPanel.Scroller);
		}
		void OnBagOutOfRight(Unit _)
		{
			switch (_model.Layout)
			{
			case ELayout.InventoryLooting:
				FocusLooting(ENavDir.Right);
				break;
			case ELayout.InventoryStash:
				FocusStash(ENavDir.Right, StashPanelPresenter.EFocusPanel.Scroller);
				break;
			}
		}
		void OnLootingInventoryOutOfLeft(Unit _)
		{
			if (IsBagPanelOpen)
			{
				FocusBag(ENavDir.Left);
			}
			else
			{
				FocusMyInventory(ENavDir.Left, PlayerInventoryPanelPresenter.EFocusPanel.Scroller);
			}
		}
		void OnMyInventoryOutOfBottom(Unit _)
		{
			if (!isSafeInventoryUnlocked)
			{
				FocusQuickSlot(ENavDir.Down);
			}
		}
		void OnMyInventoryOutOfRight(Unit _)
		{
			if (IsBagPanelOpen)
			{
				FocusBag(ENavDir.Right);
				return;
			}
			switch (_myInventoryPanelModel.FocusPanel)
			{
			case PlayerInventoryPanelPresenter.EFocusPanel.Equipment:
				if (_model.Layout == ELayout.InventoryStash)
				{
					FocusStash(ENavDir.Right, StashPanelPresenter.EFocusPanel.Filter);
				}
				break;
			case PlayerInventoryPanelPresenter.EFocusPanel.Scroller:
			case PlayerInventoryPanelPresenter.EFocusPanel.Empty:
				if (_model.Layout == ELayout.InventoryStash)
				{
					FocusStash(ENavDir.Right, StashPanelPresenter.EFocusPanel.Scroller);
				}
				else if (_model.Layout == ELayout.InventoryLooting)
				{
					FocusLooting(ENavDir.Right);
				}
				break;
			case PlayerInventoryPanelPresenter.EFocusPanel.Safe:
				FocusQuickSlot(ENavDir.Right);
				break;
			}
		}
		void OnQuickSlotPanelModelOutOfLeft(Unit _)
		{
			if (isSafeInventoryUnlocked)
			{
				FocusMyInventory(ENavDir.Left, PlayerInventoryPanelPresenter.EFocusPanel.Safe);
			}
		}
		void OnQuickSlotPanelModelOutOfTop(Unit _)
		{
			if (!isSafeInventoryUnlocked)
			{
				FocusMyInventory(ENavDir.Up, PlayerInventoryPanelPresenter.EFocusPanel.Scroller);
			}
		}
		void OnStashOutOfLeft(Unit _)
		{
			if (IsBagPanelOpen)
			{
				FocusBag(ENavDir.Left);
				return;
			}
			switch (_stashPanelModel.FocusPanel)
			{
			case StashPanelPresenter.EFocusPanel.Filter:
				FocusMyInventory(ENavDir.Left, PlayerInventoryPanelPresenter.EFocusPanel.Equipment);
				break;
			case StashPanelPresenter.EFocusPanel.Scroller:
			case StashPanelPresenter.EFocusPanel.Empty:
				FocusMyInventory(ENavDir.Left, PlayerInventoryPanelPresenter.EFocusPanel.Scroller);
				break;
			}
		}
	}

	private void InitLootingInventory()
	{
		_lootingInventoryPanelModel = new LootingInventoryPanelModel
		{
			LootingInventory = _model.LootingInventory
		};
		_lootingInventoryPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(_disposable);
		_lootingInventoryPanelModel.OnSlotLeftClick.Subscribe(OnLootingSlotLeftClick).AddTo(_disposable);
		_lootingInventoryPanelModel.OnSlotRightClick.Subscribe(OnLootingSlotRightClick).AddTo(_disposable);
		_lootingInventoryPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(_disposable);
		_lootingInventoryPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(_disposable);
		_view.LootingInventory.gameObject.SetActive(value: true);
		_view.LootingInventory.Inject(_lootingInventoryPanelModel);
	}

	private void InitMyInventoryPanel()
	{
		_myInventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory
		};
		_myInventoryPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(_disposable);
		_myInventoryPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(_disposable);
		_myInventoryPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(_disposable);
		_myInventoryPanelModel.OnSlotLeftClick.Subscribe(OnInventorySlotLeftClick).AddTo(_disposable);
		_myInventoryPanelModel.OnSlotRightClick.Subscribe(OnInventorySlotRightClick).AddTo(_disposable);
		_myInventoryPanelModel.RxFocusedSafeSlotModel.Subscribe(OnSlotFocused).AddTo(_disposable);
		_myInventoryPanelModel.OnSafeSlotDrag.Subscribe(OnSlotDrag).AddTo(_disposable);
		_myInventoryPanelModel.OnSafeSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(_disposable);
		_myInventoryPanelModel.OnSafeSlotLeftClick.Subscribe(OnSafeInventorySlotLeftClick).AddTo(_disposable);
		_myInventoryPanelModel.OnSafeSlotRightClick.Subscribe(OnSafeInventorySlotRightClick).AddTo(_disposable);
		_myInventoryPanelModel.RxFocusedEquipmentSlotModel.Subscribe(OnSlotFocused).AddTo(_disposable);
		_myInventoryPanelModel.OnEquipmentSlotDrag.Subscribe(OnSlotDrag).AddTo(_disposable);
		_myInventoryPanelModel.OnEquipmentSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(_disposable);
		_myInventoryPanelModel.OnEquipmentSlotLeftClick.Subscribe(OnEquipmentSlotLeftClick).AddTo(_disposable);
		_myInventoryPanelModel.OnEquipmentSlotRightClick.Subscribe(OnEquipmentSlotRightClick).AddTo(_disposable);
		_view.MyInventory.Inject(_myInventoryPanelModel);
	}

	private void InitQuickSlotPanel()
	{
		_quickSlotPanelModel = new QuickSlotSettingPanelModel();
		_quickSlotPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(_disposable);
		_quickSlotPanelModel.OnSlotLeftClick.Subscribe(OnQuickSlotSlotLeftClick).AddTo(_disposable);
		_quickSlotPanelModel.OnSlotRightClick.Subscribe(OnQuickSlotSlotRightClick).AddTo(_disposable);
		_quickSlotPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(_disposable);
		_quickSlotPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(_disposable);
		_view.QuickSlotSettingPanel.Inject(_quickSlotPanelModel);
	}

	private void InitStash()
	{
		_view.ItemStashPanel.gameObject.SetActive(value: true);
		_stashPanelModel = new StashPanelModel
		{
			EnableFilterButtons = true
		};
		_stashPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(_disposable);
		_stashPanelModel.OnSlotLeftClick.Subscribe(OnStashSlotLeftClick).AddTo(_disposable);
		_stashPanelModel.OnSlotRightClick.Subscribe(OnStashSlotRightClick).AddTo(_disposable);
		_stashPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(_disposable);
		_stashPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(_disposable);
		_stashPanelModel.OnFilterChanged.Subscribe(OnStashFilterChanged).AddTo(_disposable);
		_view.ItemStashPanel.Inject(_stashPanelModel);
	}


	private void MoveAllToStash()
	{
		int[] movableInventorySlotIndicesForStash = _playerManager.GetMovableInventorySlotIndicesForStash();
		if (movableInventorySlotIndicesForStash != null && movableInventorySlotIndicesForStash.Length != 0)
		{
			_gameChannel.SendToHost(new ReqMoveAllInventoryToStash
			{
				InventorySlotIndices = movableInventorySlotIndicesForStash
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		OnInputDisabled();
		if (_bagHolderSubscription != null)
		{
			_bagHolderSubscription.Dispose();
		}
		_bagHolderSubscription = null;
	}

	private void OnDisable()
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
		if (_bagHolderSubscription != null)
		{
			_bagHolderSubscription.Dispose();
		}
		_bagHolderSubscription = null;
	}

	private void OnDropGoods(InputAction.CallbackContext context)
	{
		if (_isDragging)
		{
			return;
		}
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!IsInventorySelectHalfPressed) ? long.MaxValue : ((goodsSlot.Count + 1) / 2)));
		switch (_selectedSlotModel.SlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			DropInventoryGoods(num);
			break;
		case EGoodsSlotCategory.SafeSlot:
			DropSafeGoods(num);
			break;
		case EGoodsSlotCategory.LootingSlot:
			_toastManager.NoticeFail(TLocalizedText.CannotDropInLootInventory.LocalText);
			break;
		case EGoodsSlotCategory.StashSlot:
			_toastManager.NoticeFail(TLocalizedText.CannotDropWarehouseItem.LocalText);
			break;
		}
	}

	private void OnEquipmentSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		if (_view != null && _view.BagPanel != null)
		{
			if (!_view.BagPanel.gameObject.activeSelf)
			{
				UnequipToInventorySlot();
			}
			else
			{
				TransferEquipmentSlotToBagSlot();
			}
		}
	}

	private void OnEquipmentSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		UnequipToInventorySlot();
	}

	private void OnEscapeAction(InputAction.CallbackContext context)
	{
		if (!_isDragging)
		{
			if (_view != null && _view.BagPanel != null)
			{
				if (!_view.BagPanel.gameObject.activeSelf)
				{
					Close();
				}
				else
				{
					CloseBagPanel();
				}
			}
			return;
		}
		_isDragging = false;
		_isPadDragging = false;
		_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
		_myInventoryPanelModel.RxHighlightCategory.Value = EGoodsSlotCategory.None;
		if (_targetSlotModel != null)
		{
			_selectedSlotModel = _targetSlotModel;
		}
	}

	private void OnInject(InventoryManagementPageModel model)
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
		_disposable = new CompositeDisposable();
		_selectedSlotModel = null;
		_targetSlotModel = null;
		_draggingSlotModel = null;
		_isDragging = false;
		_isPadDragging = false;
		_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
		InitMyInventoryPanel();
		InitQuickSlotPanel();
		InitBagPanel();
		_view.LootingInventory.gameObject.SetActive(value: false);
		_view.ItemStashPanel.gameObject.SetActive(value: false);
		_lootingInventoryPanelModel = null;
		_stashPanelModel = null;
		switch (_model.Layout)
		{
		case ELayout.InventoryLooting:
			InitLootingInventory();
			break;
		case ELayout.InventoryStash:
			InitStash();
			break;
		}
		InitGamePadNav();
		InputManager.RxControlDevice.Subscribe(delegate(EControlDevice device)
		{
			bool flag = device != EControlDevice.GamePad;
			_view.InsertHalf.SetActive(flag);
			_view.DropHalf.SetActive(flag);
			bool flag2 = _model.Layout == ELayout.InventoryStash;
			_view.MoveToStashAllKeyboard.SetActive(flag2 && flag);
			_view.MoveToStashAllPad.SetActive(flag2 && device == EControlDevice.GamePad);
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

	protected override void OnInputDisabled()
	{
		_isDragging = false;
		_isPadDragging = false;
		if (_view != null && _view.DraggingGoodsSlot != null)
		{
			_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
		}
		DisableAllPanelInput();
		base.OnInputDisabled();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		InputManager.UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(OnEscapeAction).AddTo(InputDisposable);
		InputManager.UiActions.Organize.OnClickWithSfx("ButtonClick").Subscribe(OnOrganizeButtonClick).AddTo(InputDisposable);
		InputManager.UiActions.UseItem.OnClickWithSfx("ButtonClick").Subscribe(OnUseButtonClick).AddTo(InputDisposable);
		InputManager.UiKeyboardActions.SetQuickSlot.OnClickWithSfx("ButtonClick").Subscribe(OnSetQuickSlotByInput).AddTo(InputDisposable);
		InputManager.UiKeyboardActions.SwapWeapon.OnClickWithSfx("ButtonClick").Subscribe(OnSwapWeaponButtonClick).AddTo(InputDisposable);
		InputManager.UiKeyboardActions.InventoryMoveToStashAll.OnClickWithSfx("ButtonClick").Subscribe(OnMoveToStashAll).AddTo(InputDisposable);
		InputManager.UiPadActions.InsertGoods.OnClickWithSfx("ButtonClick").Subscribe(OnInsertGoods).AddTo(InputDisposable);
		InputManager.UiPadActions.DropGoods.OnClickWithSfx("ButtonClick").Subscribe(OnDropGoods).AddTo(InputDisposable);
		InputManager.UiPadActions.DragAndDrop.OnClickWithSfx("ButtonClick").Subscribe(OnPadDragAndDrop).AddTo(InputDisposable);
		if (_model.Layout == ELayout.InventoryLooting)
		{
			FocusLooting(ENavDir.Right);
		}
		else
		{
			FocusMyInventory(ENavDir.Down, PlayerInventoryPanelPresenter.EFocusPanel.Scroller);
		}
	}


	private void OnInsertGoods(InputAction.CallbackContext context)
	{
		if (_isDragging)
		{
			return;
		}
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!IsInventorySelectHalfPressed) ? long.MaxValue : ((goodsSlot.Count + 1) / 2)));
		if (_view == null || _view.BagPanel == null)
		{
			return;
		}
		if (!_view.BagPanel.gameObject.activeSelf)
		{
			if (_selectedSlotModel == null)
			{
				return;
			}
			switch (_selectedSlotModel.SlotCategory)
			{
			case EGoodsSlotCategory.InventorySlot:
				if (_model.Layout == ELayout.InventoryLooting)
				{
					TransferInventorySlotToLootingSlot(-1, num);
				}
				else if (_model.Layout == ELayout.InventoryStash)
				{
					TransferInventorySlotToStashSlot(-1, num);
				}
				break;
			case EGoodsSlotCategory.SafeSlot:
				if (_model.Layout == ELayout.InventoryLooting)
				{
					TransferSafeSlotToLootingSlot(-1, num);
				}
				else if (_model.Layout == ELayout.InventoryStash)
				{
					TransferSafeSlotToStashSlot(-1, num);
				}
				break;
			case EGoodsSlotCategory.LootingSlot:
				if (goodsSlot.Category == EGoodsCategory.Item && _gameManager.MyInventory.SafeSlots.HasItem(goodsSlot.Id))
				{
					TransferLootingSlotToSafeSlot(-1, num);
				}
				else
				{
					TransferLootingSlotToInventorySlot(-1, num);
				}
				break;
			case EGoodsSlotCategory.StashSlot:
				if (goodsSlot.Category == EGoodsCategory.Item && _gameManager.MyInventory.SafeSlots.HasItem(goodsSlot.Id))
				{
					TransferStashSlotToSafeSlot(-1, num);
				}
				else
				{
					TransferStashSlotToInventorySlot(-1, num);
				}
				break;
			case EGoodsSlotCategory.WeaponSlot:
			case EGoodsSlotCategory.AccessorySlot:
			case EGoodsSlotCategory.ArtifactSlot:
				UnequipToInventorySlot();
				break;
			}
			return;
		}
		if (_selectedSlotModel == null)
		{
			return;
		}
		switch (_selectedSlotModel.SlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			TransferInventorySlotToBagSlot(num);
			break;
		case EGoodsSlotCategory.SafeSlot:
			TransferSafeSlotToBagSlot(num);
			break;
		case EGoodsSlotCategory.LootingSlot:
			TransferLootingSlotToBagSlot(num);
			break;
		case EGoodsSlotCategory.StashSlot:
			TransferStashSlotToBagSlot(num);
			break;
		case EGoodsSlotCategory.WeaponSlot:
		case EGoodsSlotCategory.AccessorySlot:
		case EGoodsSlotCategory.ArtifactSlot:
			TransferEquipmentSlotToBagSlot();
			break;
		case EGoodsSlotCategory.BagSlot:
			TransferBagSlotToInventory(_selectedSlotModel);
			break;
		}
	}

	private void OnInventorySlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((slotModel.GoodsSlot.Count + 1) / 2)));
		if (_view != null && _view.BagPanel != null && _view.BagPanel.gameObject != null)
		{
			if (_view.BagPanel.gameObject.activeSelf)
			{
				TransferInventorySlotToBagSlot(num);
				return;
			}
			if (_model.Layout == ELayout.InventoryLooting)
			{
				TransferInventorySlotToLootingSlot(-1, num);
			}
			else if (_model.Layout == ELayout.InventoryStash)
			{
				TransferInventorySlotToStashSlot(-1, num);
			}
		}
	}

	private void OnInventorySlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		if (!_isDragging)
		{
			long count = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((slotModel.GoodsSlot.Count + 1) / 2)));
			DropInventoryGoods(count);
		}
	}

	private void OnLootingSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((slotModel.GoodsSlot.Count + 1) / 2)));
		if (_view == null || _view.BagPanel == null || _view.BagPanel.gameObject == null)
		{
			return;
		}
		if (_view.BagPanel.gameObject.activeSelf)
		{
			TransferLootingSlotToBagSlot(num);
		}
		else if (slotModel != null && slotModel.GoodsSlot != null)
		{
			if (slotModel.GoodsSlot.Category == EGoodsCategory.Item && _gameManager.MyInventory.SafeSlots.HasItem(slotModel.GoodsSlot.Id))
			{
				TransferLootingSlotToSafeSlot(-1, num);
			}
			else
			{
				TransferLootingSlotToInventorySlot(-1, num);
			}
		}
	}

	private void OnLootingSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		_toastManager.NoticeFail(TLocalizedText.CannotDropInLootInventory.LocalText);
	}

	private void OnMoveToStashAll(InputAction.CallbackContext context)
	{
		if (!_isDragging && _model.Layout == ELayout.InventoryStash)
		{
			MoveAllToStash();
		}
	}

	private void OnOrganizeButtonClick(InputAction.CallbackContext context)
	{
		if (_isDragging)
		{
			return;
		}
		EGoodsSlotCategory eGoodsSlotCategory = ((_selectedSlotModel != null) ? _selectedSlotModel.SlotCategory : EGoodsSlotCategory.None);
		switch (eGoodsSlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			_gameChannel.SendToHost(new ReqOrganizeInventorySlots());
			break;
		case EGoodsSlotCategory.SafeSlot:
			_gameChannel.SendToHost(new ReqOrganizeInventorySafeSlots());
			break;
		case EGoodsSlotCategory.StashSlot:
			_playerManager.StashSlots.Organize();
			break;
		}
	}

	private void OnPadDragAndDrop(InputAction.CallbackContext context)
	{
		if (_isDragging)
		{
			OnSlotEndDrag(_selectedSlotModel);
		}
		else if (!InputManager.UiActions.InventorySelectOne.IsPressed())
		{
			if (_selectedSlotModel != null && !_selectedSlotModel.IsSlotEmpty)
			{
				_isPadDragging = true;
				OnSlotDrag(_selectedSlotModel);
			}
		}
		else if (!_isDragging && _model.Layout == ELayout.InventoryStash)
		{
			MoveAllToStash();
		}
	}

	private void OnQuickSlotSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnQuickSlotSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		_playerManager.SetQuickSlot(slotModel.SlotIndex, 0);
	}

	private void OnSafeInventorySlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((slotModel.GoodsSlot.Count + 1) / 2)));
		if (_view != null && _view.BagPanel != null && _view.BagPanel.gameObject != null)
		{
			if (_view.BagPanel.gameObject.activeSelf)
			{
				TransferSafeSlotToBagSlot(num);
				return;
			}
			if (_model.Layout == ELayout.InventoryLooting)
			{
				TransferSafeSlotToLootingSlot(-1, num);
			}
			else if (_model.Layout == ELayout.InventoryStash)
			{
				TransferSafeSlotToStashSlot(-1, num);
			}
		}
	}


	private void OnSafeInventorySlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		if (!_isDragging)
		{
			long count = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((slotModel.GoodsSlot.Count + 1) / 2)));
			DropSafeGoods(count);
		}
	}

	private void OnSetQuickSlotByInput(InputAction.CallbackContext context)
	{
		if (!_isDragging)
		{
			int quickSlotIndex = context.action.controls.IndexOf((InputControl _) => _ == context.control);
			SetQuickSlot(quickSlotIndex);
		}
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel.IsSlotEmpty)
		{
			return;
		}
		_selectedSlotModel = slotModel;
		_isDragging = true;
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? slotModel.GoodsSlot.Count : ((slotModel.GoodsSlot.Count + 1) / 2)));
		Goods goods = GoodsCreator.MakeGoods(slotModel.GoodsSlot.Type, slotModel.GoodsSlot.Id, num, 0L);
		_draggingSlotModel = new DraggingGoodsSlotModel(goods);
		_view.DraggingGoodsSlot.gameObject.SetActive(value: true);
		_view.DraggingGoodsSlot.Inject(_draggingSlotModel);
		if (slotModel.GoodsSlot.Category == EGoodsCategory.Equipment)
		{
			EGoodsSlotCategory slotCategory = slotModel.GoodsSlot.Type.GetSlotCategory();
			if (CheckEquipable(slotModel.GoodsSlot, slotCategory, out var _))
			{
				_myInventoryPanelModel.RxHighlightCategory.Value = slotCategory;
			}
		}
		if (_view.BagPanel.gameObject.activeSelf)
		{
			_bagPanelModel.RxIsDragAcceptable.Value = CanPushDraggingGoodsToOpenedBag(slotModel.GoodsSlot);
		}
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
		if (_isDragging)
		{
			_isDragging = false;
			_isPadDragging = false;
			_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
			_myInventoryPanelModel.RxHighlightCategory.Value = EGoodsSlotCategory.None;
			_bagPanelModel.RxIsDragAcceptable.Value = false;
			HandleInteraction();
		}
	}

	private void OnSlotFocused(InventoryGoodsSlotModel slotModel)
	{
		if (!_isDragging)
		{
			_selectedSlotModel = slotModel;
		}
		else
		{
			_targetSlotModel = slotModel;
		}
	}


	private void OnStashFilterChanged(EGoodsType[] allowedTypes)
	{
		_cachedStashFilter = allowedTypes;
	}

	private void OnStashSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? long.MaxValue : ((slotModel.GoodsSlot.Count + 1) / 2)));
		if (_view != null && _view.BagPanel != null && _view.BagPanel.gameObject != null)
		{
			if (_view.BagPanel.gameObject.activeSelf)
			{
				TransferStashSlotToBagSlot(num);
			}
			else if (slotModel != null && slotModel.GoodsSlot != null)
			{
				if (slotModel.GoodsSlot.Category == EGoodsCategory.Item && _gameManager.MyInventory.SafeSlots.HasItem(slotModel.GoodsSlot.Id))
				{
					TransferStashSlotToSafeSlot(-1, num);
				}
				else
				{
					TransferStashSlotToInventorySlot(-1, num);
				}
			}
		}
	}

	private void OnStashSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		_toastManager.NoticeFail(TLocalizedText.CannotDropWarehouseItem.LocalText);
	}

	private void OnSwapWeaponButtonClick(InputAction.CallbackContext context)
	{
		if (!_isDragging)
		{
			_gameChannel.SendToHost(new ReqSwapWeapon
			{
				AllowEmpty = true
			});
		}
	}

	private void OnUseButtonClick(InputAction.CallbackContext context)
	{
		if (_isDragging)
		{
			return;
		}
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		switch (_selectedSlotModel.SlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			if (goodsSlot.Category == EGoodsCategory.Item)
			{
				UseItem();
				return;
			}
			if (goodsSlot.Category == EGoodsCategory.Bag)
			{
				UseBag();
				return;
			}
			if (goodsSlot.Category == EGoodsCategory.Equipment)
			{
				EquipInventorySlot();
				return;
			}
			break;
		case EGoodsSlotCategory.SafeSlot:
			if (goodsSlot.Category == EGoodsCategory.Item)
			{
				UseItem();
				return;
			}
			if (goodsSlot.Category == EGoodsCategory.Bag)
			{
				UseBag();
				return;
			}
			if (goodsSlot.Category == EGoodsCategory.Equipment)
			{
				EquipSafeSlot();
				return;
			}
			break;
		case EGoodsSlotCategory.LootingSlot:
		case EGoodsSlotCategory.StashSlot:
			if (goodsSlot.Category == EGoodsCategory.Bag)
			{
				UseBag();
				return;
			}
			break;
		case EGoodsSlotCategory.WeaponSlot:
		case EGoodsSlotCategory.AccessorySlot:
		case EGoodsSlotCategory.ArtifactSlot:
			UnequipToInventorySlot();
			return;
		}
		_toastManager.NoticeFail(TLocalizedText.SelectItemInInventory.LocalText);
	}


	public void OpenBagPanel(BagGoods bag, EGoodsSlotCategory holderCategory, int holderSlotIndex, ulong holderNetworkObjectId)
	{
		if (bag == null)
		{
			return;
		}
		_bagHolderCategory = holderCategory;
		_bagHolderSlotIndex = holderSlotIndex;
		_bagHolderNetworkObjectId = holderNetworkObjectId;
		_view.BagPanel.Inject(_bagPanelModel);
		_bagPanelModel.Bag = bag;
		_view.BagPanel.Open();
		_view.QuickSlotSettingPanel.gameObject.SetActive(value: false);
		DisableAllPanelInput();
		_bagPanelModel.EntryDirection = ENavDir.Down;
		_bagPanelModel.RxIsInputEnabled.Value = true;
		SubscribeBagHolderChanges();
		_bagPanelModel.RxFocusedSlotModel.Skip(1).Subscribe(OnSlotFocused).AddTo(_bagHolderSubscription);
		_bagPanelModel.OnSlotLeftClick.Subscribe(TransferBagSlotToInventory).AddTo(_bagHolderSubscription);
		_bagPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(_bagHolderSubscription);
		_bagPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(_bagHolderSubscription);
	}

	private void PlayEquipSound(EGoodsSlotCategory subType)
	{
		string sfxName = "Equip";
		if (subType == EGoodsSlotCategory.ArtifactSlot)
		{
			sfxName = "Equip2";
		}
		_soundManager.PlaySfx(sfxName);
	}

	private void RefreshBagPanelFromHolder()
	{
		IDictionary<int, GoodsSlot> bagHolderSlotsForRefresh = GetBagHolderSlotsForRefresh();
		if (bagHolderSlotsForRefresh != null && bagHolderSlotsForRefresh.TryGetValue(_bagHolderSlotIndex, out var value) && value.Goods is BagGoods bag)
		{
			_bagPanelModel.Bag = bag;
		}
		else
		{
			CloseBagPanel();
		}
	}

	private void RestoreFocusAfterBagClose()
	{
		if (_bagHolderCategory != EGoodsSlotCategory.InventorySlot && _bagHolderCategory != EGoodsSlotCategory.SafeSlot)
		{
			if (_bagHolderCategory == EGoodsSlotCategory.LootingSlot)
			{
				FocusLooting(ENavDir.None);
				return;
			}
			if (_bagHolderCategory == EGoodsSlotCategory.StashSlot)
			{
				DisableAllPanelInput();
				_stashPanelModel.EntryDirection = ENavDir.None;
				_stashPanelModel.FocusPanel = StashPanelPresenter.EFocusPanel.Scroller;
				_stashPanelModel.RxIsInputEnabled.Value = true;
				return;
			}
		}
		FocusMyInventory(ENavDir.None, PlayerInventoryPanelPresenter.EFocusPanel.Scroller);
	}


	private void SubscribeBagHolderChanges()
	{
		if (_bagHolderSubscription != null)
		{
			_bagHolderSubscription.Dispose();
		}
		_bagHolderSubscription = new CompositeDisposable();
		switch (_bagHolderCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			_gameManager.MyInventory.OnInventoryChanged.Subscribe(delegate
			{
				RefreshBagPanelFromHolder();
			}).AddTo(_bagHolderSubscription);
			break;
		case EGoodsSlotCategory.SafeSlot:
			_gameManager.MyInventory.OnSafeInventoryChanged.Subscribe(delegate
			{
				RefreshBagPanelFromHolder();
			}).AddTo(_bagHolderSubscription);
			break;
		case EGoodsSlotCategory.LootingSlot:
			if (_model.LootingInventory != null)
			{
				_model.LootingInventory.OnInventoryChanged.Subscribe(delegate
				{
					RefreshBagPanelFromHolder();
				}).AddTo(_bagHolderSubscription);
			}
			break;
		case EGoodsSlotCategory.StashSlot:
			_playerManager.OnStashChanged.Subscribe(delegate
			{
				RefreshBagPanelFromHolder();
			}).AddTo(_bagHolderSubscription);
			break;
		}
	}

	private void UnsubscribeBagHolderChanges()
	{
		if (_bagHolderSubscription != null)
		{
			_bagHolderSubscription.Dispose();
		}
		_bagHolderSubscription = null;
	}


	private void Update()
	{
		if (!_isDragging)
		{
			return;
		}
		if (!_isPadDragging && Mouse.current != null && !Mouse.current.leftButton.isPressed)
		{
			OnSlotEndDrag(_selectedSlotModel);
			return;
		}
		long count = _draggingSlotModel.GoodsSlot.Count;
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? _selectedSlotModel.GoodsSlot.Count : ((_selectedSlotModel.GoodsSlot.Count + 1) / 2)));
		if (count != num)
		{
			Goods goods = _draggingSlotModel.GoodsSlot.Goods;
			Goods goods2 = GoodsCreator.MakeGoods(goods.Type, goods.Id, num, 0L);
			_draggingSlotModel.GoodsSlot = new GoodsSlot
			{
				Goods = goods2
			};
		}
		RectTransform component = _view.DraggingGoodsSlot.GetComponent<RectTransform>();
		RectTransform rectTransform = _popupManager.OverlayCanvas.transform as RectTransform;
		if (!_isPadDragging)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
			component.anchoredPosition = localPoint;
			return;
		}
		RectTransform rectTransform2 = (_targetSlotModel?.Transform ?? _selectedSlotModel?.Transform) as RectTransform;
		if (!(rectTransform2 == null))
		{
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(null, rectTransform2.position);
			Rect rect = rectTransform2.rect;
			Vector2 screenPoint = vector + new Vector2(rectTransform2.lossyScale.x * rect.width * 0.5f, (0f - rectTransform2.rect.height * rectTransform2.lossyScale.y) * 0.5f);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out var localPoint2);
			component.anchoredPosition = localPoint2;
		}
	}


	private void UseBag()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot != null && !goodsSlot.IsEmpty && goodsSlot.Goods is BagGoods bag)
		{
			ulong holderNetworkObjectId = 0uL;
			if (_selectedSlotModel.SlotCategory == EGoodsSlotCategory.LootingSlot && _model.LootingInventory != null)
			{
				holderNetworkObjectId = _model.LootingInventory.NetworkObjectId;
			}
			OpenBagPanel(bag, _selectedSlotModel.SlotCategory, goodsSlot.SlotIndex, holderNetworkObjectId);
		}
	}

	private void UseItem()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (_selectedSlotModel == null || (_selectedSlotModel.SlotCategory != EGoodsSlotCategory.InventorySlot && _selectedSlotModel.SlotCategory != EGoodsSlotCategory.SafeSlot) || goodsSlot == null || goodsSlot.Category != EGoodsCategory.Item)
		{
			_toastManager.NoticeFail(TLocalizedText.SelectItemInInventory.LocalText);
			return;
		}
		TItem tItem = TItem.Table[goodsSlot.Id];
		UnitModel component = _gameManager.MyCharacterModel.GetComponent<UnitModel>();
		TLocalizedText tLocalizedText;
		if (!tItem.CanUseFromInventory)
		{
			tLocalizedText = TLocalizedText.CannotUseInInventory;
		}
		else if (component.IsInactiveInStage)
		{
			tLocalizedText = TLocalizedText.CannotUseItemInCurrentState;
		}
		else if (component.GetRemainConsumableTypeCoolTime(tItem.ConsumableType) > 0f)
		{
			tLocalizedText = TLocalizedText.ItemUsePending;
		}
		else if (_gameManager.IsInLounge)
		{
			switch (tItem.GoodsType)
			{
			case EGoodsType.Note:
				SendUseItem();
				return;
			case EGoodsType.Blueprint:
				if (!_playerManager.IsItemUsed(tItem.Id))
				{
					_soundManager.PlaySfx("Recipe_Success");
					SendUseItem();
					return;
				}
				_soundManager.PlaySfx("Fail");
				tLocalizedText = TLocalizedText.UnlockBlueprintFailLearned;
				break;
			default:
				tLocalizedText = TLocalizedText.CannotUseInLounge;
				break;
			}
		}
		else
		{
			if (tItem.GoodsType != EGoodsType.Blueprint)
			{
				SendUseItem();
				return;
			}
			tLocalizedText = TLocalizedText.CannotUseInStage;
		}
		_toastManager.NoticeFail(tLocalizedText.LocalText);
		void SendUseItem()
		{
			_gameChannel.SendToHost(new ReqInputUseItem
			{
				ItemId = goodsSlot.Id,
				InputEvent = ESkillInputEvent.ItemSkillDown
			});
			if (tItem.GoodsType == EGoodsType.Consumable)
			{
				Close();
			}
		}
	}


	private void TransferBagInnerSwap(GoodsSlot fromSlot, GoodsSlot toSlot)
	{
		if (toSlot == null || fromSlot.SlotIndex == toSlot.SlotIndex)
		{
			return;
		}
		Goods goods = fromSlot.Goods;
		if (goods == null)
		{
			return;
		}
		long num = ((goods.Category == EGoodsCategory.Item) ? _gameManager.MyPlayer.GetInventoryItemCountMax(goods.Id) : long.MaxValue);
		if (_bagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			_playerManager.StashSlots.TrySwapInnerSlots(_bagHolderSlotIndex, fromSlot.SlotIndex, toSlot.SlotIndex, num);
		}
		else
		{
			_gameChannel.SendToHost(new ReqBagSlotToAny
			{
				BagHolderCategory = _bagHolderCategory,
				BagHolderSlotIndex = _bagHolderSlotIndex,
				BagHolderNetworkObjectId = _bagHolderNetworkObjectId,
				InnerSlotIndex = fromSlot.SlotIndex,
				TargetCategory = EGoodsSlotCategory.BagSlot,
				TargetSlotIndex = toSlot.SlotIndex,
				TargetNetworkObjectId = 0uL,
				Count = goods.Count
			});
		}
		_soundManager.PlaySfx("ItemRelease");
	}

	private void TransferBagSlotToAnyTarget(InventoryGoodsSlotModel bagSlotModel, InventoryGoodsSlotModel targetSlotModel, long count)
	{
		GoodsSlot goodsSlot = bagSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty || targetSlotModel == null)
		{
			return;
		}
		if (targetSlotModel.SlotCategory == EGoodsSlotCategory.BagSlot)
		{
			TransferBagInnerSwap(goodsSlot, targetSlotModel.GoodsSlot);
			return;
		}
		Goods goods = goodsSlot.Goods;
		long num = Math.Min(count, goods.Count);
		if (_bagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			if (targetSlotModel.SlotCategory == EGoodsSlotCategory.StashSlot)
			{
				TransferStashBagToStashSlot(goodsSlot, targetSlotModel.GoodsSlot, num);
				return;
			}
			if (!_playerManager.StashSlots.TryPullFromBag(_bagHolderSlotIndex, goodsSlot.SlotIndex, num, out var pullGoods))
			{
				return;
			}
			_gameChannel.SendToHost(new ReqGainGoods
			{
				Goods = new Goods[1] { pullGoods }
			});
		}
		else
		{
			if (goods.Category == EGoodsCategory.Item)
			{
				long? num2 = ResolveAddableForTarget(targetSlotModel.SlotCategory, goods.Id);
				if (num2.HasValue)
				{
					num = Math.Min(num, num2.Value);
					if (num < 1)
					{
						_toastManager.NoticeFail(TLocalizedText.InventoryFull.LocalText);
						return;
					}
				}
			}
			ulong targetNetworkObjectId = 0uL;
			if (targetSlotModel.SlotCategory == EGoodsSlotCategory.LootingSlot && _model.LootingInventory != null)
			{
				targetNetworkObjectId = _model.LootingInventory.NetworkObjectId;
			}
			_gameChannel.SendToHost(new ReqBagSlotToAny
			{
				BagHolderCategory = _bagHolderCategory,
				BagHolderSlotIndex = _bagHolderSlotIndex,
				BagHolderNetworkObjectId = _bagHolderNetworkObjectId,
				InnerSlotIndex = goodsSlot.SlotIndex,
				TargetCategory = targetSlotModel.SlotCategory,
				TargetSlotIndex = targetSlotModel.SlotIndex,
				TargetNetworkObjectId = targetNetworkObjectId,
				Count = num
			});
		}
		_soundManager.PlaySfx("ItemRelease");
	}

	private void TransferBagSlotToInventory(InventoryGoodsSlotModel bagSlotModel)
	{
		GoodsSlot goodsSlot = bagSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		Goods goods = goodsSlot.Goods;
		long num = ((InputManager.UiActions.InventorySelectOne.IsPressed()) ? 1 : ((!InputManager.UiKeyboardActions.InventorySelectHalf.IsPressed()) ? goods.Count : ((goods.Count + 1) / 2)));
		long num2;
		if (goods.Category == EGoodsCategory.Item)
		{
			num2 = Math.Min(num, _gameManager.MyInventory.GetAddableInventoryItemCount(goods.Id));
		}
		else
		{
			if (goods.Category != EGoodsCategory.Equipment)
			{
				return;
			}
			num2 = ((!_gameManager.MyInventory.IsSlotFull()) ? num : 0);
		}
		if (num2 < 1)
		{
			_toastManager.NoticeFail(TLocalizedText.InventoryFull.LocalText);
			return;
		}
		if (_bagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			if (!_playerManager.StashSlots.TryPullFromBag(_bagHolderSlotIndex, goodsSlot.SlotIndex, num2, out var pullGoods))
			{
				return;
			}
			_gameChannel.SendToHost(new ReqGainGoods
			{
				Goods = new Goods[1] { pullGoods }
			});
		}
		else
		{
			_gameChannel.SendToHost(new ReqBagSlotToAny
			{
				BagHolderCategory = _bagHolderCategory,
				BagHolderSlotIndex = _bagHolderSlotIndex,
				BagHolderNetworkObjectId = _bagHolderNetworkObjectId,
				InnerSlotIndex = goodsSlot.SlotIndex,
				TargetCategory = EGoodsSlotCategory.InventorySlot,
				TargetSlotIndex = -1,
				TargetNetworkObjectId = 0uL,
				Count = num2
			});
		}
		_soundManager.PlaySfx("ItemRelease");
	}


	private void TransferInventorySlotToLootingSlot(int targetIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (_model.LootingInventory != null)
		{
			if (!_model.LootingInventory.CanMoveGoods(goodsSlot.Goods))
			{
				_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
				return;
			}
			_gameChannel.SendToHost(new ReqInventorySlotToLootingSlot
			{
				LootingNetworkObjectId = _model.LootingInventory.NetworkObjectId,
				LootingSlotIndex = targetIndex,
				InventorySlotIndex = goodsSlot.SlotIndex,
				Count = Math.Min(count, goodsSlot.Count)
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void TransferInventorySlotToSafeSlot()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty || _targetSlotModel == null)
		{
			return;
		}
		if (goodsSlot.Goods is BagGoods bagGoods)
		{
			TBag tBag = TBag.Table[bagGoods.Id];
			if (_gameManager.MyInventory.HasDuplicateBagType(tBag.Type, EGoodsSlotCategory.InventorySlot, SourceSlotIndex, EGoodsSlotCategory.SafeSlot, TargetSlotIndex))
			{
				_toastManager.NoticeFail(TLocalizedText.SameBagAlreadyOwned.LocalText);
				return;
			}
		}
		_gameChannel.SendToHost(new ReqInventorySlotToSafeSlot
		{
			InventorySlotIndex = SourceSlotIndex,
			SafeSlotIndex = TargetSlotIndex
		});
		_soundManager.PlaySfx("ItemRelease");
	}

	private void TransferInventorySlotToStashSlot(int targetIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		Goods goods = GoodsCreator.MakeGoods(goodsSlot.Type, goodsSlot.Id, count, 0L);
		if (_playerManager.CanAddGoodsToStash(goods))
		{
			_gameChannel.SendToHost(new ReqInventorySlotToStashSlot
			{
				InventorySlotIndex = goodsSlot.SlotIndex,
				StashSlotIndex = targetIndex,
				Count = Math.Min(count, goodsSlot.Count)
			});
			_soundManager.PlaySfx("ItemRelease");
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughStashSlots.LocalText);
		}
	}

	private void TransferLootingSlotToInventorySlot(int targetSlotIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (goodsSlot.Goods.IsItem())
		{
			long num = ((targetSlotIndex < 0) ? _gameManager.MyInventory.GetAddableItemCountWithBags(goodsSlot.Goods.Id) : _gameManager.MyInventory.GetAddableItemCount(goodsSlot.Goods.Id));
			if (num <= 0)
			{
				_toastManager.NoticeFail(TLocalizedText.AlreadyAtMaxCount.LocalText);
				return;
			}
		}
		if (!_gameManager.MyInventory.CanMoveGoods(goodsSlot.Goods))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
			return;
		}
		if (goodsSlot.Goods is BagGoods bagGoods && _gameManager.MyInventory.HasDuplicateBagType(TBag.Table[bagGoods.Id].Type, EGoodsSlotCategory.None, -1, EGoodsSlotCategory.None, -1))
		{
			_toastManager.NoticeFail(TLocalizedText.SameBagAlreadyOwned.LocalText);
			return;
		}
		_gameChannel.SendToHost(new ReqLootingSlotToInventorySlot
		{
			LootingNetworkObjectId = _model.LootingInventory.NetworkObjectId,
			LootingSlotIndex = goodsSlot.SlotIndex,
			InventorySlotIndex = targetSlotIndex,
			Count = Math.Min(count, goodsSlot.Count)
		});
		_soundManager.PlaySfx("ItemRelease");
	}

	private void TransferLootingSlotToSafeSlot(int targetSlotIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (goodsSlot.Goods.IsItem())
		{
			long num = ((targetSlotIndex < 0) ? _gameManager.MyInventory.GetAddableItemCountWithBags(goodsSlot.Goods.Id) : _gameManager.MyInventory.GetAddableItemCount(goodsSlot.Goods.Id));
			if (num <= 0)
			{
				_toastManager.NoticeFail(TLocalizedText.AlreadyAtMaxCount.LocalText);
				return;
			}
		}
		if (!_gameManager.MyInventory.CanAddGoodsToSafeSlot(goodsSlot.Goods))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
			return;
		}
		if (goodsSlot.Goods is BagGoods bagGoods && _gameManager.MyInventory.HasDuplicateBagType(TBag.Table[bagGoods.Id].Type, EGoodsSlotCategory.None, -1, EGoodsSlotCategory.None, -1))
		{
			_toastManager.NoticeFail(TLocalizedText.SameBagAlreadyOwned.LocalText);
			return;
		}
		_gameChannel.SendToHost(new ReqLootingSlotToSafeSlot
		{
			LootingNetworkObjectId = _model.LootingInventory.NetworkObjectId,
			LootingSlotIndex = goodsSlot.SlotIndex,
			SafeSlotIndex = targetSlotIndex,
			Count = Math.Min(count, goodsSlot.Count)
		});
		_soundManager.PlaySfx("ItemRelease");
	}

	private void TransferSafeSlotToInventorySlot()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty || _targetSlotModel == null)
		{
			return;
		}
		if (goodsSlot.Goods is BagGoods bagGoods)
		{
			TBag tBag = TBag.Table[bagGoods.Id];
			if (_gameManager.MyInventory.HasDuplicateBagType(tBag.Type, EGoodsSlotCategory.SafeSlot, SourceSlotIndex, EGoodsSlotCategory.InventorySlot, TargetSlotIndex))
			{
				_toastManager.NoticeFail(TLocalizedText.SameBagAlreadyOwned.LocalText);
				return;
			}
		}
		_gameChannel.SendToHost(new ReqSafeSlotToInventorySlot
		{
			SafeSlotIndex = SourceSlotIndex,
			InventorySlotIndex = TargetSlotIndex
		});
		_soundManager.PlaySfx("ItemRelease");
	}

	private void TransferSafeSlotToLootingSlot(int targetIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (_model.LootingInventory != null)
		{
			if (!_model.LootingInventory.CanMoveGoods(goodsSlot.Goods))
			{
				_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
				return;
			}
			_gameChannel.SendToHost(new ReqSafeSlotToLootingSlot
			{
				SafeSlotIndex = goodsSlot.SlotIndex,
				LootingNetworkObjectId = _model.LootingInventory.NetworkObjectId,
				LootingSlotIndex = targetIndex,
				Count = Math.Min(count, goodsSlot.Count)
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void TransferSafeSlotToStashSlot(int targetIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		Goods goods = GoodsCreator.MakeGoods(goodsSlot.Type, goodsSlot.Id, count, 0L);
		if (_playerManager.CanAddGoodsToStash(goods))
		{
			_gameChannel.SendToHost(new ReqSafeSlotToStashSlot
			{
				SafeSlotIndex = goodsSlot.SlotIndex,
				StashSlotIndex = targetIndex,
				Count = Math.Min(count, goodsSlot.Count)
			});
			_soundManager.PlaySfx("ItemRelease");
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughStashSlots.LocalText);
		}
	}


	private void TransferStashBagToStashSlot(GoodsSlot bagSlot, GoodsSlot stashTargetSlot, long amountToPull)
	{
		if (stashTargetSlot == null)
		{
			return;
		}
		IDictionary<int, GoodsSlot> stashSlots = _playerManager.StashSlots;
		if (!stashSlots.TryPullFromBag(_bagHolderSlotIndex, bagSlot.SlotIndex, amountToPull, out var pullGoods))
		{
			return;
		}
		if (stashTargetSlot.Goods == null)
		{
			if (!stashSlots.TryPushGoods(pullGoods, stashTargetSlot.SlotIndex))
			{
				long itemMaxStackCount = ((pullGoods.Category == EGoodsCategory.Item) ? _gameManager.MyPlayer.GetInventoryItemCountMax(pullGoods.Id) : long.MaxValue);
				stashSlots.TryPushIntoBag(_bagHolderSlotIndex, pullGoods, itemMaxStackCount, bagSlot.SlotIndex);
			}
		}
		else
		{
			Goods goods = stashTargetSlot.Goods;
			if (pullGoods.Category == EGoodsCategory.Item && goods.Category == EGoodsCategory.Item && pullGoods.Id == goods.Id)
			{
				Goods goods2 = GoodsCreator.MakeGoods(pullGoods.Type, pullGoods.Id, goods.BaseCount + pullGoods.BaseCount, goods.GainCount + pullGoods.GainCount);
				stashSlots.TryPushGoods(goods2, stashTargetSlot.SlotIndex);
			}
			else
			{
				long itemMaxStackCount2 = ((goods.Category == EGoodsCategory.Item) ? _gameManager.MyPlayer.GetInventoryItemCountMax(goods.Id) : long.MaxValue);
				if (stashSlots.TryPushIntoBag(_bagHolderSlotIndex, goods, itemMaxStackCount2, bagSlot.SlotIndex))
				{
					stashSlots.TryPushGoods(pullGoods, stashTargetSlot.SlotIndex);
				}
				else
				{
					long itemMaxStackCount3 = ((pullGoods.Category == EGoodsCategory.Item) ? _gameManager.MyPlayer.GetInventoryItemCountMax(pullGoods.Id) : long.MaxValue);
					stashSlots.TryPushIntoBag(_bagHolderSlotIndex, pullGoods, itemMaxStackCount3, bagSlot.SlotIndex);
				}
			}
		}
		_soundManager.PlaySfx("ItemRelease");
	}

	private void TransferStashSlotToBagSlot(long count, int innerSlotIndex = -1)
	{
		if (_view == null || _view.BagPanel == null || _view.BagPanel.gameObject == null)
		{
			return;
		}
		if (!_view.BagPanel.gameObject.activeSelf || _selectedSlotModel == null || _selectedSlotModel.SlotCategory != EGoodsSlotCategory.StashSlot)
		{
			return;
		}
		GoodsSlot goodsSlot = _selectedSlotModel.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (goodsSlot.Category == EGoodsCategory.Bag)
		{
			_toastManager.NoticeFail(TLocalizedText.CannotPutBagInBag.LocalText);
			return;
		}
		BagGoods bag = _bagPanelModel.Bag;
		if (bag != null && TBag.Table[bag.Id].Type != goodsSlot.Goods.PushableBagType)
		{
			_toastManager.NoticeFail(TLocalizedText.GoodsNotAllowedInBag.LocalText);
			return;
		}
		long num = Math.Min(count, goodsSlot.Count);
		if (goodsSlot.Category == EGoodsCategory.Item && bag != null)
		{
			long num2 = _gameManager.MyPlayer.GetInventoryItemCountMax(goodsSlot.Goods.Id);
			long num3 = 0L;
			if (bag.GoodsSlots != null)
			{
				GoodsSlot[] goodsSlots = bag.GoodsSlots;
				foreach (GoodsSlot goodsSlot2 in goodsSlots)
				{
					if (goodsSlot2 != null && goodsSlot2.Goods != null && goodsSlot2.Goods.Id == goodsSlot.Goods.Id)
					{
						num3 += goodsSlot2.Goods.Count;
					}
				}
			}
			num = Math.Min(num, Math.Max(0L, num2 - num3));
			if (num < 1)
			{
				_toastManager.NoticeFail(TLocalizedText.BagStackFull.LocalText);
				return;
			}
		}
		if (!_playerManager.StashSlots.TryPullSlotGoods(goodsSlot.SlotIndex, num, out var pullGoods))
		{
			return;
		}
		if (_bagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			long itemMaxStackCount = ((pullGoods.Category == EGoodsCategory.Item) ? _gameManager.MyPlayer.GetInventoryItemCountMax(pullGoods.Id) : long.MaxValue);
			if (!_playerManager.StashSlots.TryPushIntoBag(_bagHolderSlotIndex, pullGoods, itemMaxStackCount, innerSlotIndex, out var swappedOutGoods))
			{
				_gameChannel.SendToHost(new ReqGainGoods
				{
					Goods = new Goods[1] { pullGoods }
				});
			}
			else if (swappedOutGoods != null && !_playerManager.StashSlots.TryPushGoods(swappedOutGoods, goodsSlot.SlotIndex))
			{
				_gameChannel.SendToHost(new ReqGainGoods
				{
					Goods = new Goods[1] { swappedOutGoods }
				});
			}
		}
		else
		{
			_gameChannel.SendToHost(new ReqAnyToBagSlot
			{
				SourceCategory = EGoodsSlotCategory.StashSlot,
				SourceSlotIndex = goodsSlot.SlotIndex,
				SourceNetworkObjectId = 0uL,
				SourceGoods = pullGoods,
				BagHolderCategory = _bagHolderCategory,
				BagHolderSlotIndex = _bagHolderSlotIndex,
				BagHolderNetworkObjectId = _bagHolderNetworkObjectId,
				InnerSlotIndex = innerSlotIndex,
				Count = num
			});
		}
		_soundManager.PlaySfx("ItemRelease");
	}


	private void TransferStashSlotToInventorySlot(int targetIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (goodsSlot.Goods.IsItem())
		{
			long num = ((targetIndex < 0) ? _gameManager.MyInventory.GetAddableItemCountWithBags(goodsSlot.Goods.Id) : _gameManager.MyInventory.GetAddableItemCount(goodsSlot.Goods.Id));
			if (num <= 0)
			{
				_toastManager.NoticeFail(TLocalizedText.AlreadyAtMaxCount.LocalText);
				return;
			}
		}
		if (!_gameManager.MyInventory.CanMoveGoods(goodsSlot.Goods))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
			return;
		}
		if (goodsSlot.Goods is BagGoods bagGoods && _gameManager.MyInventory.HasDuplicateBagType(TBag.Table[bagGoods.Id].Type, EGoodsSlotCategory.None, -1, EGoodsSlotCategory.None, -1))
		{
			_toastManager.NoticeFail(TLocalizedText.SameBagAlreadyOwned.LocalText);
			return;
		}
		Goods goods = null;
		if (goodsSlot.Category == EGoodsCategory.Equipment && !_playerManager.StashSlots.TryPullSlotGoods(goodsSlot.SlotIndex, count, out goods))
		{
			return;
		}
		if (goodsSlot.Category == EGoodsCategory.Bag && !_playerManager.StashSlots.TryPullSlotGoods(goodsSlot.SlotIndex, count, out goods))
		{
			return;
		}
		if (goodsSlot.Category == EGoodsCategory.Item)
		{
			long addableCount = ((targetIndex < 0) ? _gameManager.MyInventory.GetAddableItemCountWithBags(goodsSlot.Id) : _gameManager.MyInventory.GetAddableItemCount(goodsSlot.Id));
			long num2 = _gameManager.MyInventory.Slots.FindItemSlot(goodsSlot.Id)?.Count ?? 0;
			long num3 = _gameManager.MyInventory.SafeSlots.FindItemSlot(goodsSlot.Id)?.Count ?? 0;
			long num4 = Math.Min(Math.Min(count, goodsSlot.Count), addableCount);
			if (num3 + num2 < 1 && num4 < 1)
			{
				return;
			}
			bool flag = false;
			if (num4 > 0 && _playerManager.StashSlots.TryPullSlotGoods(goodsSlot.SlotIndex, num4, out var pullGoods))
			{
				goods = pullGoods;
				flag = true;
			}
			if (!flag)
			{
				goods = GoodsCreator.MakeItemGoods(goodsSlot.Id, 0L, 0L);
			}
		}
		if (goods != null)
		{
			_gameChannel.SendToHost(new ReqStashSlotToInventorySlot
			{
				InventorySlotIndex = targetIndex,
				Goods = goods
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void TransferStashSlotToSafeSlot(int targetIndex, long count)
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (goodsSlot.Goods.IsItem())
		{
			long num = ((targetIndex < 0) ? _gameManager.MyInventory.GetAddableItemCountWithBags(goodsSlot.Goods.Id) : _gameManager.MyInventory.GetAddableItemCount(goodsSlot.Goods.Id));
			if (num <= 0)
			{
				_toastManager.NoticeFail(TLocalizedText.AlreadyAtMaxCount.LocalText);
				return;
			}
		}
		if (!_gameManager.MyInventory.CanAddGoodsToSafeSlot(goodsSlot.Goods))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
			return;
		}
		if (goodsSlot.Goods is BagGoods bagGoods && _gameManager.MyInventory.HasDuplicateBagType(TBag.Table[bagGoods.Id].Type, EGoodsSlotCategory.None, -1, EGoodsSlotCategory.None, -1))
		{
			_toastManager.NoticeFail(TLocalizedText.SameBagAlreadyOwned.LocalText);
			return;
		}
		Goods goods = null;
		if (goodsSlot.Category == EGoodsCategory.Equipment && !_playerManager.StashSlots.TryPullSlotGoods(goodsSlot.SlotIndex, count, out goods))
		{
			return;
		}
		if (goodsSlot.Category == EGoodsCategory.Bag && !_playerManager.StashSlots.TryPullSlotGoods(goodsSlot.SlotIndex, count, out goods))
		{
			return;
		}
		if (goodsSlot.Category == EGoodsCategory.Item)
		{
			long addableCount = ((targetIndex < 0) ? _gameManager.MyInventory.GetAddableItemCountWithBags(goodsSlot.Id) : _gameManager.MyInventory.GetAddableItemCount(goodsSlot.Id));
			long num2 = _gameManager.MyInventory.Slots.FindItemSlot(goodsSlot.Id)?.Count ?? 0;
			long num3 = _gameManager.MyInventory.SafeSlots.FindItemSlot(goodsSlot.Id)?.Count ?? 0;
			long num4 = Math.Min(Math.Min(count, goodsSlot.Count), addableCount);
			if (num3 + num2 < 1 && num4 < 1)
			{
				return;
			}
			bool flag = false;
			if (num4 > 0 && _playerManager.StashSlots.TryPullSlotGoods(goodsSlot.SlotIndex, num4, out var pullGoods))
			{
				goods = pullGoods;
				flag = true;
			}
			if (!flag)
			{
				goods = GoodsCreator.MakeItemGoods(goodsSlot.Id, 0L, 0L);
			}
		}
		if (goods != null)
		{
			_gameChannel.SendToHost(new ReqStashSlotToSafeSlot
			{
				SafeSlotIndex = targetIndex,
				Goods = goods
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}


	private void TransferToBagSlot(long count, int innerSlotIndex = -1)
	{
		if (_view == null || _view.BagPanel == null || _view.BagPanel.gameObject == null)
		{
			return;
		}
		if (!_view.BagPanel.gameObject.activeSelf || _selectedSlotModel == null)
		{
			return;
		}
		GoodsSlot goodsSlot = _selectedSlotModel.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (goodsSlot.Category == EGoodsCategory.Bag)
		{
			_toastManager.NoticeFail(TLocalizedText.CannotPutBagInBag.LocalText);
			return;
		}
		BagGoods bag = _bagPanelModel.Bag;
		if (bag != null && TBag.Table[bag.Id].Type != goodsSlot.Goods.PushableBagType)
		{
			_toastManager.NoticeFail(TLocalizedText.GoodsNotAllowedInBag.LocalText);
			return;
		}
		long num = Math.Min(count, goodsSlot.Count);
		if (goodsSlot.Category == EGoodsCategory.Item && bag != null)
		{
			long num2 = _gameManager.MyPlayer.GetInventoryItemCountMax(goodsSlot.Goods.Id);
			long num3 = 0L;
			if (bag.GoodsSlots != null)
			{
				GoodsSlot[] goodsSlots = bag.GoodsSlots;
				foreach (GoodsSlot goodsSlot2 in goodsSlots)
				{
					if (goodsSlot2 != null && goodsSlot2.Goods != null && goodsSlot2.Goods.Id == goodsSlot.Goods.Id)
					{
						num3 += goodsSlot2.Goods.Count;
					}
				}
			}
			num = Math.Min(num, Math.Max(0L, num2 - num3));
			if (num <= 0)
			{
				_toastManager.NoticeFail(TLocalizedText.BagStackFull.LocalText);
				return;
			}
		}
		if (_bagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			long itemMaxStackCount = ((goodsSlot.Category == EGoodsCategory.Item) ? _gameManager.MyPlayer.GetInventoryItemCountMax(goodsSlot.Goods.Id) : long.MaxValue);
			Goods goods = goodsSlot.Goods;
			if (goodsSlot.Category == EGoodsCategory.Item)
			{
				goods = GoodsCreator.MakeGoods(goodsSlot.Goods.Type, goodsSlot.Goods.Id, num, 0L);
			}
			if (!_playerManager.StashSlots.CanPushIntoBag(_bagHolderSlotIndex, goods, itemMaxStackCount, innerSlotIndex))
			{
				_toastManager.NoticeFail(TLocalizedText.BagStackFull.LocalText);
				return;
			}
		}
		ulong sourceNetworkObjectId = 0uL;
		if (_selectedSlotModel.SlotCategory == EGoodsSlotCategory.LootingSlot && _model.LootingInventory != null)
		{
			sourceNetworkObjectId = _model.LootingInventory.NetworkObjectId;
		}
		_gameChannel.SendToHost(new ReqAnyToBagSlot
		{
			SourceCategory = _selectedSlotModel.SlotCategory,
			SourceSlotIndex = goodsSlot.SlotIndex,
			SourceNetworkObjectId = sourceNetworkObjectId,
			BagHolderCategory = _bagHolderCategory,
			BagHolderSlotIndex = _bagHolderSlotIndex,
			BagHolderNetworkObjectId = _bagHolderNetworkObjectId,
			InnerSlotIndex = innerSlotIndex,
			Count = num
		});
		_soundManager.PlaySfx("ItemRelease");
	}


	private void UnequipToInventorySlot()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (!_gameManager.MyInventory.CanMoveGoods(goodsSlot.Goods))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
			return;
		}
		_gameChannel.SendToHost(new ReqUnequipToInventorySlot
		{
			EquipmentSlotCategory = _selectedSlotModel.SlotCategory,
			EquipmentSlotIndex = goodsSlot.SlotIndex,
			InventorySlotIndex = TargetSlotIndex
		});
		_soundManager.PlaySfx("ItemRelease");
	}

	private void UnequipToLootingSlot()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (_model.LootingInventory != null)
		{
			if (!_model.LootingInventory.CanMoveGoods(goodsSlot.Goods))
			{
				_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
				return;
			}
			_gameChannel.SendToHost(new ReqUnequipToLootingSlot
			{
				EquipmentSlotCategory = _selectedSlotModel.SlotCategory,
				EquipmentSlotIndex = goodsSlot.SlotIndex,
				LootingNetworkObjectId = _model.LootingInventory.NetworkObjectId,
				LootingSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void UnequipToSafeSlot()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (_gameManager.MyInventory.CanAddGoodsToSafeSlot(goodsSlot.Goods))
		{
			_gameChannel.SendToHost(new ReqUnequipToSafeSlot
			{
				EquipmentSlotCategory = _selectedSlotModel.SlotCategory,
				EquipmentSlotIndex = goodsSlot.SlotIndex,
				SafeSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
		}
	}

	private void UnequipToStashSlot()
	{
		GoodsSlot goodsSlot = _selectedSlotModel?.GoodsSlot;
		if (goodsSlot == null || goodsSlot.IsEmpty)
		{
			return;
		}
		if (_playerManager.CanAddGoodsToStash(goodsSlot.Goods))
		{
			_gameChannel.SendToHost(new ReqUnequipToStashSlot
			{
				EquipmentSlotCategory = _selectedSlotModel.SlotCategory,
				EquipmentSlotIndex = goodsSlot.SlotIndex,
				StashSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughInventorySlots.LocalText);
		}
	}
}
