using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject baseKeyGuideText;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject description;

	[SerializeField]
	private TextMeshProUGUI description_Text;

	[SerializeField]
	private GameObject inventoryAmount;

	[SerializeField]
	private TextMeshProUGUI inventoryAmount_Value;

	[SerializeField]
	private Image newTag;

	[SerializeField]
	private Image owned;

	[SerializeField]
	private GameObject price;

	[SerializeField]
	private GameObject price_GoldText;

	[SerializeField]
	private Image price_GoldText_Icon;

	[SerializeField]
	private TextMeshProUGUI price_GoldText_Value;

	[SerializeField]
	private GameObject recordComplete;

	[SerializeField]
	private GameObject stashAmount;

	[SerializeField]
	private TextMeshProUGUI stashAmount_Value;

	[SerializeField]
	private Image title;

	[SerializeField]
	private TextMeshProUGUI title_GradeText;

	[SerializeField]
	private Image title_Mark;

	[SerializeField]
	private TextMeshProUGUI title_Name;

	[SerializeField]
	private Image title_TypeIcon;

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
	private GameObject frame;

	[SerializeField]
	private GameObject baseFrame;

	[SerializeField]
	private GameObject suggest;

	[SerializeField]
	private GameObject suggest_Body;

	[SerializeField]
	private GameObject suggest_SuggestPrice;

	[SerializeField]
	private GameObject suggest_SuggestPrice_GoldText;

	[SerializeField]
	private Image suggest_SuggestPrice_GoldText_Icon;

	[SerializeField]
	private TextMeshProUGUI suggest_SuggestPrice_GoldText_Value;

	[SerializeField]
	private GameObject suggest_Bottom;

	[SerializeField]
	private GameObject suggest_Back;

	[SerializeField]
	private Button suggest_Back_Button;

	[SerializeField]
	private GameObject suggest_Deal;

	[SerializeField]
	private Button suggest_Deal_Button;

	[SerializeField]
	private Image image;

	public GameObject Contents => null;

	public GameObject BaseKeyGuideText => null;

	public GameObject Body => null;

	public GameObject Description => null;

	public TextMeshProUGUI Description_Text => null;

	public GameObject InventoryAmount => null;

	public TextMeshProUGUI InventoryAmount_Value => null;

	public Image NewTag => null;

	public Image Owned => null;

	public GameObject Price => null;

	public GameObject Price_GoldText => null;

	public Image Price_GoldText_Icon => null;

	public TextMeshProUGUI Price_GoldText_Value => null;

	public GameObject RecordComplete => null;

	public GameObject StashAmount => null;

	public TextMeshProUGUI StashAmount_Value => null;

	public Image Title => null;

	public TextMeshProUGUI Title_GradeText => null;

	public Image Title_Mark => null;

	public TextMeshProUGUI Title_Name => null;

	public Image Title_TypeIcon => null;

	public GameObject Usage => null;

	public TextMeshProUGUI Usage_Text => null;

	public GameObject Worth => null;

	public GameObject Worth_Text => null;

	public Image Worth_Text_Icon => null;

	public TextMeshProUGUI Worth_Text_Value => null;

	public GameObject Frame => null;

	public GameObject BaseFrame => null;

	public GameObject Suggest => null;

	public GameObject Suggest_Body => null;

	public GameObject Suggest_SuggestPrice => null;

	public GameObject Suggest_SuggestPrice_GoldText => null;

	public Image Suggest_SuggestPrice_GoldText_Icon => null;

	public TextMeshProUGUI Suggest_SuggestPrice_GoldText_Value => null;

	public GameObject Suggest_Bottom => null;

	public GameObject Suggest_Back => null;

	public Button Suggest_Back_Button => null;

	public GameObject Suggest_Deal => null;

	public Button Suggest_Deal_Button => null;

	public Image Image => null;
}
