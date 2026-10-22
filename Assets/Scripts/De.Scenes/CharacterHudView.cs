using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHudView : View
{
	[SerializeField]
	private GameObject characterStressLine;

	[SerializeField]
	private Image glow;

	[SerializeField]
	private Image line;

	[SerializeField]
	private GameObject staminaGauge;

	[SerializeField]
	private Image staminaGauge_Fill;

	[SerializeField]
	private Image thin;

	[SerializeField]
	private GameObject despairBuffAdded;

	[SerializeField]
	private CanvasGroup despairBuffAdded_Normal;

	[SerializeField]
	private CanvasGroup despairBuffAdded_Dotween;

	[SerializeField]
	private TextMeshProUGUI despairBuffAdded_Text;

	[SerializeField]
	private GameObject despairBuffRemoved;

	[SerializeField]
	private CanvasGroup despairBuffRemoved_Normal;

	[SerializeField]
	private CanvasGroup despairBuffRemoved_Dotween;

	[SerializeField]
	private TextMeshProUGUI despairBuffRemoved_Text;

	[SerializeField]
	private GameObject emojiRoot;

	[SerializeField]
	private GameObject gainParadox;

	[SerializeField]
	private GameObject gainParadox_Normal;

	[SerializeField]
	private Image gainParadox_Icon;

	[SerializeField]
	private TextMeshProUGUI gainParadox_Text;

	[SerializeField]
	private Image gainParadox_Line;

	[SerializeField]
	private GameObject hpGauge;

	[SerializeField]
	private GameObject hpGauge_Anchor;

	[SerializeField]
	private Image hpGauge_BG;

	[SerializeField]
	private Image hpGauge_Gauge;

	[SerializeField]
	private TextMeshProUGUI hpGauge_TMP16B;

	[SerializeField]
	private Image hpGauge_Host;

	[SerializeField]
	private GameObject itemUseGauge;

	[SerializeField]
	private Image itemUseGauge_BaseGauge_Circle38;

	[SerializeField]
	private Image itemUseGauge_GaugeFill;

	[SerializeField]
	private Image itemUseGauge_Image;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private GameObject bubble;

	[SerializeField]
	private Image bubble_BG;

	[SerializeField]
	private GameObject bubble_Tail;

	[SerializeField]
	private TextMeshProUGUI bubble_Text;

	[SerializeField]
	private GameObject hudSkillReset;

	[SerializeField]
	private CanvasGroup hudSkillReset_Contents;

	[SerializeField]
	private CanvasGroup hudSkillReset_Skill;

	[SerializeField]
	private Image hudSkillReset_Normal;

	[SerializeField]
	private Image hudSkillReset_Icon;

	[SerializeField]
	private GameObject hudSkillReset_KeyGuide;

	[SerializeField]
	private GameObject hudSkillReset_KeyPrompt;

	[SerializeField]
	private TextMeshProUGUI hudSkillReset_Text;

	[SerializeField]
	private TextMeshProUGUI monologue;

	[SerializeField]
	private TextMeshProUGUI nameText;

	[SerializeField]
	private Image nameText_Image;

	[SerializeField]
	private GameObject oneCoin;

	[SerializeField]
	private CanvasGroup oneCoin_Normal;

	[SerializeField]
	private CanvasGroup oneCoin_Dotween;

	[SerializeField]
	private TextMeshProUGUI oneCoin_Text;

	[SerializeField]
	private Image oneCoin_ImgDeco;

	[SerializeField]
	private GameObject[] toastTexts;

	public GameObject CharacterStressLine => null;

	public Image Glow => null;

	public Image Line => null;

	public GameObject StaminaGauge => null;

	public Image StaminaGauge_Fill => null;

	public Image Thin => null;

	public GameObject DespairBuffAdded => null;

	public CanvasGroup DespairBuffAdded_Normal => null;

	public CanvasGroup DespairBuffAdded_Dotween => null;

	public TextMeshProUGUI DespairBuffAdded_Text => null;

	public GameObject DespairBuffRemoved => null;

	public CanvasGroup DespairBuffRemoved_Normal => null;

	public CanvasGroup DespairBuffRemoved_Dotween => null;

	public TextMeshProUGUI DespairBuffRemoved_Text => null;

	public GameObject EmojiRoot => null;

	public GameObject GainParadox => null;

	public GameObject GainParadox_Normal => null;

	public Image GainParadox_Icon => null;

	public TextMeshProUGUI GainParadox_Text => null;

	public Image GainParadox_Line => null;

	public GameObject HpGauge => null;

	public GameObject HpGauge_Anchor => null;

	public Image HpGauge_BG => null;

	public Image HpGauge_Gauge => null;

	public TextMeshProUGUI HpGauge_TMP16B => null;

	public Image HpGauge_Host => null;

	public GameObject ItemUseGauge => null;

	public Image ItemUseGauge_BaseGauge_Circle38 => null;

	public Image ItemUseGauge_GaugeFill => null;

	public Image ItemUseGauge_Image => null;

	public GameObject Layout => null;

	public GameObject Bubble => null;

	public Image Bubble_BG => null;

	public GameObject Bubble_Tail => null;

	public TextMeshProUGUI Bubble_Text => null;

	public GameObject HudSkillReset => null;

	public CanvasGroup HudSkillReset_Contents => null;

	public CanvasGroup HudSkillReset_Skill => null;

	public Image HudSkillReset_Normal => null;

	public Image HudSkillReset_Icon => null;

	public GameObject HudSkillReset_KeyGuide => null;

	public GameObject HudSkillReset_KeyPrompt => null;

	public TextMeshProUGUI HudSkillReset_Text => null;

	public TextMeshProUGUI Monologue => null;

	public TextMeshProUGUI NameText => null;

	public Image NameText_Image => null;

	public GameObject OneCoin => null;

	public CanvasGroup OneCoin_Normal => null;

	public CanvasGroup OneCoin_Dotween => null;

	public TextMeshProUGUI OneCoin_Text => null;

	public Image OneCoin_ImgDeco => null;

	public GameObject[] ToastTexts => null;
}
