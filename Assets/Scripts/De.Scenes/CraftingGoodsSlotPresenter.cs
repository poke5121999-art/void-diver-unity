using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class CraftingGoodsSlotPresenter : BaseSlotPresenter<CraftingGoodsSlotModel>
{
	[InjectModel]
	private CraftingGoodsSlotModel _model;

	[View]
	private CraftingGoodsSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	protected override CraftingGoodsSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private TCrafting CraftingData => TCrafting.Table[_model.CraftingId];

	private bool HasEnoughMaterials => _playerManager.HasEnoughGoods(CraftingData.MaterialGoodsDatas);

	private void OnInject(CraftingGoodsSlotModel model)
	{
		InitSlot(model);
		Goods goods = TCrafting.Table[model.CraftingId].ResultGoodsData.ToGoodsAsBase();
		InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(goods, InventoryGoodsSlotPresenter.ELayout.Normal);
		inventoryGoodsSlotModel.UseTooltip = false;
		inventoryGoodsSlotModel.ShowItemCount = true;
		_view.GoodsSlot.Inject(inventoryGoodsSlotModel);
		_view.Text.text = goods.GetName();
		_playerManager.RxProfile.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(Disposable);
		_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(Disposable);
	}

	private void OnUpdateUis()
	{
		long totalGoodsCount = _playerManager.GetTotalGoodsCount(CraftingData.ResultGoodsData, includeEquippedGoods: true);
		_view.OwnedQuantity.text = TLocalizedText.GoodsCountOwnedFormat.Format(totalGoodsCount);
		_view.Available.SetActive(_playerManager.HasEnoughGoods(CraftingData.MaterialGoodsDatas));
	}
}
