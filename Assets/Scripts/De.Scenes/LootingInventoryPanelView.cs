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

	public GameObject Content => content;

	public GameObject Inventory => inventory;

	public GameObject[] Slots => slots;

	public GameObject Top => top;

	public Image BG => bG;

	public TextMeshProUGUI Caption => caption;

	public TextMeshProUGUI GoodsCountText => goodsCountText;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;
}
