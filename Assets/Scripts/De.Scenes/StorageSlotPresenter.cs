using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class StorageSlotPresenter : BaseSlotPresenter<StorageSlotModel>
{
	[InjectModel]
	private StorageSlotModel _model;

	[View]
	private StorageSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	protected override StorageSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(StorageSlotModel model)
	{
		InitSlot(model);
		long totalGoodsCount = _playerManager.GetTotalGoodsCount(model.Goods, true);
		GoodsSlotModel goodsSlotModel = new GoodsSlotModel(model.Goods);
		goodsSlotModel.ShowItemCount = true;
		_view.GoodsSlot.Inject(goodsSlotModel);
		_view.Text.text = model.Goods.GetName();
		_view.OwnedQuantity.text = TLocalizedText.GoodsCountOwnedFormat.Format(totalGoodsCount);
	}
}
