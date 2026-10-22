using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoDisplaySlotView : View
{
	[SerializeField]
	private Image arrow;

	[SerializeField]
	private GameObject slot;

	[SerializeField]
	private TextMeshProUGUI descText;

	[SerializeField]
	private GameObject ignoreGroup;

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
	private GameObject titleGroup;

	[SerializeField]
	private TextMeshProUGUI titleGroup_ModText;

	[SerializeField]
	private TextMeshProUGUI titleGroup_TitleText;

	public Image Arrow => arrow;

	public GameObject Slot => slot;

	public TextMeshProUGUI DescText => descText;

	public GameObject IgnoreGroup => ignoreGroup;

	public GameObject Condition => condition;

	public GameObject Cooltime => cooltime;

	public Image Cooltime_Icon => cooltime_Icon;

	public TextMeshProUGUI Cooltime_Text => cooltime_Text;

	public GameObject Hp => hp;

	public Image Hp_Icon => hp_Icon;

	public TextMeshProUGUI Hp_Text => hp_Text;

	public GameObject Stress => stress;

	public Image Stress_Icon => stress_Icon;

	public TextMeshProUGUI Stress_Text => stress_Text;

	public Image Icon => icon;

	public GameObject TitleGroup => titleGroup;

	public TextMeshProUGUI TitleGroup_ModText => titleGroup_ModText;

	public TextMeshProUGUI TitleGroup_TitleText => titleGroup_TitleText;
}
