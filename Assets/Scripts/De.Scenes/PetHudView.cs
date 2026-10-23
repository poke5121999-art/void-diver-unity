using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetHudView : View
{
	[SerializeField]
	private GameObject emojiRoot;

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
	private GameObject exclamationIcon;

	[SerializeField]
	private TextMeshProUGUI monologue;

	[SerializeField]
	private GameObject questionIcon;

	[SerializeField]
	private TextMeshProUGUI nameText;

	public GameObject EmojiRoot => emojiRoot;

	public GameObject Layout => layout;

	public GameObject Bubble => bubble;

	public Image Bubble_BG => bubble_BG;

	public GameObject Bubble_Tail => bubble_Tail;

	public TextMeshProUGUI Bubble_Text => bubble_Text;

	public GameObject ExclamationIcon => exclamationIcon;

	public TextMeshProUGUI Monologue => monologue;

	public GameObject QuestionIcon => questionIcon;

	public TextMeshProUGUI NameText => nameText;
}
