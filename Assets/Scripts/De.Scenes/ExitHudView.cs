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

	public GameObject Anchor => anchor;

	public Slider EscapeBuffStackGauge => escapeBuffStackGauge;

	public GameObject Layout => layout;

	public Image Bg => bg;

	public GameObject Coin => coin;

	public Image Coin_Img => coin_Img;

	public TextMeshProUGUI TextCount => textCount;

	public TextMeshProUGUI Text => text;

	public TextMeshProUGUI TimerText => timerText;
}
