using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject corruption;

	[SerializeField]
	private GameObject corruption_Normal;

	[SerializeField]
	private Image corruption_Normal_Bg;

	[SerializeField]
	private Image corruption_Normal_Line;

	[SerializeField]
	private TextMeshProUGUI corruption_Normal_Text;

	[SerializeField]
	private GameObject corruption_Normal_Value;

	[SerializeField]
	private GameObject[] corruption_Normal_Value_Current;

	[SerializeField]
	private GameObject[] corruption_Normal_Value_Icon;

	[SerializeField]
	private TextMeshProUGUI corruption_Normal_Value_Max;

	[SerializeField]
	private EquipmentInventoryPanelPresenter equipmentInventoryPanel;

	[SerializeField]
	private GameObject inventory;

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
	private PlayerInventoryScrollerPresenter scroller;

	[SerializeField]
	private SafeInventoryPanelPresenter safeInventoryPanel;

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

	public GameObject Corruption => null;

	public GameObject Corruption_Normal => null;

	public Image Corruption_Normal_Bg => null;

	public Image Corruption_Normal_Line => null;

	public TextMeshProUGUI Corruption_Normal_Text => null;

	public GameObject Corruption_Normal_Value => null;

	public GameObject[] Corruption_Normal_Value_Current => null;

	public GameObject[] Corruption_Normal_Value_Icon => null;

	public TextMeshProUGUI Corruption_Normal_Value_Max => null;

	public EquipmentInventoryPanelPresenter EquipmentInventoryPanel => null;

	public GameObject Inventory => null;

	public EmptyPanelPresenter EmptyPanel => null;

	public GameObject Page => null;

	public Image Page_Line => null;

	public Button Page_NextButton => null;

	public GameObject[] Page_PagenationCells => null;

	public Button Page_PrevButton => null;

	public PlayerInventoryScrollerPresenter Scroller => null;

	public SafeInventoryPanelPresenter SafeInventoryPanel => null;

	public GameObject Top => null;

	public Image BG => null;

	public TextMeshProUGUI Caption => null;

	public TextMeshProUGUI GoodsCountText => null;

	public GoodsTooltipPresenter GoodsTooltip => null;
}
