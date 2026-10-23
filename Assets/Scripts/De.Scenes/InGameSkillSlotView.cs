using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameSkillSlotView : View
{
	[SerializeField]
	private GameObject chargeGauge;

	[SerializeField]
	private Image chargeGauge_Gauge;

	[SerializeField]
	private GameObject clearEffect;

	[SerializeField]
	private GameObject cooltime;

	[SerializeField]
	private Image cooltime_Image;

	[SerializeField]
	private TextMeshProUGUI cooltime_Text;

	[SerializeField]
	private GameObject gauge;

	[SerializeField]
	private Image gauge_Image;

	[SerializeField]
	private TextMeshProUGUI gauge_Text;

	[SerializeField]
	private Image hpActivate;

	[SerializeField]
	private TextMeshProUGUI hpActivate_Text;

	[SerializeField]
	private Image hpDeactivate;

	[SerializeField]
	private TextMeshProUGUI hpDeactivate_Text;

	[SerializeField]
	private Image limitStack;

	[SerializeField]
	private TextMeshProUGUI limitStack_Text;

	[SerializeField]
	private GameObject passiveGauge;

	[SerializeField]
	private Image passiveGauge_Gauge;

	[SerializeField]
	private Button slot;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image line;

	[SerializeField]
	private GameObject locked;

	[SerializeField]
	private GameObject stack;

	[SerializeField]
	private TextMeshProUGUI stack_Text;

	[SerializeField]
	private Slider stack_Gauge;

	[SerializeField]
	private Image stressActivate;

	[SerializeField]
	private TextMeshProUGUI stressActivate_Text;

	public GameObject ChargeGauge => chargeGauge;

	public Image ChargeGauge_Gauge => chargeGauge_Gauge;

	public GameObject ClearEffect => clearEffect;

	public GameObject Cooltime => cooltime;

	public Image Cooltime_Image => cooltime_Image;

	public TextMeshProUGUI Cooltime_Text => cooltime_Text;

	public GameObject Gauge => gauge;

	public Image Gauge_Image => gauge_Image;

	public TextMeshProUGUI Gauge_Text => gauge_Text;

	public Image HpActivate => hpActivate;

	public TextMeshProUGUI HpActivate_Text => hpActivate_Text;

	public Image HpDeactivate => hpDeactivate;

	public TextMeshProUGUI HpDeactivate_Text => hpDeactivate_Text;

	public Image LimitStack => limitStack;

	public TextMeshProUGUI LimitStack_Text => limitStack_Text;

	public GameObject PassiveGauge => passiveGauge;

	public Image PassiveGauge_Gauge => passiveGauge_Gauge;

	public Button Slot => slot;

	public Image Normal => normal;

	public Image Icon => icon;

	public Image Line => line;

	public GameObject Locked => locked;

	public GameObject Stack => stack;

	public TextMeshProUGUI Stack_Text => stack_Text;

	public Slider Stack_Gauge => stack_Gauge;

	public Image StressActivate => stressActivate;

	public TextMeshProUGUI StressActivate_Text => stressActivate_Text;
}
