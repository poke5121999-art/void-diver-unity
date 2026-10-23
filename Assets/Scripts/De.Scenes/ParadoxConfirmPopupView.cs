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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public TextMeshProUGUI Desc => desc;

	public Image Paradox => paradox;

	public GameObject Paradox_Cost => paradox_Cost;

	public TextMeshProUGUI Paradox_Cost_Caption => paradox_Cost_Caption;

	public GameObject Paradox_Cost_Info => paradox_Cost_Info;

	public Image Paradox_Cost_Icon => paradox_Cost_Icon;

	public TextMeshProUGUI Paradox_Cost_Value => paradox_Cost_Value;

	public Image Paradox_Icon => paradox_Icon;

	public GameObject Paradox_Info => paradox_Info;

	public TextMeshProUGUI Paradox_Level => paradox_Level;

	public TextMeshProUGUI Paradox_Name => paradox_Name;

	public GameObject Title => title;

	public TextMeshProUGUI Title_SubText => title_SubText;

	public TextMeshProUGUI Title_Text => title_Text;
}
