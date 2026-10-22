using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanelView : View
{
	[SerializeField]
	private GameObject chaInfo;

	[SerializeField]
	private TextMeshProUGUI chaInfo_ChaName;

	[SerializeField]
	private TextMeshProUGUI chaInfo_ChaSubTitle;

	[SerializeField]
	private Image chaInfo_Line;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject atk;

	[SerializeField]
	private TextMeshProUGUI atk_Value;

	[SerializeField]
	private GameObject attackSpeed;

	[SerializeField]
	private TextMeshProUGUI attackSpeed_Value;

	[SerializeField]
	private GameObject backAttackDamageBoost;

	[SerializeField]
	private TextMeshProUGUI backAttackDamageBoost_Value;

	[SerializeField]
	private TextMeshProUGUI characterName;

	[SerializeField]
	private GameObject cooldownReduction;

	[SerializeField]
	private TextMeshProUGUI cooldownReduction_Value;

	[SerializeField]
	private GameObject criticalChance;

	[SerializeField]
	private TextMeshProUGUI criticalChance_Value;

	[SerializeField]
	private GameObject criticalDamageBoost;

	[SerializeField]
	private TextMeshProUGUI criticalDamageBoost_Value;

	[SerializeField]
	private GameObject damageReduction;

	[SerializeField]
	private TextMeshProUGUI damageReduction_Value;

	[SerializeField]
	private GameObject def;

	[SerializeField]
	private TextMeshProUGUI def_Value;

	[SerializeField]
	private GameObject defensePenetration;

	[SerializeField]
	private TextMeshProUGUI defensePenetration_Value;

	[SerializeField]
	private GameObject fireResistance;

	[SerializeField]
	private TextMeshProUGUI fireResistance_Value;

	[SerializeField]
	private GameObject hp;

	[SerializeField]
	private TextMeshProUGUI hp_Value;

	[SerializeField]
	private GameObject moveSpeed;

	[SerializeField]
	private TextMeshProUGUI moveSpeed_Value;

	[SerializeField]
	private GameObject naturalRecoverStamina;

	[SerializeField]
	private TextMeshProUGUI naturalRecoverStamina_Value;

	[SerializeField]
	private GameObject naturalRecoveryHp;

	[SerializeField]
	private TextMeshProUGUI naturalRecoveryHp_Value;

	[SerializeField]
	private GameObject stamina;

	[SerializeField]
	private TextMeshProUGUI stamina_Value;

	[SerializeField]
	private GameObject stressDamageResistance;

	[SerializeField]
	private TextMeshProUGUI stressDamageResistance_Value;

	[SerializeField]
	private Toggle toggle;

	[SerializeField]
	private GameObject waterResistance;

	[SerializeField]
	private TextMeshProUGUI waterResistance_Value;

	[SerializeField]
	private GameObject weaknessDamageBoost;

	[SerializeField]
	private TextMeshProUGUI weaknessDamageBoost_Value;

	[SerializeField]
	private GameObject windResistance;

	[SerializeField]
	private TextMeshProUGUI windResistance_Value;

	[SerializeField]
	private Image locked;

	[SerializeField]
	private GameObject placeHolder;

	[SerializeField]
	private Image characterImage;

	[SerializeField]
	private Image imgFloor;

	[SerializeField]
	private Image shadow;

	[SerializeField]
	private SkeletonGraphic spine;

	[SerializeField]
	private GameObject slotGroup;

	[SerializeField]
	private GameObject[] accessorySlots;

	[SerializeField]
	private GameObject[] artifactSlots;

	[SerializeField]
	private InventoryGoodsSlotPresenter weaponSlot;

	public GameObject ChaInfo => null;

	public TextMeshProUGUI ChaInfo_ChaName => null;

	public TextMeshProUGUI ChaInfo_ChaSubTitle => null;

	public Image ChaInfo_Line => null;

	public GameObject Info => null;

	public GameObject Atk => null;

	public TextMeshProUGUI Atk_Value => null;

	public GameObject AttackSpeed => null;

	public TextMeshProUGUI AttackSpeed_Value => null;

	public GameObject BackAttackDamageBoost => null;

	public TextMeshProUGUI BackAttackDamageBoost_Value => null;

	public TextMeshProUGUI CharacterName => null;

	public GameObject CooldownReduction => null;

	public TextMeshProUGUI CooldownReduction_Value => null;

	public GameObject CriticalChance => null;

	public TextMeshProUGUI CriticalChance_Value => null;

	public GameObject CriticalDamageBoost => null;

	public TextMeshProUGUI CriticalDamageBoost_Value => null;

	public GameObject DamageReduction => null;

	public TextMeshProUGUI DamageReduction_Value => null;

	public GameObject Def => null;

	public TextMeshProUGUI Def_Value => null;

	public GameObject DefensePenetration => null;

	public TextMeshProUGUI DefensePenetration_Value => null;

	public GameObject FireResistance => null;

	public TextMeshProUGUI FireResistance_Value => null;

	public GameObject Hp => null;

	public TextMeshProUGUI Hp_Value => null;

	public GameObject MoveSpeed => null;

	public TextMeshProUGUI MoveSpeed_Value => null;

	public GameObject NaturalRecoverStamina => null;

	public TextMeshProUGUI NaturalRecoverStamina_Value => null;

	public GameObject NaturalRecoveryHp => null;

	public TextMeshProUGUI NaturalRecoveryHp_Value => null;

	public GameObject Stamina => null;

	public TextMeshProUGUI Stamina_Value => null;

	public GameObject StressDamageResistance => null;

	public TextMeshProUGUI StressDamageResistance_Value => null;

	public Toggle Toggle => null;

	public GameObject WaterResistance => null;

	public TextMeshProUGUI WaterResistance_Value => null;

	public GameObject WeaknessDamageBoost => null;

	public TextMeshProUGUI WeaknessDamageBoost_Value => null;

	public GameObject WindResistance => null;

	public TextMeshProUGUI WindResistance_Value => null;

	public Image Locked => null;

	public GameObject PlaceHolder => null;

	public Image CharacterImage => null;

	public Image ImgFloor => null;

	public Image Shadow => null;

	public SkeletonGraphic Spine => null;

	public GameObject SlotGroup => null;

	public GameObject[] AccessorySlots => null;

	public GameObject[] ArtifactSlots => null;

	public InventoryGoodsSlotPresenter WeaponSlot => null;
}
