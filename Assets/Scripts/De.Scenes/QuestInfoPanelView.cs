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

	public Button BackButton => null;

	public GameObject ButtonLayout => null;

	public Toggle ButtonLayout_DisplayToggle => null;

	public Button ButtonLayout_ShowDialogButton => null;

	public ScrollRect Scroll => null;

	public Image Viewport => null;

	public GameObject Content => null;

	public GameObject Info => null;

	public GameObject[] Conditions => null;

	public GameObject Layout => null;

	public TextMeshProUGUI Desc => null;

	public GameObject Reward => null;

	public GameObject[] Reward_Slots => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_Level => null;

	public TextMeshProUGUI Title_Text => null;

	public TextMeshProUGUI Title_Type => null;
}
