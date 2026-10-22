using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadPageJoinedPanelView : View
{
	[SerializeField]
	private GameObject cardGroup;

	[SerializeField]
	private GameObject[] cards;

	[SerializeField]
	private ParadoxTooltipPresenter paradoxTooltip;

	[SerializeField]
	private InGameSkillTooltipPresenter skillTooltip;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject infoGroup;

	[SerializeField]
	private TextMeshProUGUI difficulty;

	[SerializeField]
	private TextMeshProUGUI publish;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private GameObject loungeCode;

	[SerializeField]
	private Button loungeCode_Button;

	[SerializeField]
	private Image loungeCode_Line;

	[SerializeField]
	private TextMeshProUGUI loungeCode_Text;

	[SerializeField]
	private GameObject loungeCode_Title;

	[SerializeField]
	private Image loungeCode_Lock;

	[SerializeField]
	private GameObject loungeCode_Hide;

	[SerializeField]
	private Button loungeCode_Hide_OnOffButton;

	[SerializeField]
	private Image loungeCode_Hide_OnOffButton_Normal;

	[SerializeField]
	private Image loungeCode_Hide_OnOffButton_Off;

	[SerializeField]
	private Image loungeCode_Hide_OnOffButton_On;

	[SerializeField]
	private GameObject participating;

	[SerializeField]
	private RoomCodeDisplayPopupPresenter roomCodeDisplayPopup;

	[SerializeField]
	private GameObject setting;

	[SerializeField]
	private Button setting_Button;

	[SerializeField]
	private Image setting_Normal;

	[SerializeField]
	private Image setting_Line;

	[SerializeField]
	private TextMeshProUGUI setting_Text;

	public GameObject CardGroup => null;

	public GameObject[] Cards => null;

	public ParadoxTooltipPresenter ParadoxTooltip => null;

	public InGameSkillTooltipPresenter SkillTooltip => null;

	public GameObject Info => null;

	public GameObject InfoGroup => null;

	public TextMeshProUGUI Difficulty => null;

	public TextMeshProUGUI Publish => null;

	public TextMeshProUGUI Title => null;

	public GameObject LoungeCode => null;

	public Button LoungeCode_Button => null;

	public Image LoungeCode_Line => null;

	public TextMeshProUGUI LoungeCode_Text => null;

	public GameObject LoungeCode_Title => null;

	public Image LoungeCode_Lock => null;

	public GameObject LoungeCode_Hide => null;

	public Button LoungeCode_Hide_OnOffButton => null;

	public Image LoungeCode_Hide_OnOffButton_Normal => null;

	public Image LoungeCode_Hide_OnOffButton_Off => null;

	public Image LoungeCode_Hide_OnOffButton_On => null;

	public GameObject Participating => null;

	public RoomCodeDisplayPopupPresenter RoomCodeDisplayPopup => null;

	public GameObject Setting => null;

	public Button Setting_Button => null;

	public Image Setting_Normal => null;

	public Image Setting_Line => null;

	public TextMeshProUGUI Setting_Text => null;
}
