using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignVotePopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject anomaly;

	[SerializeField]
	private TextMeshProUGUI anomaly_Desc;

	[SerializeField]
	private Image anomaly_IconAnomaly;

	[SerializeField]
	private Image line;

	[SerializeField]
	private GameObject reward;

	[SerializeField]
	private GameObject[] reward_Slots;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject difficulty;

	[SerializeField]
	private Image difficulty_Bg;

	[SerializeField]
	private TextMeshProUGUI difficulty_Text;

	[SerializeField]
	private GameObject difficulty_TextHard;

	[SerializeField]
	private GameObject difficulty_TextInsane;

	[SerializeField]
	private GameObject newTag;

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private Image title;

	[SerializeField]
	private GameObject completed;

	[SerializeField]
	private GameObject recommandedLevel;

	[SerializeField]
	private TextMeshProUGUI recommandedLevel_Value;

	[SerializeField]
	private TextMeshProUGUI textRegion;

	[SerializeField]
	private GameObject titleLayout;

	[SerializeField]
	private GameObject titleLayout_Boss;

	[SerializeField]
	private TextMeshProUGUI titleLayout_Chapter;

	[SerializeField]
	private TextMeshProUGUI titleLayout_Name;

	[SerializeField]
	private GameObject vote;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private GameObject agree;

	[SerializeField]
	private Button agree_Button;

	[SerializeField]
	private GameObject disagree;

	[SerializeField]
	private Button disagree_Button;

	[SerializeField]
	private GameObject voting;

	[SerializeField]
	private Image voting_Line;

	[SerializeField]
	private TextMeshProUGUI voting_TextAgree;

	[SerializeField]
	private TextMeshProUGUI voting_TextDisagree;

	[SerializeField]
	private TextMeshProUGUI voting_TextVoting;

	[SerializeField]
	private Image timer;

	[SerializeField]
	private GameObject[] voteStates;

	public GameObject Contents => null;

	public GameObject Info => null;

	public GameObject Body => null;

	public GameObject Anomaly => null;

	public TextMeshProUGUI Anomaly_Desc => null;

	public Image Anomaly_IconAnomaly => null;

	public Image Line => null;

	public GameObject Reward => null;

	public GameObject[] Reward_Slots => null;

	public GameObject Top => null;

	public GameObject Difficulty => null;

	public Image Difficulty_Bg => null;

	public TextMeshProUGUI Difficulty_Text => null;

	public GameObject Difficulty_TextHard => null;

	public GameObject Difficulty_TextInsane => null;

	public GameObject NewTag => null;

	public TextMeshProUGUI Text => null;

	public Image Title => null;

	public GameObject Completed => null;

	public GameObject RecommandedLevel => null;

	public TextMeshProUGUI RecommandedLevel_Value => null;

	public TextMeshProUGUI TextRegion => null;

	public GameObject TitleLayout => null;

	public GameObject TitleLayout_Boss => null;

	public TextMeshProUGUI TitleLayout_Chapter => null;

	public TextMeshProUGUI TitleLayout_Name => null;

	public GameObject Vote => null;

	public GameObject Bottom => null;

	public GameObject Agree => null;

	public Button Agree_Button => null;

	public GameObject Disagree => null;

	public Button Disagree_Button => null;

	public GameObject Voting => null;

	public Image Voting_Line => null;

	public TextMeshProUGUI Voting_TextAgree => null;

	public TextMeshProUGUI Voting_TextDisagree => null;

	public TextMeshProUGUI Voting_TextVoting => null;

	public Image Timer => null;

	public GameObject[] VoteStates => null;
}
