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

	public GameObject Contents => contents;

	public GameObject BaseKeyGuideText => baseKeyGuideText;

	public GameObject Body => body;

	public GameObject Description => description;

	public TextMeshProUGUI Description_Text => description_Text;

	public GameObject InventoryAmount => inventoryAmount;

	public TextMeshProUGUI InventoryAmount_Value => inventoryAmount_Value;

	public Image NewTag => newTag;

	public Image Owned => owned;

	public GameObject Price => price;

	public GameObject Price_GoldText => price_GoldText;

	public Image Price_GoldText_Icon => price_GoldText_Icon;

	public TextMeshProUGUI Price_GoldText_Value => price_GoldText_Value;

	public GameObject RecordComplete => recordComplete;

	public GameObject StashAmount => stashAmount;

	public TextMeshProUGUI StashAmount_Value => stashAmount_Value;

	public Image Title => title;

	public TextMeshProUGUI Title_GradeText => title_GradeText;

	public Image Title_Mark => title_Mark;

	public TextMeshProUGUI Title_Name => title_Name;

	public Image Title_TypeIcon => title_TypeIcon;

	public GameObject Usage => usage;

	public TextMeshProUGUI Usage_Text => usage_Text;

	public GameObject Worth => worth;

	public GameObject Worth_Text => worth_Text;

	public Image Worth_Text_Icon => worth_Text_Icon;

	public TextMeshProUGUI Worth_Text_Value => worth_Text_Value;

	public GameObject Frame => frame;

	public GameObject BaseFrame => baseFrame;

	public GameObject Suggest => suggest;

	public GameObject Suggest_Body => suggest_Body;

	public GameObject Suggest_SuggestPrice => suggest_SuggestPrice;

	public GameObject Suggest_SuggestPrice_GoldText => suggest_SuggestPrice_GoldText;

	public Image Suggest_SuggestPrice_GoldText_Icon => suggest_SuggestPrice_GoldText_Icon;

	public TextMeshProUGUI Suggest_SuggestPrice_GoldText_Value => suggest_SuggestPrice_GoldText_Value;

	public GameObject Suggest_Bottom => suggest_Bottom;

	public GameObject Suggest_Back => suggest_Back;

	public Button Suggest_Back_Button => suggest_Back_Button;

	public GameObject Suggest_Deal => suggest_Deal;

	public Button Suggest_Deal_Button => suggest_Deal_Button;

	public Image Image => image;
}
