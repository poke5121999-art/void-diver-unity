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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Info => null;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public LostItemScrollerPresenter Scroller => null;

	public GameObject Top => null;
}
