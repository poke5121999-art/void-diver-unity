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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Campaign => null;

	public TextMeshProUGUI Campaign_Category => null;

	public GameObject Campaign_Difficulty => null;

	public TextMeshProUGUI Campaign_Difficulty_Value => null;

	public Image Campaign_DifficultyBg => null;

	public TextMeshProUGUI Campaign_Name => null;

	public GameObject[] VoteStates => null;

	public GameObject Bottom => null;

	public GameObject Agree => null;

	public Button Agree_Button => null;

	public GameObject Disagree => null;

	public Button Disagree_Button => null;

	public Image Voting => null;

	public Image Timer => null;
}
