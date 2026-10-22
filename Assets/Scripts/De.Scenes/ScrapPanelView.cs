using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrapPanelView : View
{
	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private TextMeshProUGUI guideText;

	[SerializeField]
	private TextMeshProUGUI invalidText;

	[SerializeField]
	private GameObject resultBox;

	[SerializeField]
	private GameObject resultBox_Content;

	[SerializeField]
	private GameObject[] resultBox_Slots;

	[SerializeField]
	private GameObject resultBox_Top;

	[SerializeField]
	private GameObject scrapBox;

	[SerializeField]
	private GameObject scrapBox_Content;

	[SerializeField]
	private Button scrapBox_Button;

	[SerializeField]
	private InventoryGoodsSlotPresenter scrapBox_GoodsSlot;

	[SerializeField]
	private GameObject scrapBox_Top;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public TextMeshProUGUI GuideText => null;

	public TextMeshProUGUI InvalidText => null;

	public GameObject ResultBox => null;

	public GameObject ResultBox_Content => null;

	public GameObject[] ResultBox_Slots => null;

	public GameObject ResultBox_Top => null;

	public GameObject ScrapBox => null;

	public GameObject ScrapBox_Content => null;

	public Button ScrapBox_Button => null;

	public InventoryGoodsSlotPresenter ScrapBox_GoodsSlot => null;

	public GameObject ScrapBox_Top => null;
}
