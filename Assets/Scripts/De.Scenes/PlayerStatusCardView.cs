using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusCardView : View
{
	[SerializeField]
	private GameObject characterStressLine;

	[SerializeField]
	private Image glow;

	[SerializeField]
	private Image line;

	[SerializeField]
	private Image thin;

	[SerializeField]
	private Image colorBar;

	[SerializeField]
	private Image face;

	[SerializeField]
	private Image face_Image;

	[SerializeField]
	private Image hostIcon;

	[SerializeField]
	private GameObject hp;

	[SerializeField]
	private GameObject hp_Anchor;

	[SerializeField]
	private Image hp_Gauge;

	[SerializeField]
	private Image hp_Gauge_Back;

	[SerializeField]
	private Image hp_Gauge_Front;

	[SerializeField]
	private Image hp_Gauge_Shield;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private GameObject[] rttIcons;

	[SerializeField]
	private TextMeshProUGUI rttText;

	[SerializeField]
	private TextMeshProUGUI steamid;

	public GameObject CharacterStressLine => characterStressLine;

	public Image Glow => glow;

	public Image Line => line;

	public Image Thin => thin;

	public Image ColorBar => colorBar;

	public Image Face => face;

	public Image Face_Image => face_Image;

	public Image HostIcon => hostIcon;

	public GameObject Hp => hp;

	public GameObject Hp_Anchor => hp_Anchor;

	public Image Hp_Gauge => hp_Gauge;

	public Image Hp_Gauge_Back => hp_Gauge_Back;

	public Image Hp_Gauge_Front => hp_Gauge_Front;

	public Image Hp_Gauge_Shield => hp_Gauge_Shield;

	public TextMeshProUGUI Name => name;

	public GameObject[] RttIcons => rttIcons;

	public TextMeshProUGUI RttText => rttText;

	public TextMeshProUGUI Steamid => steamid;
}
