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

	public GameObject Contents => contents;

	public GameObject Info => info;

	public GameObject Body => body;

	public GameObject Anomaly => anomaly;

	public TextMeshProUGUI Anomaly_Desc => anomaly_Desc;

	public Image Anomaly_IconAnomaly => anomaly_IconAnomaly;

	public Image Line => line;

	public GameObject Reward => reward;

	public GameObject[] Reward_Slots => reward_Slots;

	public GameObject Top => top;

	public GameObject Difficulty => difficulty;

	public Image Difficulty_Bg => difficulty_Bg;

	public TextMeshProUGUI Difficulty_Text => difficulty_Text;

	public GameObject Difficulty_TextHard => difficulty_TextHard;

	public GameObject Difficulty_TextInsane => difficulty_TextInsane;

	public GameObject NewTag => newTag;

	public TextMeshProUGUI Text => text;

	public Image Title => title;

	public GameObject Completed => completed;

	public GameObject RecommandedLevel => recommandedLevel;

	public TextMeshProUGUI RecommandedLevel_Value => recommandedLevel_Value;

	public TextMeshProUGUI TextRegion => textRegion;

	public GameObject TitleLayout => titleLayout;

	public GameObject TitleLayout_Boss => titleLayout_Boss;

	public TextMeshProUGUI TitleLayout_Chapter => titleLayout_Chapter;

	public TextMeshProUGUI TitleLayout_Name => titleLayout_Name;

	public GameObject Vote => vote;

	public GameObject Bottom => bottom;

	public GameObject Agree => agree;

	public Button Agree_Button => agree_Button;

	public GameObject Disagree => disagree;

	public Button Disagree_Button => disagree_Button;

	public GameObject Voting => voting;

	public Image Voting_Line => voting_Line;

	public TextMeshProUGUI Voting_TextAgree => voting_TextAgree;

	public TextMeshProUGUI Voting_TextDisagree => voting_TextDisagree;

	public TextMeshProUGUI Voting_TextVoting => voting_TextVoting;

	public Image Timer => timer;

	public GameObject[] VoteStates => voteStates;
}
