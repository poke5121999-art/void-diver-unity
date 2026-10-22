using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StashPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private EmptyPanelPresenter emptyPanel;

	[SerializeField]
	private GameObject page;

	[SerializeField]
	private Image page_Line;

	[SerializeField]
	private Button page_NextButton;

	[SerializeField]
	private GameObject[] page_PagenationCells;

	[SerializeField]
	private Button page_PrevButton;

	[SerializeField]
	private StashScrollerPresenter scroller;

	[SerializeField]
	private StashFilterPanelPresenter stashFilterPanel;

	[SerializeField]
	private GameObject tItle;

	[SerializeField]
	private Image tItle_BG;

	[SerializeField]
	private TextMeshProUGUI tItle_GoodsCountText;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	public GameObject Content => content;

	public GameObject Body => body;

	public EmptyPanelPresenter EmptyPanel => emptyPanel;

	public GameObject Page => page;

	public Image Page_Line => page_Line;

	public Button Page_NextButton => page_NextButton;

	public GameObject[] Page_PagenationCells => page_PagenationCells;

	public Button Page_PrevButton => page_PrevButton;

	public StashScrollerPresenter Scroller => scroller;

	public StashFilterPanelPresenter StashFilterPanel => stashFilterPanel;

	public GameObject TItle => tItle;

	public Image TItle_BG => tItle_BG;

	public TextMeshProUGUI TItle_GoodsCountText => tItle_GoodsCountText;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;
}
