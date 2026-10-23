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

	public GameObject Content => content;

	public GameObject Body => body;

	public GameObject[] GoodsSlots => goodsSlots;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public GameObject Top => top;

	public Image BG => bG;
}
