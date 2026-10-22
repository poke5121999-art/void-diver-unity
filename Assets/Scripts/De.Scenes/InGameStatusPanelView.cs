using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameStatusPanelView : View
{
	[SerializeField]
	private BuffTooltipPresenter buffTooltip;

	[SerializeField]
	private Image contents;

	[SerializeField]
	private Slider brightnessSlider;

	[SerializeField]
	private GameObject brightnessSlider_Contents;

	[SerializeField]
	private Image brightnessSlider_BG;

	[SerializeField]
	private Image brightnessSlider_Gauge;

	[SerializeField]
	private Image brightnessSlider_HeadLight;

	[SerializeField]
	private TextMeshProUGUI brightnessSlider_Text;

	[SerializeField]
	private Slider hpSlider;

	[SerializeField]
	private GameObject hpSlider_Contents;

	[SerializeField]
	private Image hpSlider_BG;

	[SerializeField]
	private Image hpSlider_Gauge;

	[SerializeField]
	private GameObject hpSlider_Hp;

	[SerializeField]
	private TextMeshProUGUI hpSlider_Hp_Max;

	[SerializeField]
	private GameObject hpSlider_Hp_Shield;

	[SerializeField]
	private TextMeshProUGUI hpSlider_Hp_Shield_Value;

	[SerializeField]
	private TextMeshProUGUI hpSlider_Hp_Value;

	[SerializeField]
	private Image hpSlider_LImage;

	[SerializeField]
	private Image hpSlider_RImage;

	[SerializeField]
	private GameObject hpSlider_StatusEffectTags;

	[SerializeField]
	private StressLinePresenter stressLine;

	public BuffTooltipPresenter BuffTooltip => null;

	public Image Contents => null;

	public Slider BrightnessSlider => null;

	public GameObject BrightnessSlider_Contents => null;

	public Image BrightnessSlider_BG => null;

	public Image BrightnessSlider_Gauge => null;

	public Image BrightnessSlider_HeadLight => null;

	public TextMeshProUGUI BrightnessSlider_Text => null;

	public Slider HpSlider => null;

	public GameObject HpSlider_Contents => null;

	public Image HpSlider_BG => null;

	public Image HpSlider_Gauge => null;

	public GameObject HpSlider_Hp => null;

	public TextMeshProUGUI HpSlider_Hp_Max => null;

	public GameObject HpSlider_Hp_Shield => null;

	public TextMeshProUGUI HpSlider_Hp_Shield_Value => null;

	public TextMeshProUGUI HpSlider_Hp_Value => null;

	public Image HpSlider_LImage => null;

	public Image HpSlider_RImage => null;

	public GameObject HpSlider_StatusEffectTags => null;

	public StressLinePresenter StressLine => null;
}
