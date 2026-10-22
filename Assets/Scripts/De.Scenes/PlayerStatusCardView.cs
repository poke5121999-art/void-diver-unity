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

	public GameObject CharacterStressLine => null;

	public Image Glow => null;

	public Image Line => null;

	public Image Thin => null;

	public Image ColorBar => null;

	public Image Face => null;

	public Image Face_Image => null;

	public Image HostIcon => null;

	public GameObject Hp => null;

	public GameObject Hp_Anchor => null;

	public Image Hp_Gauge => null;

	public Image Hp_Gauge_Back => null;

	public Image Hp_Gauge_Front => null;

	public Image Hp_Gauge_Shield => null;

	public TextMeshProUGUI Name => null;

	public GameObject[] RttIcons => null;

	public TextMeshProUGUI RttText => null;

	public TextMeshProUGUI Steamid => null;
}
