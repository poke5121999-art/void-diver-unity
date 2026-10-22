using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignGiveUpVotePopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject campaign;

	[SerializeField]
	private TextMeshProUGUI campaign_Category;

	[SerializeField]
	private GameObject campaign_Difficulty;

	[SerializeField]
	private TextMeshProUGUI campaign_Difficulty_Value;

	[SerializeField]
	private Image campaign_DifficultyBg;

	[SerializeField]
	private TextMeshProUGUI campaign_Name;

	[SerializeField]
	private GameObject[] voteStates;

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
	private Image voting;

	[SerializeField]
	private Image timer;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Campaign => campaign;

	public TextMeshProUGUI Campaign_Category => campaign_Category;

	public GameObject Campaign_Difficulty => campaign_Difficulty;

	public TextMeshProUGUI Campaign_Difficulty_Value => campaign_Difficulty_Value;

	public Image Campaign_DifficultyBg => campaign_DifficultyBg;

	public TextMeshProUGUI Campaign_Name => campaign_Name;

	public GameObject[] VoteStates => voteStates;

	public GameObject Bottom => bottom;

	public GameObject Agree => agree;

	public Button Agree_Button => agree_Button;

	public GameObject Disagree => disagree;

	public Button Disagree_Button => disagree_Button;

	public Image Voting => voting;

	public Image Timer => timer;
}
