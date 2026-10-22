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

	public Button Background => null;

	public GameObject Content => null;

	public CanvasGroup Body => null;

	public Button BtnEmptyEmployee => null;

	public GameObject EmployeeLayout => null;

	public GameObject EmployeeLayout_Arrow => null;

	public GameObject EmployeeLayout_LeftEmployee => null;

	public Slider EmployeeLayout_LeftEmployee_ExpSlider => null;

	public GameObject EmployeeLayout_LeftEmployee_Layout => null;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_TextLv => null;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_TextName => null;

	public GameObject EmployeeLayout_LeftEmployee_LayoutPanic => null;

	public Image EmployeeLayout_LeftEmployee_LayoutPanic_Bg => null;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_LayoutPanic_TextLv => null;

	public TextMeshProUGUI EmployeeLayout_LeftEmployee_LayoutPanic_TextName => null;

	public Slider EmployeeLayout_LeftEmployee_PanicSlider => null;

	public GameObject EmployeeLayout_LeftEmployee_PanicSlider_Contents => null;

	public Image EmployeeLayout_LeftEmployee_PanicSlider_BG => null;

	public Image EmployeeLayout_LeftEmployee_PanicSlider_ConditionFace => null;

	public Image EmployeeLayout_LeftEmployee_PanicSlider_Gauge => null;

	public Image EmployeeLayout_LeftEmployee_Shadow => null;

	public SkeletonGraphic EmployeeLayout_LeftEmployee_Skeleton => null;

	public GameObject EmployeeLayout_RightEmployee => null;

	public Slider EmployeeLayout_RightEmployee_ExpSlider => null;

	public GameObject EmployeeLayout_RightEmployee_Layout => null;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_TextLv => null;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_TextName => null;

	public GameObject EmployeeLayout_RightEmployee_LayoutPanic => null;

	public Image EmployeeLayout_RightEmployee_LayoutPanic_Bg => null;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_LayoutPanic_TextLv => null;

	public TextMeshProUGUI EmployeeLayout_RightEmployee_LayoutPanic_TextName => null;

	public Slider EmployeeLayout_RightEmployee_PanicSlider => null;

	public GameObject EmployeeLayout_RightEmployee_PanicSlider_Contents => null;

	public Image EmployeeLayout_RightEmployee_PanicSlider_BG => null;

	public Image EmployeeLayout_RightEmployee_PanicSlider_ConditionFace => null;

	public Image EmployeeLayout_RightEmployee_PanicSlider_Gauge => null;

	public Image EmployeeLayout_RightEmployee_Shadow => null;

	public SkeletonGraphic EmployeeLayout_RightEmployee_Skeleton => null;

	public Image ImgFloor => null;

	public EmployeeScrollerPresenter ScrollView => null;

	public GameObject Top => null;

	public GameObject Top_CurrencyLayout => null;
}
