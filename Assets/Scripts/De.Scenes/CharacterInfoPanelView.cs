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

	public GameObject ChaInfo => chaInfo;

	public TextMeshProUGUI ChaInfo_ChaName => chaInfo_ChaName;

	public TextMeshProUGUI ChaInfo_ChaSubTitle => chaInfo_ChaSubTitle;

	public Image ChaInfo_Line => chaInfo_Line;

	public GameObject Info => info;

	public GameObject Atk => atk;

	public TextMeshProUGUI Atk_Value => atk_Value;

	public GameObject AttackSpeed => attackSpeed;

	public TextMeshProUGUI AttackSpeed_Value => attackSpeed_Value;

	public GameObject BackAttackDamageBoost => backAttackDamageBoost;

	public TextMeshProUGUI BackAttackDamageBoost_Value => backAttackDamageBoost_Value;

	public TextMeshProUGUI CharacterName => characterName;

	public GameObject CooldownReduction => cooldownReduction;

	public TextMeshProUGUI CooldownReduction_Value => cooldownReduction_Value;

	public GameObject CriticalChance => criticalChance;

	public TextMeshProUGUI CriticalChance_Value => criticalChance_Value;

	public GameObject CriticalDamageBoost => criticalDamageBoost;

	public TextMeshProUGUI CriticalDamageBoost_Value => criticalDamageBoost_Value;

	public GameObject DamageReduction => damageReduction;

	public TextMeshProUGUI DamageReduction_Value => damageReduction_Value;

	public GameObject Def => def;

	public TextMeshProUGUI Def_Value => def_Value;

	public GameObject DefensePenetration => defensePenetration;

	public TextMeshProUGUI DefensePenetration_Value => defensePenetration_Value;

	public GameObject FireResistance => fireResistance;

	public TextMeshProUGUI FireResistance_Value => fireResistance_Value;

	public GameObject Hp => hp;

	public TextMeshProUGUI Hp_Value => hp_Value;

	public GameObject MoveSpeed => moveSpeed;

	public TextMeshProUGUI MoveSpeed_Value => moveSpeed_Value;

	public GameObject NaturalRecoverStamina => naturalRecoverStamina;

	public TextMeshProUGUI NaturalRecoverStamina_Value => naturalRecoverStamina_Value;

	public GameObject NaturalRecoveryHp => naturalRecoveryHp;

	public TextMeshProUGUI NaturalRecoveryHp_Value => naturalRecoveryHp_Value;

	public GameObject Stamina => stamina;

	public TextMeshProUGUI Stamina_Value => stamina_Value;

	public GameObject StressDamageResistance => stressDamageResistance;

	public TextMeshProUGUI StressDamageResistance_Value => stressDamageResistance_Value;

	public Toggle Toggle => toggle;

	public GameObject WaterResistance => waterResistance;

	public TextMeshProUGUI WaterResistance_Value => waterResistance_Value;

	public GameObject WeaknessDamageBoost => weaknessDamageBoost;

	public TextMeshProUGUI WeaknessDamageBoost_Value => weaknessDamageBoost_Value;

	public GameObject WindResistance => windResistance;

	public TextMeshProUGUI WindResistance_Value => windResistance_Value;

	public Image Locked => locked;

	public GameObject PlaceHolder => placeHolder;

	public Image CharacterImage => characterImage;

	public Image ImgFloor => imgFloor;

	public Image Shadow => shadow;

	public SkeletonGraphic Spine => spine;

	public GameObject SlotGroup => slotGroup;

	public GameObject[] AccessorySlots => accessorySlots;

	public GameObject[] ArtifactSlots => artifactSlots;

	public InventoryGoodsSlotPresenter WeaponSlot => weaponSlot;
}
