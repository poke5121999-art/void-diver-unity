using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentTooltipView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject baseKeyGuideText;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject artifactCorruption;

	[SerializeField]
	private TextMeshProUGUI artifactCorruption_Caption;

	[SerializeField]
	private TextMeshProUGUI artifactCorruption_Value;

	[SerializeField]
	private GameObject artifactPrice;

	[SerializeField]
	private GameObject artifactPrice_Text;

	[SerializeField]
	private GameObject artifactPrice_Text_Gold;

	[SerializeField]
	private TextMeshProUGUI artifactPrice_Text_Gold_Value;

	[SerializeField]
	private TextMeshProUGUI artifactPrice_Text_Value;

	[SerializeField]
	private GameObject artifactTagGroup;

	[SerializeField]
	private GameObject[] artifactTagGroup_Slots;

	[SerializeField]
	private GameObject attack;

	[SerializeField]
	private TextMeshProUGUI attack_Value;

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
	private GameObject[] effectGroup_Skills;

	[SerializeField]
	private GameObject[] effectGroup_Stats;

	[SerializeField]
	private GameObject elemental;

	[SerializeField]
	private TextMeshProUGUI elemental_Value;

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
	private GameObject stashAmount;

	[SerializeField]
	private TextMeshProUGUI stashAmount_Value;

	[SerializeField]
	private Image title;

	[SerializeField]
	private TextMeshProUGUI title_LevelText;

	[SerializeField]
	private Image title_Mark;

	[SerializeField]
	private TextMeshProUGUI title_Name;

	[SerializeField]
	private Image title_TypeIcon;

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
	private GameObject dealArtifact;

	[SerializeField]
	private GameObject dealArtifact_Body;

	[SerializeField]
	private GameObject dealArtifact_SuggestPrice;

	[SerializeField]
	private GameObject dealArtifact_SuggestPrice_Text;

	[SerializeField]
	private TextMeshProUGUI dealArtifact_SuggestPrice_Text_Value;

	[SerializeField]
	private GameObject sellArtifact;

	[SerializeField]
	private GameObject sellArtifact_Body;

	[SerializeField]
	private GameObject sellArtifact_SuggestPrice;

	[SerializeField]
	private GameObject sellArtifact_SuggestPrice_Text;

	[SerializeField]
	private TextMeshProUGUI sellArtifact_SuggestPrice_Text_Value;

	[SerializeField]
	private Image image;

	public GameObject Contents => null;

	public GameObject BaseKeyGuideText => null;

	public GameObject Body => null;

	public GameObject ArtifactCorruption => null;

	public TextMeshProUGUI ArtifactCorruption_Caption => null;

	public TextMeshProUGUI ArtifactCorruption_Value => null;

	public GameObject ArtifactPrice => null;

	public GameObject ArtifactPrice_Text => null;

	public GameObject ArtifactPrice_Text_Gold => null;

	public TextMeshProUGUI ArtifactPrice_Text_Gold_Value => null;

	public TextMeshProUGUI ArtifactPrice_Text_Value => null;

	public GameObject ArtifactTagGroup => null;

	public GameObject[] ArtifactTagGroup_Slots => null;

	public GameObject Attack => null;

	public TextMeshProUGUI Attack_Value => null;

	public GameObject CharacterSpecificCanEquip => null;

	public Image CharacterSpecificCanEquip_Line => null;

	public TextMeshProUGUI CharacterSpecificCanEquip_Text => null;

	public GameObject CharacterSpecificCannotEquip => null;

	public Image CharacterSpecificCannotEquip_Line => null;

	public TextMeshProUGUI CharacterSpecificCannotEquip_Text => null;

	public GameObject Description => null;

	public TextMeshProUGUI Description_Text => null;

	public GameObject Durability => null;

	public TextMeshProUGUI Durability_Value => null;

	public GameObject EffectGroup => null;

	public GameObject[] EffectGroup_Skills => null;

	public GameObject[] EffectGroup_Stats => null;

	public GameObject Elemental => null;

	public TextMeshProUGUI Elemental_Value => null;

	public Image NewTag => null;

	public Image Owned => null;

	public GameObject Price => null;

	public GameObject Price_GoldText => null;

	public Image Price_GoldText_Icon => null;

	public TextMeshProUGUI Price_GoldText_Value => null;

	public GameObject StashAmount => null;

	public TextMeshProUGUI StashAmount_Value => null;

	public Image Title => null;

	public TextMeshProUGUI Title_LevelText => null;

	public Image Title_Mark => null;

	public TextMeshProUGUI Title_Name => null;

	public Image Title_TypeIcon => null;

	public GameObject Worth => null;

	public GameObject Worth_Text => null;

	public Image Worth_Text_Icon => null;

	public TextMeshProUGUI Worth_Text_Value => null;

	public GameObject Frame => null;

	public GameObject BaseFrame => null;

	public GameObject DealArtifact => null;

	public GameObject DealArtifact_Body => null;

	public GameObject DealArtifact_SuggestPrice => null;

	public GameObject DealArtifact_SuggestPrice_Text => null;

	public TextMeshProUGUI DealArtifact_SuggestPrice_Text_Value => null;

	public GameObject SellArtifact => null;

	public GameObject SellArtifact_Body => null;

	public GameObject SellArtifact_SuggestPrice => null;

	public GameObject SellArtifact_SuggestPrice_Text => null;

	public TextMeshProUGUI SellArtifact_SuggestPrice_Text_Value => null;

	public Image Image => null;
}
