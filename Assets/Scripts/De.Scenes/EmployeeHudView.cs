using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeHudView : View
{
	[SerializeField]
	private GameObject hpGauge;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private GameObject bubble;

	[SerializeField]
	private Image bubble_BG;

	[SerializeField]
	private GameObject bubble_Tail;

	[SerializeField]
	private TextMeshProUGUI bubble_Text;

	[SerializeField]
	private GameObject campaignStartMark;

	[SerializeField]
	private GameObject completeMark;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private Image info_BG;

	[SerializeField]
	private GameObject info_Tail;

	[SerializeField]
	private TextMeshProUGUI info_Text;

	[SerializeField]
	private GameObject loungeStartMark;

	[SerializeField]
	private GameObject missionStartMark;

	[SerializeField]
	private TextMeshProUGUI monologue;

	[SerializeField]
	private GameObject npcThumbUp;

	[SerializeField]
	private GameObject questHostWaitOtherClientMark;

	[SerializeField]
	private GameObject tycoonState;

	[SerializeField]
	private Image tycoonState_BG;

	[SerializeField]
	private GameObject tycoonState_Complete;

	[SerializeField]
	private GameObject tycoonState_Panic;

	[SerializeField]
	private Image tycoonState_Tail;

	[SerializeField]
	private GameObject tycoonState_WaitMark;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private TextMeshProUGUI subName;

	public GameObject HpGauge => null;

	public GameObject Layout => null;

	public GameObject Bubble => null;

	public Image Bubble_BG => null;

	public GameObject Bubble_Tail => null;

	public TextMeshProUGUI Bubble_Text => null;

	public GameObject CampaignStartMark => null;

	public GameObject CompleteMark => null;

	public GameObject Info => null;

	public Image Info_BG => null;

	public GameObject Info_Tail => null;

	public TextMeshProUGUI Info_Text => null;

	public GameObject LoungeStartMark => null;

	public GameObject MissionStartMark => null;

	public TextMeshProUGUI Monologue => null;

	public GameObject NpcThumbUp => null;

	public GameObject QuestHostWaitOtherClientMark => null;

	public GameObject TycoonState => null;

	public Image TycoonState_BG => null;

	public GameObject TycoonState_Complete => null;

	public GameObject TycoonState_Panic => null;

	public Image TycoonState_Tail => null;

	public GameObject TycoonState_WaitMark => null;

	public TextMeshProUGUI Name => null;

	public TextMeshProUGUI SubName => null;
}
