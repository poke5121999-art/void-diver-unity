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

	public GameObject ChargeGauge => null;

	public Image ChargeGauge_Gauge => null;

	public GameObject ClearEffect => null;

	public GameObject Cooltime => null;

	public Image Cooltime_Image => null;

	public TextMeshProUGUI Cooltime_Text => null;

	public GameObject Gauge => null;

	public Image Gauge_Image => null;

	public TextMeshProUGUI Gauge_Text => null;

	public Image HpActivate => null;

	public TextMeshProUGUI HpActivate_Text => null;

	public Image HpDeactivate => null;

	public TextMeshProUGUI HpDeactivate_Text => null;

	public Image LimitStack => null;

	public TextMeshProUGUI LimitStack_Text => null;

	public GameObject PassiveGauge => null;

	public Image PassiveGauge_Gauge => null;

	public Button Slot => null;

	public Image Normal => null;

	public Image Icon => null;

	public Image Line => null;

	public GameObject Locked => null;

	public GameObject Stack => null;

	public TextMeshProUGUI Stack_Text => null;

	public Slider Stack_Gauge => null;

	public Image StressActivate => null;

	public TextMeshProUGUI StressActivate_Text => null;
}
