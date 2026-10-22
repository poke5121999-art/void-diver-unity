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

	public GameObject CharacterStressLine => characterStressLine;

	public Image Glow => glow;

	public Image Line => line;

	public GameObject StaminaGauge => staminaGauge;

	public Image StaminaGauge_Fill => staminaGauge_Fill;

	public Image Thin => thin;

	public GameObject DespairBuffAdded => despairBuffAdded;

	public CanvasGroup DespairBuffAdded_Normal => despairBuffAdded_Normal;

	public CanvasGroup DespairBuffAdded_Dotween => despairBuffAdded_Dotween;

	public TextMeshProUGUI DespairBuffAdded_Text => despairBuffAdded_Text;

	public GameObject DespairBuffRemoved => despairBuffRemoved;

	public CanvasGroup DespairBuffRemoved_Normal => despairBuffRemoved_Normal;

	public CanvasGroup DespairBuffRemoved_Dotween => despairBuffRemoved_Dotween;

	public TextMeshProUGUI DespairBuffRemoved_Text => despairBuffRemoved_Text;

	public GameObject EmojiRoot => emojiRoot;

	public GameObject GainParadox => gainParadox;

	public GameObject GainParadox_Normal => gainParadox_Normal;

	public Image GainParadox_Icon => gainParadox_Icon;

	public TextMeshProUGUI GainParadox_Text => gainParadox_Text;

	public Image GainParadox_Line => gainParadox_Line;

	public GameObject HpGauge => hpGauge;

	public GameObject HpGauge_Anchor => hpGauge_Anchor;

	public Image HpGauge_BG => hpGauge_BG;

	public Image HpGauge_Gauge => hpGauge_Gauge;

	public TextMeshProUGUI HpGauge_TMP16B => hpGauge_TMP16B;

	public Image HpGauge_Host => hpGauge_Host;

	public GameObject ItemUseGauge => itemUseGauge;

	public Image ItemUseGauge_BaseGauge_Circle38 => itemUseGauge_BaseGauge_Circle38;

	public Image ItemUseGauge_GaugeFill => itemUseGauge_GaugeFill;

	public Image ItemUseGauge_Image => itemUseGauge_Image;

	public GameObject Layout => layout;

	public GameObject Bubble => bubble;

	public Image Bubble_BG => bubble_BG;

	public GameObject Bubble_Tail => bubble_Tail;

	public TextMeshProUGUI Bubble_Text => bubble_Text;

	public GameObject HudSkillReset => hudSkillReset;

	public CanvasGroup HudSkillReset_Contents => hudSkillReset_Contents;

	public CanvasGroup HudSkillReset_Skill => hudSkillReset_Skill;

	public Image HudSkillReset_Normal => hudSkillReset_Normal;

	public Image HudSkillReset_Icon => hudSkillReset_Icon;

	public GameObject HudSkillReset_KeyGuide => hudSkillReset_KeyGuide;

	public GameObject HudSkillReset_KeyPrompt => hudSkillReset_KeyPrompt;

	public TextMeshProUGUI HudSkillReset_Text => hudSkillReset_Text;

	public TextMeshProUGUI Monologue => monologue;

	public TextMeshProUGUI NameText => nameText;

	public Image NameText_Image => nameText_Image;

	public GameObject OneCoin => oneCoin;

	public CanvasGroup OneCoin_Normal => oneCoin_Normal;

	public CanvasGroup OneCoin_Dotween => oneCoin_Dotween;

	public TextMeshProUGUI OneCoin_Text => oneCoin_Text;

	public Image OneCoin_ImgDeco => oneCoin_ImgDeco;

	public GameObject[] ToastTexts => toastTexts;
}
