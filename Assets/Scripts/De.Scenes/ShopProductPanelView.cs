using NemoLib.Mvp;
using UnityEngine;

public class ShopProductPanelView : View
{
	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private GameObject list;

	[SerializeField]
	private ShopProductScrollerPresenter scroller;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public GameObject List => null;

	public ShopProductScrollerPresenter Scroller => null;
}
