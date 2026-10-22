using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossMonsterHudView : View
{
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
	private GameObject exclamationIcon;

	[SerializeField]
	private TextMeshProUGUI monologue;

	[SerializeField]
	private GameObject questionIcon;

	[SerializeField]
	private TextMeshProUGUI nameText;

	public GameObject HpGauge => null;

	public GameObject Layout => null;

	public GameObject Bubble => null;

	public Image Bubble_BG => null;

	public GameObject Bubble_Tail => null;

	public TextMeshProUGUI Bubble_Text => null;

	public GameObject ExclamationIcon => null;

	public TextMeshProUGUI Monologue => null;

	public GameObject QuestionIcon => null;

	public TextMeshProUGUI NameText => null;
}
