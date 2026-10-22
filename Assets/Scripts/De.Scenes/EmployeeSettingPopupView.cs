using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeSettingPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private CanvasGroup body;

	[SerializeField]
	private Button btnEmptyEmployee;

	[SerializeField]
	private GameObject employeeLayout;

	[SerializeField]
	private GameObject employeeLayout_Arrow;

	[SerializeField]
	private GameObject employeeLayout_LeftEmployee;

	[SerializeField]
	private Slider employeeLayout_LeftEmployee_ExpSlider;

	[SerializeField]
	private GameObject employeeLayout_LeftEmployee_Layout;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_LeftEmployee_TextLv;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_LeftEmployee_TextName;

	[SerializeField]
	private GameObject employeeLayout_LeftEmployee_LayoutPanic;

	[SerializeField]
	private Image employeeLayout_LeftEmployee_LayoutPanic_Bg;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_LeftEmployee_LayoutPanic_TextLv;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_LeftEmployee_LayoutPanic_TextName;

	[SerializeField]
	private Slider employeeLayout_LeftEmployee_PanicSlider;

	[SerializeField]
	private GameObject employeeLayout_LeftEmployee_PanicSlider_Contents;

	[SerializeField]
	private Image employeeLayout_LeftEmployee_PanicSlider_BG;

	[SerializeField]
	private Image employeeLayout_LeftEmployee_PanicSlider_ConditionFace;

	[SerializeField]
	private Image employeeLayout_LeftEmployee_PanicSlider_Gauge;

	[SerializeField]
	private Image employeeLayout_LeftEmployee_Shadow;

	[SerializeField]
	private SkeletonGraphic employeeLayout_LeftEmployee_Skeleton;

	[SerializeField]
	private GameObject employeeLayout_RightEmployee;

	[SerializeField]
	private Slider employeeLayout_RightEmployee_ExpSlider;

	[SerializeField]
	private GameObject employeeLayout_RightEmployee_Layout;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_RightEmployee_TextLv;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_RightEmployee_TextName;

	[SerializeField]
	private GameObject employeeLayout_RightEmployee_LayoutPanic;

	[SerializeField]
	private Image employeeLayout_RightEmployee_LayoutPanic_Bg;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_RightEmployee_LayoutPanic_TextLv;

	[SerializeField]
	private TextMeshProUGUI employeeLayout_RightEmployee_LayoutPanic_TextName;

	[SerializeField]
	private Slider employeeLayout_RightEmployee_PanicSlider;

	[SerializeField]
	private GameObject employeeLayout_RightEmployee_PanicSlider_Contents;

	[SerializeField]
	private Image employeeLayout_RightEmployee_PanicSlider_BG;

	[SerializeField]
	private Image employeeLayout_RightEmployee_PanicSlider_ConditionFace;

	[SerializeField]
	private Image employeeLayout_RightEmployee_PanicSlider_Gauge;

	[SerializeField]
	private Image employeeLayout_RightEmployee_Shadow;

	[SerializeField]
	private SkeletonGraphic employeeLayout_RightEmployee_Skeleton;

	[SerializeField]
	private Image imgFloor;

	[SerializeField]
	private EmployeeScrollerPresenter scrollView;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject top_CurrencyLayout;

	public Button Background => background;

	public GameObject Content => content;

	public CanvasGroup Body => body;

	public Button BtnEmptyEmployee => btnEmptyEmployee;

	public GameObject EmployeeLayout => employeeLayout;

	public GameObject EmployeeLayout_Arrow => employeeLayout_Arrow;

	public GameObject EmployeeLayout_LeftEmployee => employeeLayout_LeftEmployee;

	public Slider EmployeeLayout_LeftEmployee_ExpSlider => employeeLayout_LeftEmployee_ExpSlider;

	public GameObject EmployeeLayout_LeftEmployee_Layout => employeeLayout_LeftEmployee_Layout;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_TextLv => employeeLayout_LeftEmployee_TextLv;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_TextName => employeeLayout_LeftEmployee_TextName;

	public GameObject EmployeeLayout_LeftEmployee_LayoutPanic => employeeLayout_LeftEmployee_LayoutPanic;

	public Image EmployeeLayout_LeftEmployee_LayoutPanic_Bg => employeeLayout_LeftEmployee_LayoutPanic_Bg;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_LayoutPanic_TextLv => employeeLayout_LeftEmployee_LayoutPanic_TextLv;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_LayoutPanic_TextName => employeeLayout_LeftEmployee_LayoutPanic_TextName;

	public Slider EmployeeLayout_LeftEmployee_PanicSlider => employeeLayout_LeftEmployee_PanicSlider;

	public GameObject EmployeeLayout_LeftEmployee_PanicSlider_Contents => employeeLayout_LeftEmployee_PanicSlider_Contents;

	public Image EmployeeLayout_LeftEmployee_PanicSlider_BG => employeeLayout_LeftEmployee_PanicSlider_BG;

	public Image EmployeeLayout_LeftEmployee_PanicSlider_ConditionFace => employeeLayout_LeftEmployee_PanicSlider_ConditionFace;

	public Image EmployeeLayout_LeftEmployee_PanicSlider_Gauge => employeeLayout_LeftEmployee_PanicSlider_Gauge;

	public Image EmployeeLayout_LeftEmployee_Shadow => employeeLayout_LeftEmployee_Shadow;

	public SkeletonGraphic EmployeeLayout_LeftEmployee_Skeleton => employeeLayout_LeftEmployee_Skeleton;

	public GameObject EmployeeLayout_RightEmployee => employeeLayout_RightEmployee;

	public Slider EmployeeLayout_RightEmployee_ExpSlider => employeeLayout_RightEmployee_ExpSlider;

	public GameObject EmployeeLayout_RightEmployee_Layout => employeeLayout_RightEmployee_Layout;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_TextLv => employeeLayout_RightEmployee_TextLv;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_TextName => employeeLayout_RightEmployee_TextName;

	public GameObject EmployeeLayout_RightEmployee_LayoutPanic => employeeLayout_RightEmployee_LayoutPanic;

	public Image EmployeeLayout_RightEmployee_LayoutPanic_Bg => employeeLayout_RightEmployee_LayoutPanic_Bg;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_LayoutPanic_TextLv => employeeLayout_RightEmployee_LayoutPanic_TextLv;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_LayoutPanic_TextName => employeeLayout_RightEmployee_LayoutPanic_TextName;

	public Slider EmployeeLayout_RightEmployee_PanicSlider => employeeLayout_RightEmployee_PanicSlider;

	public GameObject EmployeeLayout_RightEmployee_PanicSlider_Contents => employeeLayout_RightEmployee_PanicSlider_Contents;

	public Image EmployeeLayout_RightEmployee_PanicSlider_BG => employeeLayout_RightEmployee_PanicSlider_BG;

	public Image EmployeeLayout_RightEmployee_PanicSlider_ConditionFace => employeeLayout_RightEmployee_PanicSlider_ConditionFace;

	public Image EmployeeLayout_RightEmployee_PanicSlider_Gauge => employeeLayout_RightEmployee_PanicSlider_Gauge;

	public Image EmployeeLayout_RightEmployee_Shadow => employeeLayout_RightEmployee_Shadow;

	public SkeletonGraphic EmployeeLayout_RightEmployee_Skeleton => employeeLayout_RightEmployee_Skeleton;

	public Image ImgFloor => imgFloor;

	public EmployeeScrollerPresenter ScrollView => scrollView;

	public GameObject Top => top;

	public GameObject Top_CurrencyLayout => top_CurrencyLayout;
}
