using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionGamePlayPanelView : View
{
	[SerializeField]
	private GameObject aimLine;

	[SerializeField]
	private GameObject[] aimLine_Toggles;

	[SerializeField]
	private GameObject aimSupport;

	[SerializeField]
	private GameObject[] aimSupport_Toggles;

	[SerializeField]
	private GameObject autoAttack;

	[SerializeField]
	private GameObject[] autoAttack_Toggles;

	[SerializeField]
	private GameObject language;

	[SerializeField]
	private Button language_Button;

	[SerializeField]
	private Image language_Button_Normal;

	[SerializeField]
	private GameObject language_Button_Layout;

	[SerializeField]
	private TextMeshProUGUI language_Button_TextTitle;

	[SerializeField]
	private GameObject lockMouse;

	[SerializeField]
	private GameObject[] lockMouse_Toggles;

	[SerializeField]
	private GameObject padSensitivity;

	[SerializeField]
	private GameObject padSensitivity_Sensitivity;

	[SerializeField]
	private Slider padSensitivity_Slider;

	[SerializeField]
	private TextMeshProUGUI padSensitivity_Text;

	[SerializeField]
	private Button resetButton;

	[SerializeField]
	private GameObject rstickAttack;

	[SerializeField]
	private GameObject[] rstickAttack_Toggles;

	[SerializeField]
	private GameObject run;

	[SerializeField]
	private GameObject[] run_Toggles;

	public GameObject AimLine => null;

	public GameObject[] AimLine_Toggles => null;

	public GameObject AimSupport => null;

	public GameObject[] AimSupport_Toggles => null;

	public GameObject AutoAttack => null;

	public GameObject[] AutoAttack_Toggles => null;

	public GameObject Language => null;

	public Button Language_Button => null;

	public Image Language_Button_Normal => null;

	public GameObject Language_Button_Layout => null;

	public TextMeshProUGUI Language_Button_TextTitle => null;

	public GameObject LockMouse => null;

	public GameObject[] LockMouse_Toggles => null;

	public GameObject PadSensitivity => null;

	public GameObject PadSensitivity_Sensitivity => null;

	public Slider PadSensitivity_Slider => null;

	public TextMeshProUGUI PadSensitivity_Text => null;

	public Button ResetButton => null;

	public GameObject RstickAttack => null;

	public GameObject[] RstickAttack_Toggles => null;

	public GameObject Run => null;

	public GameObject[] Run_Toggles => null;
}
