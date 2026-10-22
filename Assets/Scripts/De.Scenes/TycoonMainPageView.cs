using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TycoonMainPageView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private ScrollRect scrollView;

	[SerializeField]
	private GameObject slotInfo;

	[SerializeField]
	private GameObject btnLayout;

	[SerializeField]
	private Button btnCancelArtifact;

	[SerializeField]
	private Button btnChangeEmployee;

	[SerializeField]
	private Button btnResult;

	[SerializeField]
	private Button btnSetArtifact;

	[SerializeField]
	private Button btnSettingEmployee;

	[SerializeField]
	private Button btnStart;

	[SerializeField]
	private GameObject servingNow;

	[SerializeField]
	private GameObject employeeSpine;

	[SerializeField]
	private GameObject employeeSpine_EmployeeInfo;

	[SerializeField]
	private Slider employeeSpine_EmployeeInfo_ExpSlider;

	[SerializeField]
	private GameObject employeeSpine_EmployeeInfo_Layout;

	[SerializeField]
	private Image employeeSpine_EmployeeInfo_Layout_Bg;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_EmployeeInfo_Layout_TextLv;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_EmployeeInfo_Layout_TextName;

	[SerializeField]
	private GameObject employeeSpine_EmployeeInfo_LayoutPanic;

	[SerializeField]
	private Image employeeSpine_EmployeeInfo_LayoutPanic_Bg;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_EmployeeInfo_LayoutPanic_TextLv;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_EmployeeInfo_LayoutPanic_TextName;

	[SerializeField]
	private Slider employeeSpine_EmployeeInfo_PanicSlider;

	[SerializeField]
	private GameObject employeeSpine_EmployeeInfo_PanicSlider_Contents;

	[SerializeField]
	private Image employeeSpine_EmployeeInfo_PanicSlider_ConditionFace;

	[SerializeField]
	private Image employeeSpine_EmployeeInfo_PanicSlider_Gauge;

	[SerializeField]
	private Image employeeSpine_EmployeeInfo_PanicSlider_Gauge_img_back;

	[SerializeField]
	private Image employeeSpine_EmployeeInfo_PanicSlider_Gauge_img_front;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_EmployeeInfo_TextDesc;

	[SerializeField]
	private GameObject employeeSpine_Empty;

	[SerializeField]
	private SkeletonGraphic employeeSpine_Skeleton;

	[SerializeField]
	private GameObject employeeSpine_TextBubble;

	[SerializeField]
	private Image employeeSpine_TextBubble_BG;

	[SerializeField]
	private Image employeeSpine_TextBubble_Tail;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_TextBubble_Text;

	[SerializeField]
	private GameObject employeeSpine_TextBubblePanic;

	[SerializeField]
	private Image employeeSpine_TextBubblePanic_BG;

	[SerializeField]
	private Image employeeSpine_TextBubblePanic_Tail;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_TextBubblePanic_Text;

	[SerializeField]
	private Image exhibitionStand;

	[SerializeField]
	private GameObject artifactGradeVfx;

	[SerializeField]
	private GameObject artifactGradeVfx_Elite;

	[SerializeField]
	private GameObject artifactGradeVfx_Epic;

	[SerializeField]
	private GameObject artifactGradeVfx_Legend;

	[SerializeField]
	private GameObject artifactGradeVfx_Myth;

	[SerializeField]
	private GameObject artifactGradeVfx_Rare;

	[SerializeField]
	private GameObject artifactInfo;

	[SerializeField]
	private GameObject artifactInfo_GradeLayout;

	[SerializeField]
	private Image artifactInfo_GradeLayout_Bg;

	[SerializeField]
	private TextMeshProUGUI artifactInfo_GradeLayout_Text;

	[SerializeField]
	private Slider artifactInfo_SellSlider;

	[SerializeField]
	private TextMeshProUGUI artifactInfo_TextArtifactName;

	[SerializeField]
	private TextMeshProUGUI artifactInfo_TextDesc;

	[SerializeField]
	private Image imgArtifact;

	[SerializeField]
	private GameObject textBubbleComplete;

	[SerializeField]
	private Image textBubbleComplete_BG;

	[SerializeField]
	private GameObject textBubbleComplete_GoldText;

	[SerializeField]
	private Image textBubbleComplete_GoldText_Icon;

	[SerializeField]
	private TextMeshProUGUI textBubbleComplete_GoldText_Value;

	[SerializeField]
	private Image textBubbleComplete_Tail;

	[SerializeField]
	private TextMeshProUGUI textBubbleComplete_Text;

	[SerializeField]
	private GameObject textBubbleEmpty;

	[SerializeField]
	private Image imgFloor;

	[SerializeField]
	private Image slotNum;

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private TextMeshProUGUI textBreak;

	public GameObject Content => null;

	public ScrollRect ScrollView => null;

	public GameObject SlotInfo => null;

	public GameObject BtnLayout => null;

	public Button BtnCancelArtifact => null;

	public Button BtnChangeEmployee => null;

	public Button BtnResult => null;

	public Button BtnSetArtifact => null;

	public Button BtnSettingEmployee => null;

	public Button BtnStart => null;

	public GameObject ServingNow => null;

	public GameObject EmployeeSpine => null;

	public GameObject EmployeeSpine_EmployeeInfo => null;

	public Slider EmployeeSpine_EmployeeInfo_ExpSlider => null;

	public GameObject EmployeeSpine_EmployeeInfo_Layout => null;

	public Image EmployeeSpine_EmployeeInfo_Layout_Bg => null;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_Layout_TextLv => null;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_Layout_TextName => null;

	public GameObject EmployeeSpine_EmployeeInfo_LayoutPanic => null;

	public Image EmployeeSpine_EmployeeInfo_LayoutPanic_Bg => null;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_LayoutPanic_TextLv => null;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_LayoutPanic_TextName => null;

	public Slider EmployeeSpine_EmployeeInfo_PanicSlider => null;

	public GameObject EmployeeSpine_EmployeeInfo_PanicSlider_Contents => null;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_ConditionFace => null;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_Gauge => null;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_Gauge_img_back => null;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_Gauge_img_front => null;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_TextDesc => null;

	public GameObject EmployeeSpine_Empty => null;

	public SkeletonGraphic EmployeeSpine_Skeleton => null;

	public GameObject EmployeeSpine_TextBubble => null;

	public Image EmployeeSpine_TextBubble_BG => null;

	public Image EmployeeSpine_TextBubble_Tail => null;

	public TextMeshProUGUI EmployeeSpine_TextBubble_Text => null;

	public GameObject EmployeeSpine_TextBubblePanic => null;

	public Image EmployeeSpine_TextBubblePanic_BG => null;

	public Image EmployeeSpine_TextBubblePanic_Tail => null;

	public TextMeshProUGUI EmployeeSpine_TextBubblePanic_Text => null;

	public Image ExhibitionStand => null;

	public GameObject ArtifactGradeVfx => null;

	public GameObject ArtifactGradeVfx_Elite => null;

	public GameObject ArtifactGradeVfx_Epic => null;

	public GameObject ArtifactGradeVfx_Legend => null;

	public GameObject ArtifactGradeVfx_Myth => null;

	public GameObject ArtifactGradeVfx_Rare => null;

	public GameObject ArtifactInfo => null;

	public GameObject ArtifactInfo_GradeLayout => null;

	public Image ArtifactInfo_GradeLayout_Bg => null;

	public TextMeshProUGUI ArtifactInfo_GradeLayout_Text => null;

	public Slider ArtifactInfo_SellSlider => null;

	public TextMeshProUGUI ArtifactInfo_TextArtifactName => null;

	public TextMeshProUGUI ArtifactInfo_TextDesc => null;

	public Image ImgArtifact => null;

	public GameObject TextBubbleComplete => null;

	public Image TextBubbleComplete_BG => null;

	public GameObject TextBubbleComplete_GoldText => null;

	public Image TextBubbleComplete_GoldText_Icon => null;

	public TextMeshProUGUI TextBubbleComplete_GoldText_Value => null;

	public Image TextBubbleComplete_Tail => null;

	public TextMeshProUGUI TextBubbleComplete_Text => null;

	public GameObject TextBubbleEmpty => null;

	public Image ImgFloor => null;

	public Image SlotNum => null;

	public TextMeshProUGUI Text => null;

	public TextMeshProUGUI TextBreak => null;
}
