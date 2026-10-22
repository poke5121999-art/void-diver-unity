using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
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

	protected override ShopProductSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(ShopProductSlotModel model)
	{
	}

	private void OnShopProductChanged(IList<ShopProductChangeData> changes)
	{
	}

	private void SetStockText(ShopProduct shopProduct)
	{
	}

	private void SetupConsumeGoods(Goods consumeGoods)
	{
	}

	private void OnButtonLayoutChanged(EButtonState state)
	{
	}
}
