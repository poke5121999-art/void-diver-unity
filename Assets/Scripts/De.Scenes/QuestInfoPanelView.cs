using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfoPanelView : View
{
	[SerializeField]
	private Button backButton;

	[SerializeField]
	private GameObject buttonLayout;

	[SerializeField]
	private Toggle buttonLayout_DisplayToggle;

	[SerializeField]
	private Button buttonLayout_ShowDialogButton;

	[SerializeField]
	private ScrollRect scroll;

	[SerializeField]
	private Image viewport;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject[] conditions;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private GameObject reward;

	[SerializeField]
	private GameObject[] reward_Slots;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_Level;

	[SerializeField]
	private TextMeshProUGUI title_Text;

	[SerializeField]
	private TextMeshProUGUI title_Type;

	public Button BackButton => backButton;

	public GameObject ButtonLayout => buttonLayout;

	public Toggle ButtonLayout_DisplayToggle => buttonLayout_DisplayToggle;

	public Button ButtonLayout_ShowDialogButton => buttonLayout_ShowDialogButton;

	public ScrollRect Scroll => scroll;

	public Image Viewport => viewport;

	public GameObject Content => content;

	public GameObject Info => info;

	public GameObject[] Conditions => conditions;

	public GameObject Layout => layout;

	public TextMeshProUGUI Desc => desc;

	public GameObject Reward => reward;

	public GameObject[] Reward_Slots => reward_Slots;

	public GameObject Title => title;

	public TextMeshProUGUI Title_Level => title_Level;

	public TextMeshProUGUI Title_Text => title_Text;

	public TextMeshProUGUI Title_Type => title_Type;
}
