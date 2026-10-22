using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotSettingPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject[] goodsSlots;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Image bG;

	public GameObject Content => null;

	public GameObject Body => null;

	public GameObject[] GoodsSlots => null;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public GameObject Top => null;

	public Image BG => null;
}
