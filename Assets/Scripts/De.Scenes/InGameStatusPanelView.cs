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

	public BuffTooltipPresenter BuffTooltip => buffTooltip;

	public Image Contents => contents;

	public Slider BrightnessSlider => brightnessSlider;

	public GameObject BrightnessSlider_Contents => brightnessSlider_Contents;

	public Image BrightnessSlider_BG => brightnessSlider_BG;

	public Image BrightnessSlider_Gauge => brightnessSlider_Gauge;

	public Image BrightnessSlider_HeadLight => brightnessSlider_HeadLight;

	public TextMeshProUGUI BrightnessSlider_Text => brightnessSlider_Text;

	public Slider HpSlider => hpSlider;

	public GameObject HpSlider_Contents => hpSlider_Contents;

	public Image HpSlider_BG => hpSlider_BG;

	public Image HpSlider_Gauge => hpSlider_Gauge;

	public GameObject HpSlider_Hp => hpSlider_Hp;

	public TextMeshProUGUI HpSlider_Hp_Max => hpSlider_Hp_Max;

	public GameObject HpSlider_Hp_Shield => hpSlider_Hp_Shield;

	public TextMeshProUGUI HpSlider_Hp_Shield_Value => hpSlider_Hp_Shield_Value;

	public TextMeshProUGUI HpSlider_Hp_Value => hpSlider_Hp_Value;

	public Image HpSlider_LImage => hpSlider_LImage;

	public Image HpSlider_RImage => hpSlider_RImage;

	public GameObject HpSlider_StatusEffectTags => hpSlider_StatusEffectTags;

	public StressLinePresenter StressLine => stressLine;
}
