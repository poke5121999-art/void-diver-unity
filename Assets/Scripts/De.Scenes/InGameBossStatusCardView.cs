using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameBossStatusCardView : View
{
	[SerializeField]
	private Slider groggySlider;

	[SerializeField]
	private GameObject groggySlider_Contents;

	[SerializeField]
	private Image groggySlider_BG;

	[SerializeField]
	private Image groggySlider_Gauge;

	[SerializeField]
	private GameObject groggySlider_Layout;

	[SerializeField]
	private GameObject groggySlider_StatusEffectTags;

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
	private GameObject hpSlider_StatusEffectTags;

	[SerializeField]
	private GameObject textLimit;

	[SerializeField]
	private TextMeshProUGUI textBossName;

	[SerializeField]
	private GameObject textBossName_BossTag;

	[SerializeField]
	private GameObject textBossName_MiniBossTag;

	[SerializeField]
	private MonsterWeakPointGaugePresenter weakPointGauge;

	public Slider GroggySlider => null;

	public GameObject GroggySlider_Contents => null;

	public Image GroggySlider_BG => null;

	public Image GroggySlider_Gauge => null;

	public GameObject GroggySlider_Layout => null;

	public GameObject GroggySlider_StatusEffectTags => null;

	public Slider HpSlider => null;

	public GameObject HpSlider_Contents => null;

	public Image HpSlider_BG => null;

	public Image HpSlider_Gauge => null;

	public GameObject HpSlider_Hp => null;

	public GameObject HpSlider_StatusEffectTags => null;

	public GameObject TextLimit => null;

	public TextMeshProUGUI TextBossName => null;

	public GameObject TextBossName_BossTag => null;

	public GameObject TextBossName_MiniBossTag => null;

	public MonsterWeakPointGaugePresenter WeakPointGauge => null;
}
