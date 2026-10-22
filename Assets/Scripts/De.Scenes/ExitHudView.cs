using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExitHudView : View
{
	[SerializeField]
	private GameObject anchor;

	[SerializeField]
	private Slider escapeBuffStackGauge;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private Image bg;

	[SerializeField]
	private GameObject coin;

	[SerializeField]
	private Image coin_Img;

	[SerializeField]
	private TextMeshProUGUI textCount;

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private TextMeshProUGUI timerText;

	public GameObject Anchor => null;

	public Slider EscapeBuffStackGauge => null;

	public GameObject Layout => null;

	public Image Bg => null;

	public GameObject Coin => null;

	public Image Coin_Img => null;

	public TextMeshProUGUI TextCount => null;

	public TextMeshProUGUI Text => null;

	public TextMeshProUGUI TimerText => null;
}
