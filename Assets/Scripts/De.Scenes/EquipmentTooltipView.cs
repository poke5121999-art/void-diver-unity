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

	public GameObject Contents => contents;

	public GameObject BaseKeyGuideText => baseKeyGuideText;

	public GameObject Body => body;

	public GameObject ArtifactCorruption => artifactCorruption;

	public TextMeshProUGUI ArtifactCorruption_Caption => artifactCorruption_Caption;

	public TextMeshProUGUI ArtifactCorruption_Value => artifactCorruption_Value;

	public GameObject ArtifactPrice => artifactPrice;

	public GameObject ArtifactPrice_Text => artifactPrice_Text;

	public GameObject ArtifactPrice_Text_Gold => artifactPrice_Text_Gold;

	public TextMeshProUGUI ArtifactPrice_Text_Gold_Value => artifactPrice_Text_Gold_Value;

	public TextMeshProUGUI ArtifactPrice_Text_Value => artifactPrice_Text_Value;

	public GameObject ArtifactTagGroup => artifactTagGroup;

	public GameObject[] ArtifactTagGroup_Slots => artifactTagGroup_Slots;

	public GameObject Attack => attack;

	public TextMeshProUGUI Attack_Value => attack_Value;

	public GameObject CharacterSpecificCanEquip => characterSpecificCanEquip;

	public Image CharacterSpecificCanEquip_Line => characterSpecificCanEquip_Line;

	public TextMeshProUGUI CharacterSpecificCanEquip_Text => characterSpecificCanEquip_Text;

	public GameObject CharacterSpecificCannotEquip => characterSpecificCannotEquip;

	public Image CharacterSpecificCannotEquip_Line => characterSpecificCannotEquip_Line;

	public TextMeshProUGUI CharacterSpecificCannotEquip_Text => characterSpecificCannotEquip_Text;

	public GameObject Description => description;

	public TextMeshProUGUI Description_Text => description_Text;

	public GameObject Durability => durability;

	public TextMeshProUGUI Durability_Value => durability_Value;

	public GameObject EffectGroup => effectGroup;

	public GameObject[] EffectGroup_Skills => effectGroup_Skills;

	public GameObject[] EffectGroup_Stats => effectGroup_Stats;

	public GameObject Elemental => elemental;

	public TextMeshProUGUI Elemental_Value => elemental_Value;

	public Image NewTag => newTag;

	public Image Owned => owned;

	public GameObject Price => price;

	public GameObject Price_GoldText => price_GoldText;

	public Image Price_GoldText_Icon => price_GoldText_Icon;

	public TextMeshProUGUI Price_GoldText_Value => price_GoldText_Value;

	public GameObject StashAmount => stashAmount;

	public TextMeshProUGUI StashAmount_Value => stashAmount_Value;

	public Image Title => title;

	public TextMeshProUGUI Title_LevelText => title_LevelText;

	public Image Title_Mark => title_Mark;

	public TextMeshProUGUI Title_Name => title_Name;

	public Image Title_TypeIcon => title_TypeIcon;

	public GameObject Worth => worth;

	public GameObject Worth_Text => worth_Text;

	public Image Worth_Text_Icon => worth_Text_Icon;

	public TextMeshProUGUI Worth_Text_Value => worth_Text_Value;

	public GameObject Frame => frame;

	public GameObject BaseFrame => baseFrame;

	public GameObject DealArtifact => dealArtifact;

	public GameObject DealArtifact_Body => dealArtifact_Body;

	public GameObject DealArtifact_SuggestPrice => dealArtifact_SuggestPrice;

	public GameObject DealArtifact_SuggestPrice_Text => dealArtifact_SuggestPrice_Text;

	public TextMeshProUGUI DealArtifact_SuggestPrice_Text_Value => dealArtifact_SuggestPrice_Text_Value;

	public GameObject SellArtifact => sellArtifact;

	public GameObject SellArtifact_Body => sellArtifact_Body;

	public GameObject SellArtifact_SuggestPrice => sellArtifact_SuggestPrice;

	public GameObject SellArtifact_SuggestPrice_Text => sellArtifact_SuggestPrice_Text;

	public TextMeshProUGUI SellArtifact_SuggestPrice_Text_Value => sellArtifact_SuggestPrice_Text_Value;

	public Image Image => image;
}
