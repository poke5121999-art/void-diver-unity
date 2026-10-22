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

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public GameObject List => list;

	public ShopProductScrollerPresenter Scroller => scroller;
}
