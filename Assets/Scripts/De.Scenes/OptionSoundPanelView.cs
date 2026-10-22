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

	public GameObject Bgm => bgm;

	public GameObject Bgm_Volume => bgm_Volume;

	public Toggle Bgm_Volume_OnOffToggle => bgm_Volume_OnOffToggle;

	public Slider Bgm_Volume_Slider => bgm_Volume_Slider;

	public TextMeshProUGUI Bgm_Volume_Text => bgm_Volume_Text;

	public Button ResetButton => resetButton;

	public GameObject Sfx => sfx;

	public GameObject Sfx_Volume => sfx_Volume;

	public Toggle Sfx_Volume_OnOffToggle => sfx_Volume_OnOffToggle;

	public Slider Sfx_Volume_Slider => sfx_Volume_Slider;

	public TextMeshProUGUI Sfx_Volume_Text => sfx_Volume_Text;

	public GameObject TotalVolume => totalVolume;

	public GameObject TotalVolume_Volume => totalVolume_Volume;

	public Toggle TotalVolume_Volume_OnOffToggle => totalVolume_Volume_OnOffToggle;

	public Slider TotalVolume_Volume_Slider => totalVolume_Volume_Slider;

	public TextMeshProUGUI TotalVolume_Volume_Text => totalVolume_Volume_Text;
}
