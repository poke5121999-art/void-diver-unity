using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeCardView : View
{
	[SerializeField]
	private Button contents;

	[SerializeField]
	private GameObject normal;

	[SerializeField]
	private GameObject btns;

	[SerializeField]
	private Button btns_BtnCleansing;

	[SerializeField]
	private Image btns_BtnCleansing_Normal;

	[SerializeField]
	private GameObject btns_BtnCleansing_Gold;

	[SerializeField]
	private TextMeshProUGUI btns_BtnCleansing_Gold_Value;

	[SerializeField]
	private Image btns_BtnCleansing_Line;

	[SerializeField]
	private TextMeshProUGUI btns_BtnCleansing_Text;

	[SerializeField]
	private Button btns_BtnFire;

	[SerializeField]
	private GameObject employeeInfo;

	[SerializeField]
	private GameObject employeeInfo_EmployeeCondition;

	[SerializeField]
	private TextMeshProUGUI employeeInfo_EmployeeCondition_TextPanic;

	[SerializeField]
	private TextMeshProUGUI employeeInfo_EmployeeCondition_TextPlaced;

	[SerializeField]
	private Slider employeeInfo_ExpSlider;

	[SerializeField]
	private GameObject employeeInfo_GoldText;

	[SerializeField]
	private Image employeeInfo_GoldText_Icon;

	[SerializeField]
	private TextMeshProUGUI employeeInfo_GoldText_Value;

	[SerializeField]
	private GameObject employeeInfo_Layout;

	[SerializeField]
	private TextMeshProUGUI employeeInfo_TextLv;

	[SerializeField]
	private TextMeshProUGUI employeeInfo_TextLvUp;

	[SerializeField]
	private TextMeshProUGUI employeeInfo_TextName;

	[SerializeField]
	private TextMeshProUGUI employeeInfo_TextNamePanic;

	[SerializeField]
	private Image employeeInfo_Line;

	[SerializeField]
	private Image employeeInfo_LinePanic;

	[SerializeField]
	private GameObject employeeInfo_MaskFace;

	[SerializeField]
	private Image employeeInfo_EmployeeFace;

	[SerializeField]
	private Slider employeeInfo_PanicSlider;

	[SerializeField]
	private GameObject employeeInfo_PanicSlider_Contents;

	[SerializeField]
	private Image employeeInfo_PanicSlider_BG;

	[SerializeField]
	private Image employeeInfo_PanicSlider_Condition;

	[SerializeField]
	private Image employeeInfo_PanicSlider_Gauge;

	[SerializeField]
	private Image employeeInfo_PanicSlider_Gauge_img_back;

	[SerializeField]
	private Image employeeInfo_PanicSlider_Gauge_img_front;

	[SerializeField]
	private Image employeeInfo_PanicSlider_LImage;

	[SerializeField]
	private Image employeeInfo_PanicSlider_RImage;

	[SerializeField]
	private GameObject specialSkillFirst;

	[SerializeField]
	private GameObject specialSkillFirst_Explain;

	[SerializeField]
	private TextMeshProUGUI specialSkillFirst_TextSkill;

	[SerializeField]
	private TextMeshProUGUI specialSkillFirst_TextSkillDesc;

	[SerializeField]
	private GameObject specialSkillFirst_Get;

	[SerializeField]
	private TextMeshProUGUI specialSkillFirst_Get_TextSkill;

	[SerializeField]
	private TextMeshProUGUI specialSkillFirst_Get_TextSkillDesc;

	[SerializeField]
	private Image specialSkillFirst_Lock;

	[SerializeField]
	private GameObject specialSkillSecond;

	[SerializeField]
	private GameObject specialSkillSecond_Explain;

	[SerializeField]
	private TextMeshProUGUI specialSkillSecond_TextSkill;

	[SerializeField]
	private TextMeshProUGUI specialSkillSecond_TextSkillDesc;

	[SerializeField]
	private GameObject specialSkillSecond_Get;

	[SerializeField]
	private TextMeshProUGUI specialSkillSecond_Get_TextSkill;

	[SerializeField]
	private TextMeshProUGUI specialSkillSecond_Get_TextSkillDesc;

	[SerializeField]
	private Image specialSkillSecond_Lock;

	[SerializeField]
	private GameObject stats;

	[SerializeField]
	private GameObject charm;

	[SerializeField]
	private TextMeshProUGUI charm_Num;

	[SerializeField]
	private TextMeshProUGUI charm_NumLvUp;

	[SerializeField]
	private TextMeshProUGUI charm_Title;

	[SerializeField]
	private GameObject persuasion;

	[SerializeField]
	private TextMeshProUGUI persuasion_Num;

	[SerializeField]
	private TextMeshProUGUI persuasion_NumLvUp;

	[SerializeField]
	private TextMeshProUGUI persuasion_Title;

	[SerializeField]
	private GameObject resistance;

	[SerializeField]
	private TextMeshProUGUI resistance_Num;

	[SerializeField]
	private TextMeshProUGUI resistance_NumLvUp;

	[SerializeField]
	private TextMeshProUGUI resistance_Title;

	public Button Contents => contents;

	public GameObject Normal => normal;

	public GameObject Btns => btns;

	public Button Btns_BtnCleansing => btns_BtnCleansing;

	public Image Btns_BtnCleansing_Normal => btns_BtnCleansing_Normal;

	public GameObject Btns_BtnCleansing_Gold => btns_BtnCleansing_Gold;

	public TextMeshProUGUI Btns_BtnCleansing_Gold_Value => btns_BtnCleansing_Gold_Value;

	public Image Btns_BtnCleansing_Line => btns_BtnCleansing_Line;

	public TextMeshProUGUI Btns_BtnCleansing_Text => btns_BtnCleansing_Text;

	public Button Btns_BtnFire => btns_BtnFire;

	public GameObject EmployeeInfo => employeeInfo;

	public GameObject EmployeeInfo_EmployeeCondition => employeeInfo_EmployeeCondition;

	public TextMeshProUGUI EmployeeInfo_EmployeeCondition_TextPanic => employeeInfo_EmployeeCondition_TextPanic;

	public TextMeshProUGUI EmployeeInfo_EmployeeCondition_TextPlaced => employeeInfo_EmployeeCondition_TextPlaced;

	public Slider EmployeeInfo_ExpSlider => employeeInfo_ExpSlider;

	public GameObject EmployeeInfo_GoldText => employeeInfo_GoldText;

	public Image EmployeeInfo_GoldText_Icon => employeeInfo_GoldText_Icon;

	public TextMeshProUGUI EmployeeInfo_GoldText_Value => employeeInfo_GoldText_Value;

	public GameObject EmployeeInfo_Layout => employeeInfo_Layout;

	public TextMeshProUGUI EmployeeInfo_TextLv => employeeInfo_TextLv;

	public TextMeshProUGUI EmployeeInfo_TextLvUp => employeeInfo_TextLvUp;

	public TextMeshProUGUI EmployeeInfo_TextName => employeeInfo_TextName;

	public TextMeshProUGUI EmployeeInfo_TextNamePanic => employeeInfo_TextNamePanic;

	public Image EmployeeInfo_Line => employeeInfo_Line;

	public Image EmployeeInfo_LinePanic => employeeInfo_LinePanic;

	public GameObject EmployeeInfo_MaskFace => employeeInfo_MaskFace;

	public Image EmployeeInfo_EmployeeFace => employeeInfo_EmployeeFace;

	public Slider EmployeeInfo_PanicSlider => employeeInfo_PanicSlider;

	public GameObject EmployeeInfo_PanicSlider_Contents => employeeInfo_PanicSlider_Contents;

	public Image EmployeeInfo_PanicSlider_BG => employeeInfo_PanicSlider_BG;

	public Image EmployeeInfo_PanicSlider_Condition => employeeInfo_PanicSlider_Condition;

	public Image EmployeeInfo_PanicSlider_Gauge => employeeInfo_PanicSlider_Gauge;

	public Image EmployeeInfo_PanicSlider_Gauge_img_back => employeeInfo_PanicSlider_Gauge_img_back;

	public Image EmployeeInfo_PanicSlider_Gauge_img_front => employeeInfo_PanicSlider_Gauge_img_front;

	public Image EmployeeInfo_PanicSlider_LImage => employeeInfo_PanicSlider_LImage;

	public Image EmployeeInfo_PanicSlider_RImage => employeeInfo_PanicSlider_RImage;

	public GameObject SpecialSkillFirst => specialSkillFirst;

	public GameObject SpecialSkillFirst_Explain => specialSkillFirst_Explain;

	public TextMeshProUGUI SpecialSkillFirst_TextSkill => specialSkillFirst_TextSkill;

	public TextMeshProUGUI SpecialSkillFirst_TextSkillDesc => specialSkillFirst_TextSkillDesc;

	public GameObject SpecialSkillFirst_Get => specialSkillFirst_Get;

	public TextMeshProUGUI SpecialSkillFirst_Get_TextSkill => specialSkillFirst_Get_TextSkill;

	public TextMeshProUGUI SpecialSkillFirst_Get_TextSkillDesc => specialSkillFirst_Get_TextSkillDesc;

	public Image SpecialSkillFirst_Lock => specialSkillFirst_Lock;

	public GameObject SpecialSkillSecond => specialSkillSecond;

	public GameObject SpecialSkillSecond_Explain => specialSkillSecond_Explain;

	public TextMeshProUGUI SpecialSkillSecond_TextSkill => specialSkillSecond_TextSkill;

	public TextMeshProUGUI SpecialSkillSecond_TextSkillDesc => specialSkillSecond_TextSkillDesc;

	public GameObject SpecialSkillSecond_Get => specialSkillSecond_Get;

	public TextMeshProUGUI SpecialSkillSecond_Get_TextSkill => specialSkillSecond_Get_TextSkill;

	public TextMeshProUGUI SpecialSkillSecond_Get_TextSkillDesc => specialSkillSecond_Get_TextSkillDesc;

	public Image SpecialSkillSecond_Lock => specialSkillSecond_Lock;

	public GameObject Stats => stats;

	public GameObject Charm => charm;

	public TextMeshProUGUI Charm_Num => charm_Num;

	public TextMeshProUGUI Charm_NumLvUp => charm_NumLvUp;

	public TextMeshProUGUI Charm_Title => charm_Title;

	public GameObject Persuasion => persuasion;

	public TextMeshProUGUI Persuasion_Num => persuasion_Num;

	public TextMeshProUGUI Persuasion_NumLvUp => persuasion_NumLvUp;

	public TextMeshProUGUI Persuasion_Title => persuasion_Title;

	public GameObject Resistance => resistance;

	public TextMeshProUGUI Resistance_Num => resistance_Num;

	public TextMeshProUGUI Resistance_NumLvUp => resistance_NumLvUp;

	public TextMeshProUGUI Resistance_Title => resistance_Title;
}
