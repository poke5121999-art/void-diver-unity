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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Desc => null;

	public TextMeshProUGUI Desc_Text => null;

	public GameObject InfoGroup => null;

	public GameObject Cooltime => null;

	public TextMeshProUGUI Cooltime_Value => null;

	public GameObject Elemental => null;

	public TextMeshProUGUI Elemental_Value => null;

	public GameObject HP => null;

	public TextMeshProUGUI HP_Value => null;

	public GameObject MP => null;

	public TextMeshProUGUI MP_Value => null;

	public GameObject SP => null;

	public TextMeshProUGUI SP_Value => null;

	public GameObject StackCooltime => null;

	public TextMeshProUGUI StackCooltime_Value => null;

	public GameObject SpecificityGroup => null;

	public GameObject[] Skills => null;

	public Image Title => null;

	public TextMeshProUGUI Title_Name => null;
}
