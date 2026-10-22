using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSoundPanelView : View
{
	[SerializeField]
	private GameObject bgm;

	[SerializeField]
	private GameObject bgm_Volume;

	[SerializeField]
	private Toggle bgm_Volume_OnOffToggle;

	[SerializeField]
	private Slider bgm_Volume_Slider;

	[SerializeField]
	private TextMeshProUGUI bgm_Volume_Text;

	[SerializeField]
	private Button resetButton;

	[SerializeField]
	private GameObject sfx;

	[SerializeField]
	private GameObject sfx_Volume;

	[SerializeField]
	private Toggle sfx_Volume_OnOffToggle;

	[SerializeField]
	private Slider sfx_Volume_Slider;

	[SerializeField]
	private TextMeshProUGUI sfx_Volume_Text;

	[SerializeField]
	private GameObject totalVolume;

	[SerializeField]
	private GameObject totalVolume_Volume;

	[SerializeField]
	private Toggle totalVolume_Volume_OnOffToggle;

	[SerializeField]
	private Slider totalVolume_Volume_Slider;

	[SerializeField]
	private TextMeshProUGUI totalVolume_Volume_Text;

	public GameObject Bgm => null;

	public GameObject Bgm_Volume => null;

	public Toggle Bgm_Volume_OnOffToggle => null;

	public Slider Bgm_Volume_Slider => null;

	public TextMeshProUGUI Bgm_Volume_Text => null;

	public Button ResetButton => null;

	public GameObject Sfx => null;

	public GameObject Sfx_Volume => null;

	public Toggle Sfx_Volume_OnOffToggle => null;

	public Slider Sfx_Volume_Slider => null;

	public TextMeshProUGUI Sfx_Volume_Text => null;

	public GameObject TotalVolume => null;

	public GameObject TotalVolume_Volume => null;

	public Toggle TotalVolume_Volume_OnOffToggle => null;

	public Slider TotalVolume_Volume_Slider => null;

	public TextMeshProUGUI TotalVolume_Volume_Text => null;
}
