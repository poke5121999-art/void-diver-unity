using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInfoPanelView : View
{
	[SerializeField]
	private Button backButton;

	[SerializeField]
	private GameObject characterSpecificCanEquip;

	[SerializeField]
	private Image characterSpecificCanEquip_Line;

	[SerializeField]
	private TextMeshProUGUI characterSpecificCanEquip_Text;

	[SerializeField]
	private GameObject characterSpecificCannotEquip;

	[SerializeField]
	private Image characterSpecificCannotEquip_Line;

	[SerializeField]
	private TextMeshProUGUI characterSpecificCannotEquip_Text;

	[SerializeField]
	private Image scroll;

	[SerializeField]
	private Image viewport;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject artifactCorruption;

	[SerializeField]
	private TextMeshProUGUI artifactCorruption_Caption;

	[SerializeField]
	private TextMeshProUGUI artifactCorruption_Value;

	[SerializeField]
	private GameObject artifactPrice;

	[SerializeField]
	private GameObject artifactPrice_GoldText;

	[SerializeField]
	private Image artifactPrice_GoldText_Icon;

	[SerializeField]
	private TextMeshProUGUI artifactPrice_GoldText_Value;

	[SerializeField]
	private GameObject artifactTagGroup;

	[SerializeField]
	private GameObject[] artifactTagGroup_Slots;

	[SerializeField]
	private GameObject attack;

	[SerializeField]
	private TextMeshProUGUI attack_Value;

	[SerializeField]
	private GameObject description;

	[SerializeField]
	private TextMeshProUGUI description_Text;

	[SerializeField]
	private GameObject durability;

	[SerializeField]
	private TextMeshProUGUI durability_Value;

	[SerializeField]
	private GameObject effectGroup;

	[SerializeField]
	private GameObject[] effectGroup_ArtifactsStats;

	[SerializeField]
	private GameObject[] effectGroup_Skills;

	[SerializeField]
	private GameObject elemental;

	[SerializeField]
	private TextMeshProUGUI elemental_Value;

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
	private Image title;

	[SerializeField]
	private TextMeshProUGUI title_LevelText;

	[SerializeField]
	private TextMeshProUGUI title_Name;

	[SerializeField]
	private Image title_TypeIcon;

	public Button BackButton => backButton;

	public GameObject CharacterSpecificCanEquip => characterSpecificCanEquip;

	public Image CharacterSpecificCanEquip_Line => characterSpecificCanEquip_Line;

	public TextMeshProUGUI CharacterSpecificCanEquip_Text => characterSpecificCanEquip_Text;

	public GameObject CharacterSpecificCannotEquip => characterSpecificCannotEquip;

	public Image CharacterSpecificCannotEquip_Line => characterSpecificCannotEquip_Line;

	public TextMeshProUGUI CharacterSpecificCannotEquip_Text => characterSpecificCannotEquip_Text;

	public Image Scroll => scroll;

	public Image Viewport => viewport;

	public GameObject Content => content;

	public GameObject ArtifactCorruption => artifactCorruption;

	public TextMeshProUGUI ArtifactCorruption_Caption => artifactCorruption_Caption;

	public TextMeshProUGUI ArtifactCorruption_Value => artifactCorruption_Value;

	public GameObject ArtifactPrice => artifactPrice;

	public GameObject ArtifactPrice_GoldText => artifactPrice_GoldText;

	public Image ArtifactPrice_GoldText_Icon => artifactPrice_GoldText_Icon;

	public TextMeshProUGUI ArtifactPrice_GoldText_Value => artifactPrice_GoldText_Value;

	public GameObject ArtifactTagGroup => artifactTagGroup;

	public GameObject[] ArtifactTagGroup_Slots => artifactTagGroup_Slots;

	public GameObject Attack => attack;

	public TextMeshProUGUI Attack_Value => attack_Value;

	public GameObject Description => description;

	public TextMeshProUGUI Description_Text => description_Text;

	public GameObject Durability => durability;

	public TextMeshProUGUI Durability_Value => durability_Value;

	public GameObject EffectGroup => effectGroup;

	public GameObject[] EffectGroup_ArtifactsStats => effectGroup_ArtifactsStats;

	public GameObject[] EffectGroup_Skills => effectGroup_Skills;

	public GameObject Elemental => elemental;

	public TextMeshProUGUI Elemental_Value => elemental_Value;

	public GameObject Price => price;

	public GameObject Price_GoldText => price_GoldText;

	public Image Price_GoldText_Icon => price_GoldText_Icon;

	public TextMeshProUGUI Price_GoldText_Value => price_GoldText_Value;

	public GameObject StashAmount => stashAmount;

	public TextMeshProUGUI StashAmount_Value => stashAmount_Value;

	public Image Title => title;

	public TextMeshProUGUI Title_LevelText => title_LevelText;

	public TextMeshProUGUI Title_Name => title_Name;

	public Image Title_TypeIcon => title_TypeIcon;
}
