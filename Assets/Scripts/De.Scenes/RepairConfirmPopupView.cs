using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairConfirmPopupView : View
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
	private Image canter;

	[SerializeField]
	private TextMeshProUGUI canter_DescText;

	[SerializeField]
	private GameObject canter_Layout;

	[SerializeField]
	private GameObject canter_Durability;

	[SerializeField]
	private TextMeshProUGUI canter_Durability_Text;

	[SerializeField]
	private GameObject canter_Durability_Valued;

	[SerializeField]
	private TextMeshProUGUI canter_Durability_Valued_After;

	[SerializeField]
	private TextMeshProUGUI canter_Durability_Valued_Before;

	[SerializeField]
	private GameObject canter_Durability_Valued_Between;

	[SerializeField]
	private GameObject canter_Price;

	[SerializeField]
	private GameObject canter_Price_Gold;

	[SerializeField]
	private TextMeshProUGUI canter_Price_Gold_Value;

	[SerializeField]
	private TextMeshProUGUI canter_Price_Text;

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

	public Image Canter => null;

	public TextMeshProUGUI Canter_DescText => null;

	public GameObject Canter_Layout => null;

	public GameObject Canter_Durability => null;

	public TextMeshProUGUI Canter_Durability_Text => null;

	public GameObject Canter_Durability_Valued => null;

	public TextMeshProUGUI Canter_Durability_Valued_After => null;

	public TextMeshProUGUI Canter_Durability_Valued_Before => null;

	public GameObject Canter_Durability_Valued_Between => null;

	public GameObject Canter_Price => null;

	public GameObject Canter_Price_Gold => null;

	public TextMeshProUGUI Canter_Price_Gold_Value => null;

	public TextMeshProUGUI Canter_Price_Text => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_SubText => null;

	public TextMeshProUGUI Title_Text => null;
}
