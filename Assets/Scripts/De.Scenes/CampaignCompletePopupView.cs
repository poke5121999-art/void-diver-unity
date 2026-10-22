using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignCompletePopupView : View
{
	[SerializeField]
	private Button baseBgDimmer;

	[SerializeField]
	private Button bG;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject reward;

	[SerializeField]
	private Image reward_ExpClear;

	[SerializeField]
	private GameObject reward_ExpClear_Group;

	[SerializeField]
	private TextMeshProUGUI reward_ExpClear_Value;

	[SerializeField]
	private GameObject[] reward_Slots;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private TextMeshProUGUI categoryText;

	public Button BaseBgDimmer => null;

	public Button BG => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Reward => null;

	public Image Reward_ExpClear => null;

	public GameObject Reward_ExpClear_Group => null;

	public TextMeshProUGUI Reward_ExpClear_Value => null;

	public GameObject[] Reward_Slots => null;

	public TextMeshProUGUI Title => null;

	public TextMeshProUGUI CategoryText => null;
}
