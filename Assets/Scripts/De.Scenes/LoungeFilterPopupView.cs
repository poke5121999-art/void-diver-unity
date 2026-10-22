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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject LoungeInfo => loungeInfo;

	public GameObject BlockOtherArea => blockOtherArea;

	public Toggle BlockOtherArea_Toggle => blockOtherArea_Toggle;

	public GameObject LoungeDifficulty => loungeDifficulty;

	public GameObject LoungeDifficulty_Classic => loungeDifficulty_Classic;

	public TextMeshProUGUI LoungeDifficulty_Classic_Text => loungeDifficulty_Classic_Text;

	public Toggle LoungeDifficulty_Classic_Toggle => loungeDifficulty_Classic_Toggle;

	public GameObject LoungeDifficulty_Junior => loungeDifficulty_Junior;

	public TextMeshProUGUI LoungeDifficulty_Junior_Text => loungeDifficulty_Junior_Text;

	public Toggle LoungeDifficulty_Junior_Toggle => loungeDifficulty_Junior_Toggle;

	public GameObject LoungeDifficulty_Story => loungeDifficulty_Story;

	public TextMeshProUGUI LoungeDifficulty_Story_Text => loungeDifficulty_Story_Text;

	public Toggle LoungeDifficulty_Story_Toggle => loungeDifficulty_Story_Toggle;

	public GameObject LoungeDifficulty_Veteran => loungeDifficulty_Veteran;

	public TextMeshProUGUI LoungeDifficulty_Veteran_Text => loungeDifficulty_Veteran_Text;

	public Toggle LoungeDifficulty_Veteran_Toggle => loungeDifficulty_Veteran_Toggle;

	public GameObject MinHostLevel => minHostLevel;

	public Button MinHostLevel_DownButton => minHostLevel_DownButton;

	public TextMeshProUGUI MinHostLevel_LevelText => minHostLevel_LevelText;

	public Slider MinHostLevel_Slider => minHostLevel_Slider;

	public Button MinHostLevel_UpButton => minHostLevel_UpButton;

	public GameObject Ping => ping;

	public GameObject[] Ping_Toggles => ping_Toggles;

	public GameObject ShowPlayingLounge => showPlayingLounge;

	public Toggle ShowPlayingLounge_Toggle => showPlayingLounge_Toggle;

	public GameObject Reset => reset;

	public Button Reset_Button => reset_Button;

	public GameObject BtnGroup => btnGroup;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;
}
