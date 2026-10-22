using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoungeFilterPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject loungeInfo;

	[SerializeField]
	private GameObject blockOtherArea;

	[SerializeField]
	private Toggle blockOtherArea_Toggle;

	[SerializeField]
	private GameObject loungeDifficulty;

	[SerializeField]
	private GameObject loungeDifficulty_Classic;

	[SerializeField]
	private TextMeshProUGUI loungeDifficulty_Classic_Text;

	[SerializeField]
	private Toggle loungeDifficulty_Classic_Toggle;

	[SerializeField]
	private GameObject loungeDifficulty_Junior;

	[SerializeField]
	private TextMeshProUGUI loungeDifficulty_Junior_Text;

	[SerializeField]
	private Toggle loungeDifficulty_Junior_Toggle;

	[SerializeField]
	private GameObject loungeDifficulty_Story;

	[SerializeField]
	private TextMeshProUGUI loungeDifficulty_Story_Text;

	[SerializeField]
	private Toggle loungeDifficulty_Story_Toggle;

	[SerializeField]
	private GameObject loungeDifficulty_Veteran;

	[SerializeField]
	private TextMeshProUGUI loungeDifficulty_Veteran_Text;

	[SerializeField]
	private Toggle loungeDifficulty_Veteran_Toggle;

	[SerializeField]
	private GameObject minHostLevel;

	[SerializeField]
	private Button minHostLevel_DownButton;

	[SerializeField]
	private TextMeshProUGUI minHostLevel_LevelText;

	[SerializeField]
	private Slider minHostLevel_Slider;

	[SerializeField]
	private Button minHostLevel_UpButton;

	[SerializeField]
	private GameObject ping;

	[SerializeField]
	private GameObject[] ping_Toggles;

	[SerializeField]
	private GameObject showPlayingLounge;

	[SerializeField]
	private Toggle showPlayingLounge_Toggle;

	[SerializeField]
	private GameObject reset;

	[SerializeField]
	private Button reset_Button;

	[SerializeField]
	private GameObject btnGroup;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Button;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject LoungeInfo => null;

	public GameObject BlockOtherArea => null;

	public Toggle BlockOtherArea_Toggle => null;

	public GameObject LoungeDifficulty => null;

	public GameObject LoungeDifficulty_Classic => null;

	public TextMeshProUGUI LoungeDifficulty_Classic_Text => null;

	public Toggle LoungeDifficulty_Classic_Toggle => null;

	public GameObject LoungeDifficulty_Junior => null;

	public TextMeshProUGUI LoungeDifficulty_Junior_Text => null;

	public Toggle LoungeDifficulty_Junior_Toggle => null;

	public GameObject LoungeDifficulty_Story => null;

	public TextMeshProUGUI LoungeDifficulty_Story_Text => null;

	public Toggle LoungeDifficulty_Story_Toggle => null;

	public GameObject LoungeDifficulty_Veteran => null;

	public TextMeshProUGUI LoungeDifficulty_Veteran_Text => null;

	public Toggle LoungeDifficulty_Veteran_Toggle => null;

	public GameObject MinHostLevel => null;

	public Button MinHostLevel_DownButton => null;

	public TextMeshProUGUI MinHostLevel_LevelText => null;

	public Slider MinHostLevel_Slider => null;

	public Button MinHostLevel_UpButton => null;

	public GameObject Ping => null;

	public GameObject[] Ping_Toggles => null;

	public GameObject ShowPlayingLounge => null;

	public Toggle ShowPlayingLounge_Toggle => null;

	public GameObject Reset => null;

	public Button Reset_Button => null;

	public GameObject BtnGroup => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;
}
