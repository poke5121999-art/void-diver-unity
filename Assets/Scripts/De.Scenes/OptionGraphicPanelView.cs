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

	public GameObject CameraShaking => cameraShaking;

	public GameObject[] CameraShaking_Toggles => cameraShaking_Toggles;

	public GameObject FPS => fPS;

	public GameObject[] FPS_Toggles => fPS_Toggles;

	public GameObject Gamma => gamma;

	public Button Gamma_Button => gamma_Button;

	public GameObject GraphicsQuality => graphicsQuality;

	public GameObject[] GraphicsQuality_Toggles => graphicsQuality_Toggles;

	public Button ResetButton => resetButton;

	public GameObject Resolution => resolution;

	public Button Resolution_Button => resolution_Button;

	public Image Resolution_Button_Normal => resolution_Button_Normal;

	public Image Resolution_Button_Icon => resolution_Button_Icon;

	public GameObject Resolution_Button_Layout => resolution_Button_Layout;

	public GameObject Resolution_Button_Gold => resolution_Button_Gold;

	public Image Resolution_Button_Gold_Icon => resolution_Button_Gold_Icon;

	public TextMeshProUGUI Resolution_Button_Gold_TextValue => resolution_Button_Gold_TextValue;

	public TextMeshProUGUI Resolution_Button_TextTitle => resolution_Button_TextTitle;

	public Image Resolution_Button_Line => resolution_Button_Line;

	public GameObject Screen => screen;

	public GameObject[] Screen_Toggles => screen_Toggles;

	public GameObject VSync => vSync;

	public GameObject[] VSync_Toggles => vSync_Toggles;
}
