using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CraftingMaterialPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private CraftingMaterialPanelModel _model;

	[View]
	private CraftingMaterialPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	private const int _numberOfSlotsInRow = 6;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	public int NavigableColumnCount => 6;

	protected void OnInject(CraftingMaterialPanelModel model)
	{
		_disposable?.Clear();
		_disposable = new CompositeDisposable();
		_view.NeedCurrency.SetActive(_model.NeedGold > 0);
		_view.NeedCoin.SetActive(_model.NeedCoin > 0);
		_view.NeedRealCoin.SetActive(_model.NeedRealCoin > 0);
		_view.Confirm.SetActive(model.ShowConfirm);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			model.OnConfirm.OnNext(Unit.Default);
		}).AddTo(_disposable);
		_playerManager.RxProfile.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(_disposable);
		_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(_disposable);
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

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> list = _view.MaterialSlots.Take(_model.Materials?.Length ?? 0).Select(SlotSelectable).ToList();
		SetNavGrid(list, 6);
		if (!_model.ShowConfirm)
		{
			return;
		}
		List<Selectable> items = new List<Selectable> { _view.Confirm_Button };
		SetNavVertical(items);
		for (int i = 0; i < list.Count; i++)
		{
			if (list.Count <= i + 6)
			{
				ConnectNav(list[i], ENavDir.Down, _view.Confirm_Button);
			}
		}
		if (list.Count > 0)
		{
			ConnectNav(_view.Confirm_Button, ENavDir.Up, list[(list.Count - 1) / 6 * 6]);
		}
		SetDefaultNav(_view.Confirm_Button);
	}

	// <GetNavigableSlots>b__11_0 @0x1808dc5c0 (machine code checked), shared by InitGamePadNav: a material slot's selectable is
	// the Button of the GoodsSlotView under its MaterialSlotView, not a Selectable on the slot object (it has none, so
	// SetNavGrid got null keys and OnInject stopped before input was wired).
	private static Selectable SlotSelectable(GameObject slot)
	{
		return slot.GetComponent<MaterialSlotView>().GoodsSlot.GetComponent<GoodsSlotView>().Slot;
	}

	public IReadOnlyList<Selectable> GetNavigableSlots()
	{
		return _view.MaterialSlots.Take(_model.Materials?.Length ?? 0).Select(SlotSelectable).ToList();
	}

	private void OnUpdateUis()
	{
		Goods[] array = _model.Materials?.Select((Goods _) => _.Clone()).ToArray() ?? Array.Empty<Goods>();
		_view.Caption.gameObject.SetActive(array.Any());
		_view.MaterialSlots.First().transform.parent.gameObject.SetActive(array.Any());
		if (array.Any())
		{
			for (int i = 0; i < _view.MaterialSlots.Length; i++)
			{
				GameObject gameObject = _view.MaterialSlots[i];
				gameObject.SetActive(i < array.Length);
				if (i < array.Length)
				{
					MaterialSlotPresenter materialSlotPresenter = gameObject.GetComponent<MaterialSlotPresenter>();
					MaterialSlotModel model = new MaterialSlotModel
					{
						Goods = array[i],
						IsUnknown = false,
						IsCrafting = true,
						OnCraftingNavigate = _model.OnCraftingNavigate
					};
					materialSlotPresenter.Inject(model);
				}
			}
		}
		long num = _model.NeedGold * _model.CraftingCount;
		string text = num.ToString("N0");
		_view.GoldText.text = (_playerManager.HasEnoughGold(num) ? text : text.WithColor("#a45646"));
		if (_model.NeedCoin > 0)
		{
			long num2 = _model.NeedCoin * _model.CraftingCount;
			string text2 = num2.ToString("N0");
			_view.CoinText.text = (_playerManager.HasEnoughCoin(num2) ? text2 : text2.WithColor("#a45646"));
		}
		if (_model.NeedRealCoin > 0)
		{
			long num3 = _model.NeedRealCoin * _model.CraftingCount;
			long totalGoodsCount = _playerManager.GetTotalGoodsCount(GoodsCreator.MakeItemGoods(10001));
			string text3 = num3.ToString("N0");
			_view.RealCoinText.text = ((totalGoodsCount >= num3) ? text3 : text3.WithColor("#a45646"));
		}
		Goods[] array2 = _model.Materials ?? Array.Empty<Goods>();
		if (_model.NeedGold > 0)
		{
			array2 = array2.Append(new GoldGoods(0L, _model.NeedGold)).ToArray();
		}
		if (_model.NeedCoin > 0)
		{
			array2 = array2.Append(new CoinGoods(0L, _model.NeedCoin)).ToArray();
		}
		if (_model.NeedRealCoin > 0)
		{
			array2 = array2.Append(GoodsCreator.MakeItemGoods(10001, _model.NeedRealCoin)).ToArray();
		}
		long num4 = _playerManager.CalculateEnoughCraftingCount(array2);
		_view.CraftableCount.text = TLocalizedText.CraftableItemCountFormat.Format(num4);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable.Dispose();
	}
}
