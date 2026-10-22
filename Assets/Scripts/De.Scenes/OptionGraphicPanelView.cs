using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionGraphicPanelView : View
{
	[SerializeField]
	private GameObject cameraShaking;

	[SerializeField]
	private GameObject[] cameraShaking_Toggles;

	[SerializeField]
	private GameObject fPS;

	[SerializeField]
	private GameObject[] fPS_Toggles;

	[SerializeField]
	private GameObject gamma;

	[SerializeField]
	private Button gamma_Button;

	[SerializeField]
	private GameObject graphicsQuality;

	[SerializeField]
	private GameObject[] graphicsQuality_Toggles;

	[SerializeField]
	private Button resetButton;

	[SerializeField]
	private GameObject resolution;

	[SerializeField]
	private Button resolution_Button;

	[SerializeField]
	private Image resolution_Button_Normal;

	[SerializeField]
	private Image resolution_Button_Icon;

	[SerializeField]
	private GameObject resolution_Button_Layout;

	[SerializeField]
	private GameObject resolution_Button_Gold;

	[SerializeField]
	private Image resolution_Button_Gold_Icon;

	[SerializeField]
	private TextMeshProUGUI resolution_Button_Gold_TextValue;

	[SerializeField]
	private TextMeshProUGUI resolution_Button_TextTitle;

	[SerializeField]
	private Image resolution_Button_Line;

	[SerializeField]
	private GameObject screen;

	[SerializeField]
	private GameObject[] screen_Toggles;

	[SerializeField]
	private GameObject vSync;

	[SerializeField]
	private GameObject[] vSync_Toggles;

	public GameObject CameraShaking => null;

	public GameObject[] CameraShaking_Toggles => null;

	public GameObject FPS => null;

	public GameObject[] FPS_Toggles => null;

	public GameObject Gamma => null;

	public Button Gamma_Button => null;

	public GameObject GraphicsQuality => null;

	public GameObject[] GraphicsQuality_Toggles => null;

	public Button ResetButton => null;

	public GameObject Resolution => null;

	public Button Resolution_Button => null;

	public Image Resolution_Button_Normal => null;

	public Image Resolution_Button_Icon => null;

	public GameObject Resolution_Button_Layout => null;

	public GameObject Resolution_Button_Gold => null;

	public Image Resolution_Button_Gold_Icon => null;

	public TextMeshProUGUI Resolution_Button_Gold_TextValue => null;

	public TextMeshProUGUI Resolution_Button_TextTitle => null;

	public Image Resolution_Button_Line => null;

	public GameObject Screen => null;

	public GameObject[] Screen_Toggles => null;

	public GameObject VSync => null;

	public GameObject[] VSync_Toggles => null;
}
