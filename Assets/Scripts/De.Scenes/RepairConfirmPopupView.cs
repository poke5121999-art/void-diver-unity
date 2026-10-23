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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public Image Canter => canter;

	public TextMeshProUGUI Canter_DescText => canter_DescText;

	public GameObject Canter_Layout => canter_Layout;

	public GameObject Canter_Durability => canter_Durability;

	public TextMeshProUGUI Canter_Durability_Text => canter_Durability_Text;

	public GameObject Canter_Durability_Valued => canter_Durability_Valued;

	public TextMeshProUGUI Canter_Durability_Valued_After => canter_Durability_Valued_After;

	public TextMeshProUGUI Canter_Durability_Valued_Before => canter_Durability_Valued_Before;

	public GameObject Canter_Durability_Valued_Between => canter_Durability_Valued_Between;

	public GameObject Canter_Price => canter_Price;

	public GameObject Canter_Price_Gold => canter_Price_Gold;

	public TextMeshProUGUI Canter_Price_Gold_Value => canter_Price_Gold_Value;

	public TextMeshProUGUI Canter_Price_Text => canter_Price_Text;

	public GameObject Title => title;

	public TextMeshProUGUI Title_SubText => title_SubText;

	public TextMeshProUGUI Title_Text => title_Text;
}
