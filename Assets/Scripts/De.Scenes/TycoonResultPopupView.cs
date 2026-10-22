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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Artifact => artifact;

	public TextMeshProUGUI FailDesc => failDesc;

	public GoodsSlotPresenter GoodsSlot => goodsSlot;

	public GameObject ResultCost => resultCost;

	public GameObject ResultCost_EmployeeSkillBubble => resultCost_EmployeeSkillBubble;

	public GameObject ResultCost_EmployeeSkillBubble_Layout => resultCost_EmployeeSkillBubble_Layout;

	public TextMeshProUGUI ResultCost_EmployeeSkillBubble_TextPercent => resultCost_EmployeeSkillBubble_TextPercent;

	public TextMeshProUGUI ResultCost_EmployeeSkillBubble_TextPrice => resultCost_EmployeeSkillBubble_TextPrice;

	public GameObject ResultCost_EmployeeSkillBubble_Up => resultCost_EmployeeSkillBubble_Up;

	public TextMeshProUGUI ResultCost_EmployeeSkillBubble_TextTitle => resultCost_EmployeeSkillBubble_TextTitle;

	public GameObject ResultCost_Gold => resultCost_Gold;

	public Image ResultCost_Gold_Icon => resultCost_Gold_Icon;

	public GameObject ResultCost_Gold_LayoutDown => resultCost_Gold_LayoutDown;

	public GameObject ResultCost_Gold_LayoutDown_Down => resultCost_Gold_LayoutDown_Down;

	public TextMeshProUGUI ResultCost_Gold_LayoutDown_TextPercent => resultCost_Gold_LayoutDown_TextPercent;

	public GameObject ResultCost_Gold_LayoutUp => resultCost_Gold_LayoutUp;

	public TextMeshProUGUI ResultCost_Gold_LayoutUp_TextPercent => resultCost_Gold_LayoutUp_TextPercent;

	public GameObject ResultCost_Gold_LayoutUp_Up => resultCost_Gold_LayoutUp_Up;

	public TextMeshProUGUI ResultCost_Gold_TextCost => resultCost_Gold_TextCost;

	public TextMeshProUGUI ResultCost_TitleText => resultCost_TitleText;

	public GameObject ResultExp => resultExp;

	public TextMeshProUGUI ResultExp_TextCost => resultExp_TextCost;

	public TextMeshProUGUI ResultExp_TitleText => resultExp_TitleText;

	public GameObject SellResult => sellResult;

	public TextMeshProUGUI SellResult_TextFail => sellResult_TextFail;

	public TextMeshProUGUI SellResult_TextSuccess => sellResult_TextSuccess;

	public TextMeshProUGUI SellResult_TextTitle => sellResult_TextTitle;

	public GameObject Buttons => buttons;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject Employee => employee;

	public GameObject Employee_EmployeeInfo => employee_EmployeeInfo;

	public TextMeshProUGUI Employee_EmployeeInfo_TextLevel => employee_EmployeeInfo_TextLevel;

	public TextMeshProUGUI Employee_EmployeeInfo_TextName => employee_EmployeeInfo_TextName;

	public EmployeeSimpleSlotPresenter Employee_EmployeeSimpleSlot => employee_EmployeeSimpleSlot;

	public GameObject Employee_Fail => employee_Fail;

	public TextMeshProUGUI Employee_Fail_TextDesc => employee_Fail_TextDesc;

	public TextMeshProUGUI Employee_Fail_TextTitle => employee_Fail_TextTitle;

	public GameObject Employee_Success => employee_Success;

	public GameObject Employee_Success_ResultEmployeeExp => employee_Success_ResultEmployeeExp;

	public Slider Employee_Success_ResultEmployeeExp_ExpSlider => employee_Success_ResultEmployeeExp_ExpSlider;

	public TextMeshProUGUI Employee_Success_ResultEmployeeExp_Text => employee_Success_ResultEmployeeExp_Text;

	public GameObject Employee_Success_ResultEmployeeStress => employee_Success_ResultEmployeeStress;

	public GameObject Employee_Success_ResultEmployeeStress_EmployeeSkillBubble => employee_Success_ResultEmployeeStress_EmployeeSkillBubble;

	public TextMeshProUGUI Employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextDesc => employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextDesc;

	public TextMeshProUGUI Employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextTitle => employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextTitle;

	public Slider Employee_Success_ResultEmployeeStress_PanicSlider => employee_Success_ResultEmployeeStress_PanicSlider;

	public GameObject Employee_Success_ResultEmployeeStress_PanicSlider_Contents => employee_Success_ResultEmployeeStress_PanicSlider_Contents;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_ConditionFace => employee_Success_ResultEmployeeStress_PanicSlider_ConditionFace;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_Gauge => employee_Success_ResultEmployeeStress_PanicSlider_Gauge;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_back => employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_back;

	public Image Employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_front => employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_front;

	public GameObject Top => top;

	public GameObject PopupTitle => popupTitle;

	public TextMeshProUGUI PopupTitle_TextTitle => popupTitle_TextTitle;
}
