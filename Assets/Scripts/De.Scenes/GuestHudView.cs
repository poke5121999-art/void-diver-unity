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

	public GameObject EmojiRoot => emojiRoot;

	public GameObject HpGauge => hpGauge;

	public GameObject Layout => layout;

	public GameObject Bubble => bubble;

	public Image Bubble_BG => bubble_BG;

	public GameObject Bubble_Tail => bubble_Tail;

	public TextMeshProUGUI Bubble_Text => bubble_Text;

	public TextMeshProUGUI Monologue => monologue;

	public GameObject NpcThumbUp => npcThumbUp;

	public GameObject QuestAvailableMark => questAvailableMark;

	public GameObject QuestCompleteMark => questCompleteMark;

	public TextMeshProUGUI Name => name;

	public TextMeshProUGUI SubName => subName;
}
