using System;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class InteriorSlotPresenter : BaseSlotPresenter<InteriorSlotModel>
{
	[InjectModel]
	private InteriorSlotModel _model;

	[View]
	private InteriorSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private AssetManager _assetManager;

	protected override InteriorSlotModel SlotModel => _model;

	private void OnInject(InteriorSlotModel model)
	{
		InitSlot(model);
		TInteriorShop tInteriorShop = TInteriorShop.Table[model.InteriorShopId];
		_view.TypeText.text = model.Interior.Type.ToLocalText();
		switch (model.Interior.Type)
		{
		case EInteriorType.Area:
			SetupArea(tInteriorShop);
			break;
		case EInteriorType.NpcLevel:
			SetupNpcLevel(tInteriorShop);
			break;
		case EInteriorType.AreaDecoration:
			SetupDeco(tInteriorShop);
			break;
		}
	}

	private void SetupArea(TInteriorShop shop)
	{
		TArea tArea = TArea.Table[shop.Interior.Id];
		bool flag = _gameLoungeManager.IsAreaUnlocked(shop.Interior.Id);
		_view.NameText.text = tArea.Name;
		_view.CheckBox.gameObject.SetActive(flag);
		_view.NewTag.SetActive(false);
		_view.Img.sprite = _assetManager.GetIconInterior(EInteriorType.Area, shop.Interior.Id);
		SetupLocked(shop.UnlockConditions, flag);
		SetupCost(shop.ConsumeGold, shop.ConsumeCoin, flag);
	}

	private void SetupDeco(TInteriorShop shop)
	{
		TAreaDecoration tAreaDecoration = TAreaDecoration.Table[shop.Interior.Id];
		bool flag = _gameLoungeManager.IsAreaDecorationUnlocked(shop.Interior.Id);
		bool flag2 = _gameContext.ActiveAreaDecorations != null && _gameContext.ActiveAreaDecorations.TryGetValue(tAreaDecoration.GroupId, out var value) && value == shop.Interior.Id;
		_view.NameText.text = tAreaDecoration.Name;
		_view.CheckBox.gameObject.SetActive(flag2 & flag);
		_view.NewTag.SetActive(false);
		_view.Img.sprite = _assetManager.GetIconInterior(EInteriorType.AreaDecoration, shop.Interior.Id);
		bool isAreaLocked = !_gameLoungeManager.IsAreaUnlocked(shop.AreaId);
		SetupLocked(shop.UnlockConditions, flag, isAreaLocked);
		SetupCost(shop.ConsumeGold, shop.ConsumeCoin, flag);
	}

	private void SetupNpcLevel(TInteriorShop shop)
	{
		TNpc tNpc = TNpc.Table[shop.Interior.Id];
		int value = shop.Interior.Value;
		bool flag = value <= _gameLoungeManager.GetNpcLevel(shop.Interior.Id);
		_view.NameText.text = tNpc.Name;
		_view.CheckBox.gameObject.SetActive(flag);
		_view.NewTag.SetActive(false);
		_view.Img.sprite = _assetManager.GetIconInterior(EInteriorType.NpcLevel, shop.Interior.Id);
		SetupLocked(shop.UnlockConditions, flag);
		SetupCost(shop.ConsumeGold, shop.ConsumeCoin, flag);
	}

	private void SetupLocked(EventCondition[] unlockConditions, bool isUnlocked, bool isAreaLocked = false)
	{
		if (isAreaLocked)
		{
			_view.Locked.gameObject.SetActive(true);
			_view.Locked_Text.gameObject.SetActive(false);
		}
		else if (!isUnlocked)
		{
			bool flag = unlockConditions != null && unlockConditions.Length > 0;
			bool flag2 = !flag || _playerManager.CheckStateConditions(unlockConditions);
			_view.Locked.gameObject.SetActive(!flag2 && flag);
			if (flag && !flag2)
			{
				EventCondition[] array = unlockConditions.Where((EventCondition c) => c.Type != EEventConditionType.AreaUnlocked).ToArray();
				_view.Locked_Text.gameObject.SetActive(array.Length != 0);
				if (array.Length != 0)
				{
					_view.Locked_Text.text = string.Join("\n", array.Select((EventCondition c) =>
					{
						string colorCode = (_playerManager.CheckStateCondition(c) ? "#DCDCDC" : "#a45646");
						return c.ToDescription().WithColor(colorCode);
					}));
				}
			}
		}
		else
		{
			_view.Locked.gameObject.SetActive(false);
		}
	}

	private void SetupCost(int consumeGold, int consumeCoin, bool isUnlocked)
	{
		_view.Gold.SetActive(!isUnlocked && consumeGold > 0);
		_view.Coin.SetActive(!isUnlocked && consumeCoin > 0);
		if (isUnlocked)
		{
			return;
		}
		if (consumeGold > 0)
		{
			bool flag = _playerManager.HasEnoughGold(consumeGold);
			_view.Gold_Value.text = consumeGold.ToNumberString();
			_view.Gold_Value.color = UiUtility.HexToColor(flag ? "#DCDCDC" : "#a45646");
		}
		if (consumeCoin > 0)
		{
			bool flag2 = _playerManager.HasEnoughCoin(consumeCoin);
			_view.Coin_Value.text = consumeCoin.ToNumberString();
			_view.Coin_Value.color = UiUtility.HexToColor(flag2 ? "#DCDCDC" : "#a45646");
		}
	}

	private bool IsAreaLocked(TInteriorShop shop)
	{
		return !_gameLoungeManager.IsAreaUnlocked(shop.AreaId);
	}
}
