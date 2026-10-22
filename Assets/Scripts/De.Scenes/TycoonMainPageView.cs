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

	public GameObject Content => content;

	public ScrollRect ScrollView => scrollView;

	public GameObject SlotInfo => slotInfo;

	public GameObject BtnLayout => btnLayout;

	public Button BtnCancelArtifact => btnCancelArtifact;

	public Button BtnChangeEmployee => btnChangeEmployee;

	public Button BtnResult => btnResult;

	public Button BtnSetArtifact => btnSetArtifact;

	public Button BtnSettingEmployee => btnSettingEmployee;

	public Button BtnStart => btnStart;

	public GameObject ServingNow => servingNow;

	public GameObject EmployeeSpine => employeeSpine;

	public GameObject EmployeeSpine_EmployeeInfo => employeeSpine_EmployeeInfo;

	public Slider EmployeeSpine_EmployeeInfo_ExpSlider => employeeSpine_EmployeeInfo_ExpSlider;

	public GameObject EmployeeSpine_EmployeeInfo_Layout => employeeSpine_EmployeeInfo_Layout;

	public Image EmployeeSpine_EmployeeInfo_Layout_Bg => employeeSpine_EmployeeInfo_Layout_Bg;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_Layout_TextLv => employeeSpine_EmployeeInfo_Layout_TextLv;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_Layout_TextName => employeeSpine_EmployeeInfo_Layout_TextName;

	public GameObject EmployeeSpine_EmployeeInfo_LayoutPanic => employeeSpine_EmployeeInfo_LayoutPanic;

	public Image EmployeeSpine_EmployeeInfo_LayoutPanic_Bg => employeeSpine_EmployeeInfo_LayoutPanic_Bg;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_LayoutPanic_TextLv => employeeSpine_EmployeeInfo_LayoutPanic_TextLv;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_LayoutPanic_TextName => employeeSpine_EmployeeInfo_LayoutPanic_TextName;

	public Slider EmployeeSpine_EmployeeInfo_PanicSlider => employeeSpine_EmployeeInfo_PanicSlider;

	public GameObject EmployeeSpine_EmployeeInfo_PanicSlider_Contents => employeeSpine_EmployeeInfo_PanicSlider_Contents;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_ConditionFace => employeeSpine_EmployeeInfo_PanicSlider_ConditionFace;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_Gauge => employeeSpine_EmployeeInfo_PanicSlider_Gauge;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_Gauge_img_back => employeeSpine_EmployeeInfo_PanicSlider_Gauge_img_back;

	public Image EmployeeSpine_EmployeeInfo_PanicSlider_Gauge_img_front => employeeSpine_EmployeeInfo_PanicSlider_Gauge_img_front;

	public TextMeshProUGUI EmployeeSpine_EmployeeInfo_TextDesc => employeeSpine_EmployeeInfo_TextDesc;

	public GameObject EmployeeSpine_Empty => employeeSpine_Empty;

	public SkeletonGraphic EmployeeSpine_Skeleton => employeeSpine_Skeleton;

	public GameObject EmployeeSpine_TextBubble => employeeSpine_TextBubble;

	public Image EmployeeSpine_TextBubble_BG => employeeSpine_TextBubble_BG;

	public Image EmployeeSpine_TextBubble_Tail => employeeSpine_TextBubble_Tail;

	public TextMeshProUGUI EmployeeSpine_TextBubble_Text => employeeSpine_TextBubble_Text;

	public GameObject EmployeeSpine_TextBubblePanic => employeeSpine_TextBubblePanic;

	public Image EmployeeSpine_TextBubblePanic_BG => employeeSpine_TextBubblePanic_BG;

	public Image EmployeeSpine_TextBubblePanic_Tail => employeeSpine_TextBubblePanic_Tail;

	public TextMeshProUGUI EmployeeSpine_TextBubblePanic_Text => employeeSpine_TextBubblePanic_Text;

	public Image ExhibitionStand => exhibitionStand;

	public GameObject ArtifactGradeVfx => artifactGradeVfx;

	public GameObject ArtifactGradeVfx_Elite => artifactGradeVfx_Elite;

	public GameObject ArtifactGradeVfx_Epic => artifactGradeVfx_Epic;

	public GameObject ArtifactGradeVfx_Legend => artifactGradeVfx_Legend;

	public GameObject ArtifactGradeVfx_Myth => artifactGradeVfx_Myth;

	public GameObject ArtifactGradeVfx_Rare => artifactGradeVfx_Rare;

	public GameObject ArtifactInfo => artifactInfo;

	public GameObject ArtifactInfo_GradeLayout => artifactInfo_GradeLayout;

	public Image ArtifactInfo_GradeLayout_Bg => artifactInfo_GradeLayout_Bg;

	public TextMeshProUGUI ArtifactInfo_GradeLayout_Text => artifactInfo_GradeLayout_Text;

	public Slider ArtifactInfo_SellSlider => artifactInfo_SellSlider;

	public TextMeshProUGUI ArtifactInfo_TextArtifactName => artifactInfo_TextArtifactName;

	public TextMeshProUGUI ArtifactInfo_TextDesc => artifactInfo_TextDesc;

	public Image ImgArtifact => imgArtifact;

	public GameObject TextBubbleComplete => textBubbleComplete;

	public Image TextBubbleComplete_BG => textBubbleComplete_BG;

	public GameObject TextBubbleComplete_GoldText => textBubbleComplete_GoldText;

	public Image TextBubbleComplete_GoldText_Icon => textBubbleComplete_GoldText_Icon;

	public TextMeshProUGUI TextBubbleComplete_GoldText_Value => textBubbleComplete_GoldText_Value;

	public Image TextBubbleComplete_Tail => textBubbleComplete_Tail;

	public TextMeshProUGUI TextBubbleComplete_Text => textBubbleComplete_Text;

	public GameObject TextBubbleEmpty => textBubbleEmpty;

	public Image ImgFloor => imgFloor;

	public Image SlotNum => slotNum;

	public TextMeshProUGUI Text => text;

	public TextMeshProUGUI TextBreak => textBreak;
}
