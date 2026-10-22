using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootingInventoryPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject inventory;

	[SerializeField]
	private GameObject[] slots;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private TextMeshProUGUI caption;

	[SerializeField]
	private TextMeshProUGUI goodsCountText;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	public GameObject Content => null;

	public GameObject Inventory => null;

	public GameObject[] Slots => null;

	public GameObject Top => null;

	public Image BG => null;

	public TextMeshProUGUI Caption => null;

	public TextMeshProUGUI GoodsCountText => null;

	public GoodsTooltipPresenter GoodsTooltip => null;
}
