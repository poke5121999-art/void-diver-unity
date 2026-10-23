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

	public GameObject AimLine => aimLine;

	public GameObject[] AimLine_Toggles => aimLine_Toggles;

	public GameObject AimSupport => aimSupport;

	public GameObject[] AimSupport_Toggles => aimSupport_Toggles;

	public GameObject AutoAttack => autoAttack;

	public GameObject[] AutoAttack_Toggles => autoAttack_Toggles;

	public GameObject Language => language;

	public Button Language_Button => language_Button;

	public Image Language_Button_Normal => language_Button_Normal;

	public GameObject Language_Button_Layout => language_Button_Layout;

	public TextMeshProUGUI Language_Button_TextTitle => language_Button_TextTitle;

	public GameObject LockMouse => lockMouse;

	public GameObject[] LockMouse_Toggles => lockMouse_Toggles;

	public GameObject PadSensitivity => padSensitivity;

	public GameObject PadSensitivity_Sensitivity => padSensitivity_Sensitivity;

	public Slider PadSensitivity_Slider => padSensitivity_Slider;

	public TextMeshProUGUI PadSensitivity_Text => padSensitivity_Text;

	public Button ResetButton => resetButton;

	public GameObject RstickAttack => rstickAttack;

	public GameObject[] RstickAttack_Toggles => rstickAttack_Toggles;

	public GameObject Run => run;

	public GameObject[] Run_Toggles => run_Toggles;
}
