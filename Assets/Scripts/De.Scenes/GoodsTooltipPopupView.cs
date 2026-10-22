using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class GoodsTooltipPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	public Button Background => background;

	public GameObject Contents => contents;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;
}
