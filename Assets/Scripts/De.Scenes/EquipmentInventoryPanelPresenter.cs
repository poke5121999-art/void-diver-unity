using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using SomeNet.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class EquipmentInventoryPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private EquipmentInventoryPanelModel _model;

	[View]
	private EquipmentInventoryPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	private InventoryGoodsSlotModel[] _weaponSlotModels;

	private InventoryGoodsSlotModel[] _accessorySlotModels;

	private InventoryGoodsSlotModel[] _artifactSlotModels;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	private GamePlayer Player => _gameManager.MyPlayer;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
	}

	private void OnInject(EquipmentInventoryPanelModel model)
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
		_disposable = new CompositeDisposable();
		InitEquipmentSlots();
		InitGamePadNav();
		model.RxHighlightCategory.Subscribe(OnHighlightCategoryChanged).AddTo(_disposable);
		Player.RxCharacter.Pairwise().Subscribe(delegate(Pair<Character> pair)
		{
			OnCharacterChanged(pair.Previous, pair.Current);
		}).AddTo(_disposable);
		model.OutOfLeft.Subscribe(delegate
		{
			OutOfLeft();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool b) => b).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool b) => !b).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OutOfLeft()
	{
		_gameChannel.SendToHost(new ReqSwapWeapon
		{
			AllowEmpty = true
		});
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> list = new List<Selectable>();
		list.Add(_view.WeaponSlot.GetComponent<InventoryGoodsSlotView>().Slot);
		list.AddRange(_view.AccessorySlots.Select((GameObject go) => go.GetComponent<InventoryGoodsSlotView>().Slot));
		list.AddRange(_view.ArtifactSlots.Select((GameObject go) => go.GetComponent<InventoryGoodsSlotView>().Slot));
		if (list.Count != 0)
		{
			SetNavHorizontal(list);
		}
	}

	private void InitEquipmentSlots()
	{
		Character character = _playerManager.SelectedCharacter;
		InitWeapon();
		InitAccessories();
		InitArtifacts();

		void InitAccessories()
		{
			int intValue = TConst.CharacterAccessorySlotCount.IntValue;
			_accessorySlotModels = new InventoryGoodsSlotModel[intValue];
			for (int i = 0; i < intValue; i++)
			{
				_accessorySlotModels[i] = CreateSlotModel(EGoodsSlotCategory.AccessorySlot, character.AccessorySlots[i].Goods, i);
				_view.AccessorySlots[i].GetComponent<InventoryGoodsSlotPresenter>().Inject(_accessorySlotModels[i]);
			}
		}

		void InitArtifacts()
		{
			int intValue2 = TConst.CharacterArtifactSlotCount.IntValue;
			_artifactSlotModels = new InventoryGoodsSlotModel[intValue2];
			for (int j = 0; j < intValue2; j++)
			{
				_artifactSlotModels[j] = CreateSlotModel(EGoodsSlotCategory.ArtifactSlot, character.ArtifactSlots[j].Goods, j);
				_view.ArtifactSlots[j].GetComponent<InventoryGoodsSlotPresenter>().Inject(_artifactSlotModels[j]);
			}
		}

		void InitWeapon()
		{
			_weaponSlotModels = new InventoryGoodsSlotModel[2];
			for (int k = 0; k < 2; k++)
			{
				_weaponSlotModels[k] = CreateSlotModel(EGoodsSlotCategory.WeaponSlot, character.WeaponSlots[k].Goods, k);
			}
			int activeWeaponIndex = character.ActiveWeaponIndex;
			_view.WeaponSlot.Inject(_weaponSlotModels[activeWeaponIndex]);
			_view.SubWeapon_WeaponSlot.Inject(_weaponSlotModels[(activeWeaponIndex == 0) ? 1 : 0]);
			_view.SubWeapon_Dim.raycastTarget = false;
			_view.SubWeapon_IconSwap.raycastTarget = false;
		}

		InventoryGoodsSlotModel CreateSlotModel(EGoodsSlotCategory category, Goods goods, int index)
		{
			GoodsSlot slot = new GoodsSlot
			{
				Goods = goods,
				SlotIndex = index
			};
			InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(category, slot)
			{
				UseTooltip = false,
				ShowItemCount = false
			};
			inventoryGoodsSlotModel.OnClick.Subscribe(_model.OnEquipmentSlotClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnLeftClick.Subscribe(_model.OnEquipmentSlotLeftClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnRightClick.Subscribe(_model.OnEquipmentSlotRightClick).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEnter.Subscribe(OnEquipmentSlotEnter).AddTo(_disposable);
			inventoryGoodsSlotModel.OnExit.Subscribe(OnEquipmentSlotExit).AddTo(_disposable);
			inventoryGoodsSlotModel.OnDrag.Subscribe(_model.OnEquipmentSlotDrag).AddTo(_disposable);
			inventoryGoodsSlotModel.OnEndDrag.Subscribe(_model.OnEquipmentSlotEndDrag).AddTo(_disposable);
			return inventoryGoodsSlotModel;
		}
	}

	private void OnEquipmentSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedEquipmentSlotModel.SetValueAndForceNotify(slotModel);
	}

	private void OnEquipmentSlotExit(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedEquipmentSlotModel.SetValueAndForceNotify(null);
	}

	private void OnHighlightCategoryChanged(EGoodsSlotCategory slotCategory)
	{
		_weaponSlotModels.ForEach(delegate(InventoryGoodsSlotModel m)
		{
			m.RxHighlightOn.Value = m.SlotCategory == slotCategory;
		});
		_accessorySlotModels.ForEach(delegate(InventoryGoodsSlotModel m)
		{
			m.RxHighlightOn.Value = m.SlotCategory == slotCategory;
		});
		_artifactSlotModels.ForEach(delegate(InventoryGoodsSlotModel m)
		{
			m.RxHighlightOn.Value = m.SlotCategory == slotCategory;
		});
	}

	private void OnCharacterChanged(Character oldCharacter, Character newCharacter)
	{
		UpdateWeapon();
		UpdateAccessories();
		UpdateArtifacts();

		void UpdateAccessories()
		{
			for (int i = 0; i < _accessorySlotModels.Length; i++)
			{
				AccessoryGoods accessoryGoods = oldCharacter.AccessorySlots[i].Goods as AccessoryGoods;
				AccessoryGoods accessoryGoods2 = newCharacter.AccessorySlots[i].Goods as AccessoryGoods;
				if (!(((accessoryGoods != null) ? accessoryGoods.Id : 0) == ((accessoryGoods2 != null) ? accessoryGoods2.Id : 0) && Mathf.Approximately((accessoryGoods != null) ? accessoryGoods.CurrentDurability : 0f, (accessoryGoods2 != null) ? accessoryGoods2.CurrentDurability : 0f)))
				{
					_accessorySlotModels[i].GoodsSlot = new GoodsSlot
					{
						SlotIndex = i,
						Goods = accessoryGoods2
					};
				}
			}
		}

		void UpdateArtifacts()
		{
			for (int j = 0; j < _artifactSlotModels.Length; j++)
			{
				Goods goods = oldCharacter.ArtifactSlots[j].Goods;
				Goods goods2 = newCharacter.ArtifactSlots[j].Goods;
				if (((goods != null) ? goods.Id : 0) != ((goods2 != null) ? goods2.Id : 0))
				{
					_artifactSlotModels[j].GoodsSlot = new GoodsSlot
					{
						SlotIndex = j,
						Goods = goods2
					};
				}
			}
		}

		void UpdateWeapon()
		{
			for (int k = 0; k < _weaponSlotModels.Length; k++)
			{
				WeaponGoods weaponGoods = oldCharacter.WeaponSlots[k].Goods as WeaponGoods;
				WeaponGoods weaponGoods2 = newCharacter.WeaponSlots[k].Goods as WeaponGoods;
				if (!(((weaponGoods != null) ? weaponGoods.Id : 0) == ((weaponGoods2 != null) ? weaponGoods2.Id : 0) && Mathf.Approximately((weaponGoods != null) ? weaponGoods.CurrentDurability : 0f, (weaponGoods2 != null) ? weaponGoods2.CurrentDurability : 0f)))
				{
					_weaponSlotModels[k].GoodsSlot = new GoodsSlot
					{
						SlotIndex = k,
						Goods = weaponGoods2
					};
				}
			}
			if (oldCharacter.ActiveWeaponIndex != newCharacter.ActiveWeaponIndex)
			{
				int activeWeaponIndex = newCharacter.ActiveWeaponIndex;
				_view.WeaponSlot.Inject(_weaponSlotModels[activeWeaponIndex]);
				_view.SubWeapon_WeaponSlot.Inject(_weaponSlotModels[(activeWeaponIndex == 0) ? 1 : 0]);
			}
		}
	}
}
