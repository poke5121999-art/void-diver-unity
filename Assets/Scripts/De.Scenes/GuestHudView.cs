using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuestHudView : View
{
	[SerializeField]
	private GameObject emojiRoot;

	[SerializeField]
	private GameObject hpGauge;

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
	private TextMeshProUGUI monologue;

	[SerializeField]
	private GameObject npcThumbUp;

	[SerializeField]
	private GameObject questAvailableMark;

	[SerializeField]
	private GameObject questCompleteMark;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private TextMeshProUGUI subName;

	public GameObject EmojiRoot => null;

	public GameObject HpGauge => null;

	public GameObject Layout => null;

	public GameObject Bubble => null;

	public Image Bubble_BG => null;

	public GameObject Bubble_Tail => null;

	public TextMeshProUGUI Bubble_Text => null;

	public TextMeshProUGUI Monologue => null;

	public GameObject NpcThumbUp => null;

	public GameObject QuestAvailableMark => null;

	public GameObject QuestCompleteMark => null;

	public TextMeshProUGUI Name => null;

	public TextMeshProUGUI SubName => null;
}
