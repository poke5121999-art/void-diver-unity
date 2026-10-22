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

	public GameObject CardGroup => cardGroup;

	public GameObject[] Cards => cards;

	public ParadoxTooltipPresenter ParadoxTooltip => paradoxTooltip;

	public InGameSkillTooltipPresenter SkillTooltip => skillTooltip;

	public GameObject Info => info;

	public GameObject InfoGroup => infoGroup;

	public TextMeshProUGUI Difficulty => difficulty;

	public TextMeshProUGUI Publish => publish;

	public TextMeshProUGUI Title => title;

	public GameObject LoungeCode => loungeCode;

	public Button LoungeCode_Button => loungeCode_Button;

	public Image LoungeCode_Line => loungeCode_Line;

	public TextMeshProUGUI LoungeCode_Text => loungeCode_Text;

	public GameObject LoungeCode_Title => loungeCode_Title;

	public Image LoungeCode_Lock => loungeCode_Lock;

	public GameObject LoungeCode_Hide => loungeCode_Hide;

	public Button LoungeCode_Hide_OnOffButton => loungeCode_Hide_OnOffButton;

	public Image LoungeCode_Hide_OnOffButton_Normal => loungeCode_Hide_OnOffButton_Normal;

	public Image LoungeCode_Hide_OnOffButton_Off => loungeCode_Hide_OnOffButton_Off;

	public Image LoungeCode_Hide_OnOffButton_On => loungeCode_Hide_OnOffButton_On;

	public GameObject Participating => participating;

	public RoomCodeDisplayPopupPresenter RoomCodeDisplayPopup => roomCodeDisplayPopup;

	public GameObject Setting => setting;

	public Button Setting_Button => setting_Button;

	public Image Setting_Normal => setting_Normal;

	public Image Setting_Line => setting_Line;

	public TextMeshProUGUI Setting_Text => setting_Text;
}
