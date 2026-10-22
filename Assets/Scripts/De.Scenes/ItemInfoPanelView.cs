using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanelView : View
{
	[SerializeField]
	private Button backButton;

	[SerializeField]
	private Image newTag;

	[SerializeField]
	private Image owned;

	[SerializeField]
	private GameObject recordComplete;

	[SerializeField]
	private Image scroll;

	[SerializeField]
	private Image viewport;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject description;

	[SerializeField]
	private TextMeshProUGUI description_Text;

	[SerializeField]
	private GameObject inventoryAmount;

	[SerializeField]
	private TextMeshProUGUI inventoryAmount_Value;

	[SerializeField]
	private GameObject price;

	[SerializeField]
	private GameObject price_GoldText;

	[SerializeField]
	private Image price_GoldText_Icon;

	[SerializeField]
	private TextMeshProUGUI price_GoldText_Value;

	[SerializeField]
	private GameObject stashAmount;

	[SerializeField]
	private TextMeshProUGUI stashAmount_Value;

	[SerializeField]
	private GameObject usage;

	[SerializeField]
	private TextMeshProUGUI usage_Text;

	[SerializeField]
	private GameObject worth;

	[SerializeField]
	private GameObject worth_Text;

	[SerializeField]
	private Image worth_Text_Icon;

	[SerializeField]
	private TextMeshProUGUI worth_Text_Value;

	[SerializeField]
	private Button showNoteButton;

	[SerializeField]
	private Image title;

	[SerializeField]
	private TextMeshProUGUI title_GradeText;

	[SerializeField]
	private TextMeshProUGUI title_Name;

	[SerializeField]
	private Image title_TypeIcon;

	public Button BackButton => null;

	public Image NewTag => null;

	public Image Owned => null;

	public GameObject RecordComplete => null;

	public Image Scroll => null;

	public Image Viewport => null;

	public GameObject Content => null;

	public GameObject Description => null;

	public TextMeshProUGUI Description_Text => null;

	public GameObject InventoryAmount => null;

	public TextMeshProUGUI InventoryAmount_Value => null;

	public GameObject Price => null;

	public GameObject Price_GoldText => null;

	public Image Price_GoldText_Icon => null;

	public TextMeshProUGUI Price_GoldText_Value => null;

	public GameObject StashAmount => null;

	public TextMeshProUGUI StashAmount_Value => null;

	public GameObject Usage => null;

	public TextMeshProUGUI Usage_Text => null;

	public GameObject Worth => null;

	public GameObject Worth_Text => null;

	public Image Worth_Text_Icon => null;

	public TextMeshProUGUI Worth_Text_Value => null;

	public Button ShowNoteButton => null;

	public Image Title => null;

	public TextMeshProUGUI Title_GradeText => null;

	public TextMeshProUGUI Title_Name => null;

	public Image Title_TypeIcon => null;
}
