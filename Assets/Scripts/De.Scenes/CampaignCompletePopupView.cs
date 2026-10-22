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

	public Button BaseBgDimmer => baseBgDimmer;

	public Button BG => bG;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Reward => reward;

	public Image Reward_ExpClear => reward_ExpClear;

	public GameObject Reward_ExpClear_Group => reward_ExpClear_Group;

	public TextMeshProUGUI Reward_ExpClear_Value => reward_ExpClear_Value;

	public GameObject[] Reward_Slots => reward_Slots;

	public TextMeshProUGUI Title => title;

	public TextMeshProUGUI CategoryText => categoryText;
}
