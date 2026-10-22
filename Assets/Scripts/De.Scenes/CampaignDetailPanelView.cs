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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Dialog => null;

	public CanvasGroup Dialog_Contents => null;

	public TextMeshProUGUI Dialog_Desc => null;

	public GameObject Left => null;

	public GameObject Right => null;

	public GameObject Info => null;

	public GameObject Info_Body => null;

	public GameObject Info_Anomaly => null;

	public TextMeshProUGUI Info_Anomaly_Description => null;

	public Image Info_Anomaly_Icon => null;

	public TextMeshProUGUI Info_Anomaly_Name => null;

	public GameObject Info_Buttons => null;

	public Button Info_StartCampaignButton => null;

	public GameObject Info_Difficulty => null;

	public Image Info_Difficulty_BgEasy => null;

	public Image Info_Difficulty_BgHard => null;

	public Image Info_Difficulty_BgInsane => null;

	public Image Info_Difficulty_BgNormal => null;

	public GameObject Info_Difficulty_Layout => null;

	public TextMeshProUGUI Info_Difficulty_Layout_TextDesc => null;

	public TextMeshProUGUI Info_Difficulty_Layout_TextHard => null;

	public TextMeshProUGUI Info_Difficulty_Layout_TextInsane => null;

	public Button Info_Difficulty_NextButton => null;

	public GameObject Info_Difficulty_NextButton_Vfx => null;

	public GameObject Info_Difficulty_NextButton_Hard => null;

	public GameObject Info_Difficulty_NextButton_Insane => null;

	public GameObject Info_Difficulty_NextButton_Normal => null;

	public Button Info_Difficulty_PadButton => null;

	public Button Info_Difficulty_PreButton => null;

	public GameObject Info_Difficulty_PreButton_Vfx => null;

	public GameObject Info_Difficulty_PreButton_Easy => null;

	public GameObject Info_Difficulty_PreButton_Hard => null;

	public GameObject Info_Difficulty_PreButton_Normal => null;

	public TextMeshProUGUI Info_Difficulty_Text => null;

	public GameObject Info_FirstReward => null;

	public GameObject[] Info_FirstReward_Slots => null;

	public TextMeshProUGUI Info_FirstReward_TextDesc => null;

	public GameObject Info_Level => null;

	public TextMeshProUGUI Info_Level_Value => null;

	public GameObject Info_Region => null;

	public TextMeshProUGUI Info_Region_TextRegion => null;

	public GameObject Info_Reward => null;

	public GameObject[] Info_Reward_Slots => null;

	public GameObject Info_Top => null;

	public Image Info_NewTag => null;

	public Image Info_Title => null;

	public GameObject Info_Completed => null;

	public GameObject Info_Layout => null;

	public GameObject Info_Layout_Boss => null;

	public TextMeshProUGUI Info_Layout_Chapter => null;

	public TextMeshProUGUI Info_Layout_Name => null;

	public GameObject KeyGuide => null;

	public GameObject Cursor => null;
}
