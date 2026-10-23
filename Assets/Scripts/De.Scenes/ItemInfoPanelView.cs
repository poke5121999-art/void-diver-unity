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

	public Button BackButton => backButton;

	public Image NewTag => newTag;

	public Image Owned => owned;

	public GameObject RecordComplete => recordComplete;

	public Image Scroll => scroll;

	public Image Viewport => viewport;

	public GameObject Content => content;

	public GameObject Description => description;

	public TextMeshProUGUI Description_Text => description_Text;

	public GameObject InventoryAmount => inventoryAmount;

	public TextMeshProUGUI InventoryAmount_Value => inventoryAmount_Value;

	public GameObject Price => price;

	public GameObject Price_GoldText => price_GoldText;

	public Image Price_GoldText_Icon => price_GoldText_Icon;

	public TextMeshProUGUI Price_GoldText_Value => price_GoldText_Value;

	public GameObject StashAmount => stashAmount;

	public TextMeshProUGUI StashAmount_Value => stashAmount_Value;

	public GameObject Usage => usage;

	public TextMeshProUGUI Usage_Text => usage_Text;

	public GameObject Worth => worth;

	public GameObject Worth_Text => worth_Text;

	public Image Worth_Text_Icon => worth_Text_Icon;

	public TextMeshProUGUI Worth_Text_Value => worth_Text_Value;

	public Button ShowNoteButton => showNoteButton;

	public Image Title => title;

	public TextMeshProUGUI Title_GradeText => title_GradeText;

	public TextMeshProUGUI Title_Name => title_Name;

	public Image Title_TypeIcon => title_TypeIcon;
}
