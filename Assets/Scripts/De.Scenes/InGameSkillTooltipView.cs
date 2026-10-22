using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameSkillTooltipView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject desc;

	[SerializeField]
	private TextMeshProUGUI desc_Text;

	[SerializeField]
	private GameObject infoGroup;

	[SerializeField]
	private GameObject cooltime;

	[SerializeField]
	private TextMeshProUGUI cooltime_Value;

	[SerializeField]
	private GameObject elemental;

	[SerializeField]
	private TextMeshProUGUI elemental_Value;

	[SerializeField]
	private GameObject hP;

	[SerializeField]
	private TextMeshProUGUI hP_Value;

	[SerializeField]
	private GameObject mP;

	[SerializeField]
	private TextMeshProUGUI mP_Value;

	[SerializeField]
	private GameObject sP;

	[SerializeField]
	private TextMeshProUGUI sP_Value;

	[SerializeField]
	private GameObject stackCooltime;

	[SerializeField]
	private TextMeshProUGUI stackCooltime_Value;

	[SerializeField]
	private GameObject specificityGroup;

	[SerializeField]
	private GameObject[] skills;

	[SerializeField]
	private Image title;

	[SerializeField]
	private TextMeshProUGUI title_Name;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Desc => desc;

	public TextMeshProUGUI Desc_Text => desc_Text;

	public GameObject InfoGroup => infoGroup;

	public GameObject Cooltime => cooltime;

	public TextMeshProUGUI Cooltime_Value => cooltime_Value;

	public GameObject Elemental => elemental;

	public TextMeshProUGUI Elemental_Value => elemental_Value;

	public GameObject HP => hP;

	public TextMeshProUGUI HP_Value => hP_Value;

	public GameObject MP => mP;

	public TextMeshProUGUI MP_Value => mP_Value;

	public GameObject SP => sP;

	public TextMeshProUGUI SP_Value => sP_Value;

	public GameObject StackCooltime => stackCooltime;

	public TextMeshProUGUI StackCooltime_Value => stackCooltime_Value;

	public GameObject SpecificityGroup => specificityGroup;

	public GameObject[] Skills => skills;

	public Image Title => title;

	public TextMeshProUGUI Title_Name => title_Name;
}
