using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxConfirmPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Button;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private Image paradox;

	[SerializeField]
	private GameObject paradox_Cost;

	[SerializeField]
	private TextMeshProUGUI paradox_Cost_Caption;

	[SerializeField]
	private GameObject paradox_Cost_Info;

	[SerializeField]
	private Image paradox_Cost_Icon;

	[SerializeField]
	private TextMeshProUGUI paradox_Cost_Value;

	[SerializeField]
	private Image paradox_Icon;

	[SerializeField]
	private GameObject paradox_Info;

	[SerializeField]
	private TextMeshProUGUI paradox_Level;

	[SerializeField]
	private TextMeshProUGUI paradox_Name;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_SubText;

	[SerializeField]
	private TextMeshProUGUI title_Text;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public TextMeshProUGUI Desc => null;

	public Image Paradox => null;

	public GameObject Paradox_Cost => null;

	public TextMeshProUGUI Paradox_Cost_Caption => null;

	public GameObject Paradox_Cost_Info => null;

	public Image Paradox_Cost_Icon => null;

	public TextMeshProUGUI Paradox_Cost_Value => null;

	public Image Paradox_Icon => null;

	public GameObject Paradox_Info => null;

	public TextMeshProUGUI Paradox_Level => null;

	public TextMeshProUGUI Paradox_Name => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_SubText => null;

	public TextMeshProUGUI Title_Text => null;
}
