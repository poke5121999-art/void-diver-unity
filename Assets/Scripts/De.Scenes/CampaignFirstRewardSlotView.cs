using NemoLib.Mvp;
using UnityEngine;

public class CampaignFirstRewardSlotView : View
{
	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

	[SerializeField]
	private GameObject receipted;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public GameObject Receipted => null;
}
