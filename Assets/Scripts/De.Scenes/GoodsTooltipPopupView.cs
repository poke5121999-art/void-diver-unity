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

	public Button Background => null;

	public GameObject Contents => null;

	public GoodsTooltipPresenter GoodsTooltip => null;
}
