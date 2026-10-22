using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelView : View
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
	private GameObject frame;

	[SerializeField]
	private GameObject baseFrame;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private TextMeshProUGUI title;

	public GameObject Content => content;

	public GameObject Body => body;

	public GameObject[] GoodsSlots => goodsSlots;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public GameObject Frame => frame;

	public GameObject BaseFrame => baseFrame;

	public GameObject Top => top;

	public Image BG => bG;

	public TextMeshProUGUI Title => title;
}
