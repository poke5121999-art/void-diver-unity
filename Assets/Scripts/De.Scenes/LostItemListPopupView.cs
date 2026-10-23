using NemoLib.Mvp;
using UnityEngine;

public class LostItemListPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private LostItemScrollerPresenter scroller;

	[SerializeField]
	private GameObject top;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Info => info;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public LostItemScrollerPresenter Scroller => scroller;

	public GameObject Top => top;
}
