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

	public Slider GroggySlider => groggySlider;

	public GameObject GroggySlider_Contents => groggySlider_Contents;

	public Image GroggySlider_BG => groggySlider_BG;

	public Image GroggySlider_Gauge => groggySlider_Gauge;

	public GameObject GroggySlider_Layout => groggySlider_Layout;

	public GameObject GroggySlider_StatusEffectTags => groggySlider_StatusEffectTags;

	public Slider HpSlider => hpSlider;

	public GameObject HpSlider_Contents => hpSlider_Contents;

	public Image HpSlider_BG => hpSlider_BG;

	public Image HpSlider_Gauge => hpSlider_Gauge;

	public GameObject HpSlider_Hp => hpSlider_Hp;

	public GameObject HpSlider_StatusEffectTags => hpSlider_StatusEffectTags;

	public GameObject TextLimit => textLimit;

	public TextMeshProUGUI TextBossName => textBossName;

	public GameObject TextBossName_BossTag => textBossName_BossTag;

	public GameObject TextBossName_MiniBossTag => textBossName_MiniBossTag;

	public MonsterWeakPointGaugePresenter WeakPointGauge => weakPointGauge;
}
