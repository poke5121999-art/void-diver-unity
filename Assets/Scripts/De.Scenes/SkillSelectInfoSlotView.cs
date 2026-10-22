using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectInfoSlotView : View
{
	[SerializeField]
	private GameObject modLine;

	[SerializeField]
	private Button slot;

	[SerializeField]
	private TextMeshProUGUI descText;

	[SerializeField]
	private GameObject ignoreGroup;

	[SerializeField]
	private GameObject activeOtherSlot;

	[SerializeField]
	private GameObject condition;

	[SerializeField]
	private GameObject cooltime;

	[SerializeField]
	private Image cooltime_Icon;

	[SerializeField]
	private TextMeshProUGUI cooltime_Text;

	[SerializeField]
	private GameObject hp;

	[SerializeField]
	private Image hp_Icon;

	[SerializeField]
	private TextMeshProUGUI hp_Text;

	[SerializeField]
	private GameObject stress;

	[SerializeField]
	private Image stress_Icon;

	[SerializeField]
	private TextMeshProUGUI stress_Text;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image line;

	[SerializeField]
	private GameObject titleGroup;

	[SerializeField]
	private TextMeshProUGUI titleGroup_ModText;

	[SerializeField]
	private TextMeshProUGUI titleGroup_TitleText;

	public GameObject ModLine => null;

	public Button Slot => null;

	public TextMeshProUGUI DescText => null;

	public GameObject IgnoreGroup => null;

	public GameObject ActiveOtherSlot => null;

	public GameObject Condition => null;

	public GameObject Cooltime => null;

	public Image Cooltime_Icon => null;

	public TextMeshProUGUI Cooltime_Text => null;

	public GameObject Hp => null;

	public Image Hp_Icon => null;

	public TextMeshProUGUI Hp_Text => null;

	public GameObject Stress => null;

	public Image Stress_Icon => null;

	public TextMeshProUGUI Stress_Text => null;

	public Image Icon => null;

	public Image Line => null;

	public GameObject TitleGroup => null;

	public TextMeshProUGUI TitleGroup_ModText => null;

	public TextMeshProUGUI TitleGroup_TitleText => null;
}
