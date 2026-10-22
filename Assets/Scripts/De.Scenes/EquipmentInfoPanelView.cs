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

	public Button BackButton => null;

	public GameObject CharacterSpecificCanEquip => null;

	public Image CharacterSpecificCanEquip_Line => null;

	public TextMeshProUGUI CharacterSpecificCanEquip_Text => null;

	public GameObject CharacterSpecificCannotEquip => null;

	public Image CharacterSpecificCannotEquip_Line => null;

	public TextMeshProUGUI CharacterSpecificCannotEquip_Text => null;

	public Image Scroll => null;

	public Image Viewport => null;

	public GameObject Content => null;

	public GameObject ArtifactCorruption => null;

	public TextMeshProUGUI ArtifactCorruption_Caption => null;

	public TextMeshProUGUI ArtifactCorruption_Value => null;

	public GameObject ArtifactPrice => null;

	public GameObject ArtifactPrice_GoldText => null;

	public Image ArtifactPrice_GoldText_Icon => null;

	public TextMeshProUGUI ArtifactPrice_GoldText_Value => null;

	public GameObject ArtifactTagGroup => null;

	public GameObject[] ArtifactTagGroup_Slots => null;

	public GameObject Attack => null;

	public TextMeshProUGUI Attack_Value => null;

	public GameObject Description => null;

	public TextMeshProUGUI Description_Text => null;

	public GameObject Durability => null;

	public TextMeshProUGUI Durability_Value => null;

	public GameObject EffectGroup => null;

	public GameObject[] EffectGroup_ArtifactsStats => null;

	public GameObject[] EffectGroup_Skills => null;

	public GameObject Elemental => null;

	public TextMeshProUGUI Elemental_Value => null;

	public GameObject Price => null;

	public GameObject Price_GoldText => null;

	public Image Price_GoldText_Icon => null;

	public TextMeshProUGUI Price_GoldText_Value => null;

	public GameObject StashAmount => null;

	public TextMeshProUGUI StashAmount_Value => null;

	public Image Title => null;

	public TextMeshProUGUI Title_LevelText => null;

	public TextMeshProUGUI Title_Name => null;

	public Image Title_TypeIcon => null;
}
