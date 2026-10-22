using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcHudView : View
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
	private new TextMeshProUGUI name;

	[SerializeField]
	private TextMeshProUGUI subName;

	public GameObject HpGauge => hpGauge;

	public GameObject Layout => layout;

	public GameObject Bubble => bubble;

	public Image Bubble_BG => bubble_BG;

	public GameObject Bubble_Tail => bubble_Tail;

	public TextMeshProUGUI Bubble_Text => bubble_Text;

	public GameObject CampaignStartMark => campaignStartMark;

	public GameObject CompleteMark => completeMark;

	public GameObject Info => info;

	public Image Info_BG => info_BG;

	public GameObject Info_Tail => info_Tail;

	public TextMeshProUGUI Info_Text => info_Text;

	public GameObject LoungeStartMark => loungeStartMark;

	public GameObject MissionStartMark => missionStartMark;

	public TextMeshProUGUI Monologue => monologue;

	public GameObject NpcThumbUp => npcThumbUp;

	public GameObject QuestHostWaitOtherClientMark => questHostWaitOtherClientMark;

	public TextMeshProUGUI Name => name;

	public TextMeshProUGUI SubName => subName;
}
