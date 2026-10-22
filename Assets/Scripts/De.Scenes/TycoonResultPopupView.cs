using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TycoonResultPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject artifact;

	[SerializeField]
	private TextMeshProUGUI failDesc;

	[SerializeField]
	private GoodsSlotPresenter goodsSlot;

	[SerializeField]
	private GameObject resultCost;

	[SerializeField]
	private GameObject resultCost_EmployeeSkillBubble;

	[SerializeField]
	private GameObject resultCost_EmployeeSkillBubble_Layout;

	[SerializeField]
	private TextMeshProUGUI resultCost_EmployeeSkillBubble_TextPercent;

	[SerializeField]
	private TextMeshProUGUI resultCost_EmployeeSkillBubble_TextPrice;

	[SerializeField]
	private GameObject resultCost_EmployeeSkillBubble_Up;

	[SerializeField]
	private TextMeshProUGUI resultCost_EmployeeSkillBubble_TextTitle;

	[SerializeField]
	private GameObject resultCost_Gold;

	[SerializeField]
	private Image resultCost_Gold_Icon;

	[SerializeField]
	private GameObject resultCost_Gold_LayoutDown;

	[SerializeField]
	private GameObject resultCost_Gold_LayoutDown_Down;

	[SerializeField]
	private TextMeshProUGUI resultCost_Gold_LayoutDown_TextPercent;

	[SerializeField]
	private GameObject resultCost_Gold_LayoutUp;

	[SerializeField]
	private TextMeshProUGUI resultCost_Gold_LayoutUp_TextPercent;

	[SerializeField]
	private GameObject resultCost_Gold_LayoutUp_Up;

	[SerializeField]
	private TextMeshProUGUI resultCost_Gold_TextCost;

	[SerializeField]
	private TextMeshProUGUI resultCost_TitleText;

	[SerializeField]
	private GameObject resultExp;

	[SerializeField]
	private TextMeshProUGUI resultExp_TextCost;

	[SerializeField]
	private TextMeshProUGUI resultExp_TitleText;

	[SerializeField]
	private GameObject sellResult;

	[SerializeField]
	private TextMeshProUGUI sellResult_TextFail;

	[SerializeField]
	private TextMeshProUGUI sellResult_TextSuccess;

	[SerializeField]
	private TextMeshProUGUI sellResult_TextTitle;

	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private GameObject employee;

	[SerializeField]
	private GameObject employee_EmployeeInfo;

	[SerializeField]
	private TextMeshProUGUI employee_EmployeeInfo_TextLevel;

	[SerializeField]
	private TextMeshProUGUI employee_EmployeeInfo_TextName;

	[SerializeField]
	private EmployeeSimpleSlotPresenter employee_EmployeeSimpleSlot;

	[SerializeField]
	private GameObject employee_Fail;

	[SerializeField]
	private TextMeshProUGUI employee_Fail_TextDesc;

	[SerializeField]
	private TextMeshProUGUI employee_Fail_TextTitle;

	[SerializeField]
	private GameObject employee_Success;

	[SerializeField]
	private GameObject employee_Success_ResultEmployeeExp;

	[SerializeField]
	private Slider employee_Success_ResultEmployeeExp_ExpSlider;

	[SerializeField]
	private TextMeshProUGUI employee_Success_ResultEmployeeExp_Text;

	[SerializeField]
	private GameObject employee_Success_ResultEmployeeStress;

	[SerializeField]
	private GameObject employee_Success_ResultEmployeeStress_EmployeeSkillBubble;

	[SerializeField]
	private TextMeshProUGUI employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextDesc;

	[SerializeField]
	private TextMeshProUGUI employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextTitle;

	[SerializeField]
	private Slider employee_Success_ResultEmployeeStress_PanicSlider;

	[SerializeField]
	private GameObject employee_Success_ResultEmployeeStress_PanicSlider_Contents;

	[SerializeField]
	private Image employee_Success_ResultEmployeeStress_PanicSlider_ConditionFace;

	[SerializeField]
	private Image employee_Success_ResultEmployeeStress_PanicSlider_Gauge;

	[SerializeField]
	private Image employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_back;

	[SerializeField]
	private Image employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_front;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject popupTitle;

	[SerializeField]
	private TextMeshProUGUI popupTitle_TextTitle;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Artifact => null;

	public TextMeshProUGUI FailDesc => null;

	public GoodsSlotPresenter GoodsSlot => null;

	public GameObject ResultCost => null;

	public GameObject ResultCost_EmployeeSkillBubble => null;

	public GameObject ResultCost_EmployeeSkillBubble_Layout => null;

	public TextMeshProUGUI ResultCost_EmployeeSkillBubble_TextPercent => null;

	public TextMeshProUGUI ResultCost_EmployeeSkillBubble_TextPrice => null;

	public GameObject ResultCost_EmployeeSkillBubble_Up => null;

	public TextMeshProUGUI ResultCost_EmployeeSkillBubble_TextTitle => null;

	public GameObject ResultCost_Gold => null;

	public Image ResultCost_Gold_Icon => null;

	public GameObject ResultCost_Gold_LayoutDown => null;

	public GameObject ResultCost_Gold_LayoutDown_Down => null;

	public TextMeshProUGUI ResultCost_Gold_LayoutDown_TextPercent => null;

	public GameObject ResultCost_Gold_LayoutUp => null;

	public TextMeshProUGUI ResultCost_Gold_LayoutUp_TextPercent => null;

	public GameObject ResultCost_Gold_LayoutUp_Up => null;

	public TextMeshProUGUI ResultCost_Gold_TextCost => null;

	public TextMeshProUGUI ResultCost_TitleText => null;

	public GameObject ResultExp => null;

	public TextMeshProUGUI ResultExp_TextCost => null;

	public TextMeshProUGUI ResultExp_TitleText => null;

	public GameObject SellResult => null;

	public TextMeshProUGUI SellResult_TextFail => null;

	public TextMeshProUGUI SellResult_TextSuccess => null;

	public TextMeshProUGUI SellResult_TextTitle => null;

	public GameObject Buttons => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Employee => null;

	public GameObject Employee_EmployeeInfo => null;

	public TextMeshProUGUI Employee_EmployeeInfo_TextLevel => null;

	public TextMeshProUGUI Employee_EmployeeInfo_TextName => null;

	public EmployeeSimpleSlotPresenter Employee_EmployeeSimpleSlot => null;

	public GameObject Employee_Fail => null;

	public TextMeshProUGUI Employee_Fail_TextDesc => null;

	public TextMeshProUGUI Employee_Fail_TextTitle => null;

	public GameObject Employee_Success => null;

	public GameObject Employee_Success_ResultEmployeeExp => null;

	public Slider Employee_Success_ResultEmployeeExp_ExpSlider => null;

	public TextMeshProUGUI Employee_Success_ResultEmployeeExp_Text => null;

	public GameObject Employee_Success_ResultEmployeeStress => null;

	public GameObject Employee_Success_ResultEmployeeStress_EmployeeSkillBubble => null;

	public TextMeshProUGUI Employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextDesc => null;

	public TextMeshProUGUI Employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextTitle => null;

	public Slider Employee_Success_ResultEmployeeStress_PanicSlider => null;

	public GameObject Employee_Success_ResultEmployeeStress_PanicSlider_Contents => null;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_ConditionFace => null;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_Gauge => null;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_back => null;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_front => null;

	public GameObject Top => null;

	public GameObject PopupTitle => null;

	public TextMeshProUGUI PopupTitle_TextTitle => null;
}
