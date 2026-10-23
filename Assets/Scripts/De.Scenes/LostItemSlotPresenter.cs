using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UnityEngine.UI;

public class LostItemSlotPresenter : BaseSlotPresenter<LostItemSlotModel>
{
	[InjectModel]
	private LostItemSlotModel _model;

	[View]
	private LostItemSlotView _view;

	protected override LostItemSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(LostItemSlotModel model)
	{
		InitSlot(model);
		_view.GoodsSlot.Inject(new InventoryGoodsSlotModel(model.LostGoods.Goods));
		bool flag = model.Mode == ELostItemScrollerMode.SelectedOnly;
		_view.Gold.SetActive(flag && !model.IsArtifact);
		_view.Coin.SetActive(flag && model.IsArtifact);
		if (flag)
		{
			(model.IsArtifact ? _view.Coin_Value : _view.Gold_Value).GetComponent<ValueTween>().SetValue(model.Price, applyImmediately: true);
		}
		_view.Text.text = model.LostGoods.Goods.GetName();
		_view.DayCount.text = string.Format(TLocalizedText.LostGoodsActionPointLeft.LocalText, model.LostGoods.ExpireTurn.ToString());
	}
}
