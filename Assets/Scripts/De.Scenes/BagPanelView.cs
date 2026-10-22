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

	public GameObject Content => null;

	public GameObject Body => null;

	public GameObject[] GoodsSlots => null;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public GameObject Frame => null;

	public GameObject BaseFrame => null;

	public GameObject Top => null;

	public Image BG => null;

	public TextMeshProUGUI Title => null;
}
