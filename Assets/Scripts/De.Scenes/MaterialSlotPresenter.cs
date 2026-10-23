using System;
using GameCommon.Data;
using NemoLib.Mvp;
using VContainer;

public class MaterialSlotPresenter : Presenter
{
	[View]
	private MaterialSlotView _view;

	[InjectModel]
	private MaterialSlotModel _model;

	[Inject]
	private PlayerManager _playerManager;

	private void OnInject(MaterialSlotModel model)
	{
		GoodsSlotModel goodsSlotModel = new GoodsSlotModel(_model.Goods);
		goodsSlotModel.ShowItemCount = false;
		goodsSlotModel.UseTooltip = !_model.IsUnknown;
		goodsSlotModel.IsCrafting = _model.IsCrafting;
		goodsSlotModel.OnCraftingNavigate = _model.OnCraftingNavigate;
		_view.GoodsSlot.Inject(goodsSlotModel);
		if (!_model.IsUnknown)
		{
			UpdateUI();
		}
		else
		{
			UpdateUnknownUI();
		}
	}

	private void UpdateUI()
	{
		long totalGoodsCount = _playerManager.GetTotalGoodsCount(_model.Goods);
		string text = ((totalGoodsCount < _model.Goods.Count) ? totalGoodsCount.ToString().WithColor("#a45646") : totalGoodsCount.ToString());
		_view.TextCount.text = string.Format("{0}/{1}", text, _model.Goods.Count);
	}

	private void UpdateUnknownUI()
	{
		_view.TextCount.text = "?/?".WithColor("#a45646");
	}
}
