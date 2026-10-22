using System.Collections.Generic;
using GameCommon.Data;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopProductSlotPresenter : BaseSlotPresenter<ShopProductSlotModel>
{
	[InjectModel]
	private ShopProductSlotModel _model;

	[View]
	private ShopProductSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameShopManager _shopManager;

	protected override ShopProductSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(ShopProductSlotModel model)
	{
		InitSlot(model);
		TShopProduct tShopProduct = TShopProduct.Table[model.Id];
		ShopProduct shopProduct = _shopManager.GetShopProduct(model.Id);
		Goods goods = tShopProduct.GainGoodsData.ToGoodsAsBase();
		Goods consumeGoods = tShopProduct.ConsumeGoodsData.ToGoodsAsBase();
		string name = goods.GetName();
		_view.Text.text = name;
		SetupConsumeGoods(consumeGoods);
		SetStockText(shopProduct);
		_shopManager.OnShopProductChanged.Subscribe(OnShopProductChanged).AddTo(Disposable);
		InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(goods, InventoryGoodsSlotPresenter.ELayout.Normal);
		inventoryGoodsSlotModel.UseTooltip = false;
		inventoryGoodsSlotModel.ShowItemCount = false;
		_view.GoodsSlot.Inject(inventoryGoodsSlotModel);
		model.RxButtonState.Subscribe(OnButtonLayoutChanged).AddTo(Disposable);
	}

	private void OnShopProductChanged(IList<ShopProductChangeData> changes)
	{
		ShopProductChangeData shopProductChangeData = changes.FirstOrDefault((ShopProductChangeData _) => _.Key == _model.Id);
		if (shopProductChangeData != null)
		{
			SetStockText(shopProductChangeData.NewValue);
		}
	}

	private void SetStockText(ShopProduct shopProduct)
	{
		bool flag = shopProduct == null || shopProduct.StockMax < 1;
		_view.TextStock.gameObject.SetActive(!flag);
		if (!flag)
		{
			_view.TextStock.text = TLocalizedText.StockCountFormat.Format(shopProduct.Stock, shopProduct.StockMax);
		}
	}

	private void SetupConsumeGoods(Goods consumeGoods)
	{
		string text = string.Format("{0:N0}", consumeGoods.Count);
		_view.Gold.SetActive(false);
		_view.Coin.SetActive(false);
		_view.Item.SetActive(false);
		switch ((int)consumeGoods.Type)
		{
		case 1:
			_view.Gold.SetActive(true);
			_view.Gold_Value.text = text;
			break;
		case 2:
			_view.Coin.SetActive(true);
			_view.Coin_Value.text = text;
			break;
		default:
			_view.Item.SetActive(true);
			_view.Item_Icon.sprite = _assetManager.GetIconGoods(consumeGoods.Type, consumeGoods.Id);
			_view.Item_Value.text = text;
			break;
		}
	}

	private void OnButtonLayoutChanged(EButtonState state)
	{
		_view.Button.GetComponent<Animator>()?.SetInteger("Layout", (int)state);
	}
}
