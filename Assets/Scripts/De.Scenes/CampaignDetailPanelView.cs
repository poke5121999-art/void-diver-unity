using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignDetailPanelView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject dialog;

	[SerializeField]
	private CanvasGroup dialog_Contents;

	[SerializeField]
	private TextMeshProUGUI dialog_Desc;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject info_Body;

	[SerializeField]
	private GameObject info_Anomaly;

	[SerializeField]
	private TextMeshProUGUI info_Anomaly_Description;

	[SerializeField]
	private Image info_Anomaly_Icon;

	[SerializeField]
	private TextMeshProUGUI info_Anomaly_Name;

	[SerializeField]
	private GameObject info_Buttons;

	[SerializeField]
	private Button info_StartCampaignButton;

	[SerializeField]
	private GameObject info_Difficulty;

	[SerializeField]
	private Image info_Difficulty_BgEasy;

	[SerializeField]
	private Image info_Difficulty_BgHard;

	[SerializeField]
	private Image info_Difficulty_BgInsane;

	[SerializeField]
	private Image info_Difficulty_BgNormal;

	[SerializeField]
	private GameObject info_Difficulty_Layout;

	[SerializeField]
	private TextMeshProUGUI info_Difficulty_Layout_TextDesc;

	[SerializeField]
	private TextMeshProUGUI info_Difficulty_Layout_TextHard;

	[SerializeField]
	private TextMeshProUGUI info_Difficulty_Layout_TextInsane;

	[SerializeField]
	private Button info_Difficulty_NextButton;

	[SerializeField]
	private GameObject info_Difficulty_NextButton_Vfx;

	[SerializeField]
	private GameObject info_Difficulty_NextButton_Hard;

	[SerializeField]
	private GameObject info_Difficulty_NextButton_Insane;

	[SerializeField]
	private GameObject info_Difficulty_NextButton_Normal;

	[SerializeField]
	private Button info_Difficulty_PadButton;

	[SerializeField]
	private Button info_Difficulty_PreButton;

	[SerializeField]
	private GameObject info_Difficulty_PreButton_Vfx;

	[SerializeField]
	private GameObject info_Difficulty_PreButton_Easy;

	[SerializeField]
	private GameObject info_Difficulty_PreButton_Hard;

	[SerializeField]
	private GameObject info_Difficulty_PreButton_Normal;

	[SerializeField]
	private TextMeshProUGUI info_Difficulty_Text;

	[SerializeField]
	private GameObject info_FirstReward;

	[SerializeField]
	private GameObject[] info_FirstReward_Slots;

	[SerializeField]
	private TextMeshProUGUI info_FirstReward_TextDesc;

	[SerializeField]
	private GameObject info_Level;

	[SerializeField]
	private TextMeshProUGUI info_Level_Value;

	[SerializeField]
	private GameObject info_Region;

	[SerializeField]
	private TextMeshProUGUI info_Region_TextRegion;

	[SerializeField]
	private GameObject info_Reward;

	[SerializeField]
	private GameObject[] info_Reward_Slots;

	[SerializeField]
	private GameObject info_Top;

	[SerializeField]
	private Image info_NewTag;

	[SerializeField]
	private Image info_Title;

	[SerializeField]
	private GameObject info_Completed;

	[SerializeField]
	private GameObject info_Layout;

	[SerializeField]
	private GameObject info_Layout_Boss;

	[SerializeField]
	private TextMeshProUGUI info_Layout_Chapter;

	[SerializeField]
	private TextMeshProUGUI info_Layout_Name;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject cursor;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Dialog => dialog;

	public CanvasGroup Dialog_Contents => dialog_Contents;

	public TextMeshProUGUI Dialog_Desc => dialog_Desc;

	public GameObject Left => left;

	public GameObject Right => right;

	public GameObject Info => info;

	public GameObject Info_Body => info_Body;

	public GameObject Info_Anomaly => info_Anomaly;

	public TextMeshProUGUI Info_Anomaly_Description => info_Anomaly_Description;

	public Image Info_Anomaly_Icon => info_Anomaly_Icon;

	public TextMeshProUGUI Info_Anomaly_Name => info_Anomaly_Name;

	public GameObject Info_Buttons => info_Buttons;

	public Button Info_StartCampaignButton => info_StartCampaignButton;

	public GameObject Info_Difficulty => info_Difficulty;

	public Image Info_Difficulty_BgEasy => info_Difficulty_BgEasy;

	public Image Info_Difficulty_BgHard => info_Difficulty_BgHard;

	public Image Info_Difficulty_BgInsane => info_Difficulty_BgInsane;

	public Image Info_Difficulty_BgNormal => info_Difficulty_BgNormal;

	public GameObject Info_Difficulty_Layout => info_Difficulty_Layout;

	public TextMeshProUGUI Info_Difficulty_Layout_TextDesc => info_Difficulty_Layout_TextDesc;

	public TextMeshProUGUI Info_Difficulty_Layout_TextHard => info_Difficulty_Layout_TextHard;

	public TextMeshProUGUI Info_Difficulty_Layout_TextInsane => info_Difficulty_Layout_TextInsane;

	public Button Info_Difficulty_NextButton => info_Difficulty_NextButton;

	public GameObject Info_Difficulty_NextButton_Vfx => info_Difficulty_NextButton_Vfx;

	public GameObject Info_Difficulty_NextButton_Hard => info_Difficulty_NextButton_Hard;

	public GameObject Info_Difficulty_NextButton_Insane => info_Difficulty_NextButton_Insane;

	public GameObject Info_Difficulty_NextButton_Normal => info_Difficulty_NextButton_Normal;

	public Button Info_Difficulty_PadButton => info_Difficulty_PadButton;

	public Button Info_Difficulty_PreButton => info_Difficulty_PreButton;

	public GameObject Info_Difficulty_PreButton_Vfx => info_Difficulty_PreButton_Vfx;

	public GameObject Info_Difficulty_PreButton_Easy => info_Difficulty_PreButton_Easy;

	public GameObject Info_Difficulty_PreButton_Hard => info_Difficulty_PreButton_Hard;

	public GameObject Info_Difficulty_PreButton_Normal => info_Difficulty_PreButton_Normal;

	public TextMeshProUGUI Info_Difficulty_Text => info_Difficulty_Text;

	public GameObject Info_FirstReward => info_FirstReward;

	public GameObject[] Info_FirstReward_Slots => info_FirstReward_Slots;

	public TextMeshProUGUI Info_FirstReward_TextDesc => info_FirstReward_TextDesc;

	public GameObject Info_Level => info_Level;

	public TextMeshProUGUI Info_Level_Value => info_Level_Value;

	public GameObject Info_Region => info_Region;

	public TextMeshProUGUI Info_Region_TextRegion => info_Region_TextRegion;

	public GameObject Info_Reward => info_Reward;

	public GameObject[] Info_Reward_Slots => info_Reward_Slots;

	public GameObject Info_Top => info_Top;

	public Image Info_NewTag => info_NewTag;

	public Image Info_Title => info_Title;

	public GameObject Info_Completed => info_Completed;

	public GameObject Info_Layout => info_Layout;

	public GameObject Info_Layout_Boss => info_Layout_Boss;

	public TextMeshProUGUI Info_Layout_Chapter => info_Layout_Chapter;

	public TextMeshProUGUI Info_Layout_Name => info_Layout_Name;

	public GameObject KeyGuide => keyGuide;

	public GameObject Cursor => cursor;
}
