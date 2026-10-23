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

	public GameObject Content => content;

	public GameObject Corruption => corruption;

	public GameObject Corruption_Normal => corruption_Normal;

	public Image Corruption_Normal_Bg => corruption_Normal_Bg;

	public Image Corruption_Normal_Line => corruption_Normal_Line;

	public TextMeshProUGUI Corruption_Normal_Text => corruption_Normal_Text;

	public GameObject Corruption_Normal_Value => corruption_Normal_Value;

	public GameObject[] Corruption_Normal_Value_Current => corruption_Normal_Value_Current;

	public GameObject[] Corruption_Normal_Value_Icon => corruption_Normal_Value_Icon;

	public TextMeshProUGUI Corruption_Normal_Value_Max => corruption_Normal_Value_Max;

	public EquipmentInventoryPanelPresenter EquipmentInventoryPanel => equipmentInventoryPanel;

	public GameObject Inventory => inventory;

	public EmptyPanelPresenter EmptyPanel => emptyPanel;

	public GameObject Page => page;

	public Image Page_Line => page_Line;

	public Button Page_NextButton => page_NextButton;

	public GameObject[] Page_PagenationCells => page_PagenationCells;

	public Button Page_PrevButton => page_PrevButton;

	public PlayerInventoryScrollerPresenter Scroller => scroller;

	public SafeInventoryPanelPresenter SafeInventoryPanel => safeInventoryPanel;

	public GameObject Top => top;

	public Image BG => bG;

	public TextMeshProUGUI Caption => caption;

	public TextMeshProUGUI GoodsCountText => goodsCountText;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;
}
