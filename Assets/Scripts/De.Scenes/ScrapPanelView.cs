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

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public TextMeshProUGUI GuideText => guideText;

	public TextMeshProUGUI InvalidText => invalidText;

	public GameObject ResultBox => resultBox;

	public GameObject ResultBox_Content => resultBox_Content;

	public GameObject[] ResultBox_Slots => resultBox_Slots;

	public GameObject ResultBox_Top => resultBox_Top;

	public GameObject ScrapBox => scrapBox;

	public GameObject ScrapBox_Content => scrapBox_Content;

	public Button ScrapBox_Button => scrapBox_Button;

	public InventoryGoodsSlotPresenter ScrapBox_GoodsSlot => scrapBox_GoodsSlot;

	public GameObject ScrapBox_Top => scrapBox_Top;
}
