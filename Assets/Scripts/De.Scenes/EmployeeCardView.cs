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

	public Button Contents => null;

	public GameObject Normal => null;

	public GameObject Btns => null;

	public Button Btns_BtnCleansing => null;

	public Image Btns_BtnCleansing_Normal => null;

	public GameObject Btns_BtnCleansing_Gold => null;

	public TextMeshProUGUI Btns_BtnCleansing_Gold_Value => null;

	public Image Btns_BtnCleansing_Line => null;

	public TextMeshProUGUI Btns_BtnCleansing_Text => null;

	public Button Btns_BtnFire => null;

	public GameObject EmployeeInfo => null;

	public GameObject EmployeeInfo_EmployeeCondition => null;

	public TextMeshProUGUI EmployeeInfo_EmployeeCondition_TextPanic => null;

	public TextMeshProUGUI EmployeeInfo_EmployeeCondition_TextPlaced => null;

	public Slider EmployeeInfo_ExpSlider => null;

	public GameObject EmployeeInfo_GoldText => null;

	public Image EmployeeInfo_GoldText_Icon => null;

	public TextMeshProUGUI EmployeeInfo_GoldText_Value => null;

	public GameObject EmployeeInfo_Layout => null;

	public TextMeshProUGUI EmployeeInfo_TextLv => null;

	public TextMeshProUGUI EmployeeInfo_TextLvUp => null;

	public TextMeshProUGUI EmployeeInfo_TextName => null;

	public TextMeshProUGUI EmployeeInfo_TextNamePanic => null;

	public Image EmployeeInfo_Line => null;

	public Image EmployeeInfo_LinePanic => null;

	public GameObject EmployeeInfo_MaskFace => null;

	public Image EmployeeInfo_EmployeeFace => null;

	public Slider EmployeeInfo_PanicSlider => null;

	public GameObject EmployeeInfo_PanicSlider_Contents => null;

	public Image EmployeeInfo_PanicSlider_BG => null;

	public Image EmployeeInfo_PanicSlider_Condition => null;

	public Image EmployeeInfo_PanicSlider_Gauge => null;

	public Image EmployeeInfo_PanicSlider_Gauge_img_back => null;

	public Image EmployeeInfo_PanicSlider_Gauge_img_front => null;

	public Image EmployeeInfo_PanicSlider_LImage => null;

	public Image EmployeeInfo_PanicSlider_RImage => null;

	public GameObject SpecialSkillFirst => null;

	public GameObject SpecialSkillFirst_Explain => null;

	public TextMeshProUGUI SpecialSkillFirst_TextSkill => null;

	public TextMeshProUGUI SpecialSkillFirst_TextSkillDesc => null;

	public GameObject SpecialSkillFirst_Get => null;

	public TextMeshProUGUI SpecialSkillFirst_Get_TextSkill => null;

	public TextMeshProUGUI SpecialSkillFirst_Get_TextSkillDesc => null;

	public Image SpecialSkillFirst_Lock => null;

	public GameObject SpecialSkillSecond => null;

	public GameObject SpecialSkillSecond_Explain => null;

	public TextMeshProUGUI SpecialSkillSecond_TextSkill => null;

	public TextMeshProUGUI SpecialSkillSecond_TextSkillDesc => null;

	public GameObject SpecialSkillSecond_Get => null;

	public TextMeshProUGUI SpecialSkillSecond_Get_TextSkill => null;

	public TextMeshProUGUI SpecialSkillSecond_Get_TextSkillDesc => null;

	public Image SpecialSkillSecond_Lock => null;

	public GameObject Stats => null;

	public GameObject Charm => null;

	public TextMeshProUGUI Charm_Num => null;

	public TextMeshProUGUI Charm_NumLvUp => null;

	public TextMeshProUGUI Charm_Title => null;

	public GameObject Persuasion => null;

	public TextMeshProUGUI Persuasion_Num => null;

	public TextMeshProUGUI Persuasion_NumLvUp => null;

	public TextMeshProUGUI Persuasion_Title => null;

	public GameObject Resistance => null;

	public TextMeshProUGUI Resistance_Num => null;

	public TextMeshProUGUI Resistance_NumLvUp => null;

	public TextMeshProUGUI Resistance_Title => null;
}
