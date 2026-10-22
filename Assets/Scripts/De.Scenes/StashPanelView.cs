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

	public GameObject Content => null;

	public GameObject Body => null;

	public EmptyPanelPresenter EmptyPanel => null;

	public GameObject Page => null;

	public Image Page_Line => null;

	public Button Page_NextButton => null;

	public GameObject[] Page_PagenationCells => null;

	public Button Page_PrevButton => null;

	public StashScrollerPresenter Scroller => null;

	public StashFilterPanelPresenter StashFilterPanel => null;

	public GameObject TItle => null;

	public Image TItle_BG => null;

	public TextMeshProUGUI TItle_GoodsCountText => null;

	public GoodsTooltipPresenter GoodsTooltip => null;
}
