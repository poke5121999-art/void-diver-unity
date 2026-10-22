using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinExchangePopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject change;

	[SerializeField]
	private Image change_ImgCoinLeft;

	[SerializeField]
	private Image change_ImgCoinRight;

	[SerializeField]
	private Image change_ImgRealCoinLeft;

	[SerializeField]
	private Image change_ImgRealCoinRight;

	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private GameObject quantity;

	[SerializeField]
	private Button quantity_DecreaseButton;

	[SerializeField]
	private Button quantity_IncreaseButton;

	[SerializeField]
	private Slider quantity_Slider;

	[SerializeField]
	private TextMeshProUGUI quantity_Value;

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
	private ToggleGroup toggleGroup;

	[SerializeField]
	private Toggle toggleGroup_ToggleIn;

	[SerializeField]
	private Toggle toggleGroup_ToggleOut;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private GameObject currencyLayout;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Change => change;

	public Image Change_ImgCoinLeft => change_ImgCoinLeft;

	public Image Change_ImgCoinRight => change_ImgCoinRight;

	public Image Change_ImgRealCoinLeft => change_ImgRealCoinLeft;

	public Image Change_ImgRealCoinRight => change_ImgRealCoinRight;

	public TextMeshProUGUI Desc => desc;

	public GameObject Quantity => quantity;

	public Button Quantity_DecreaseButton => quantity_DecreaseButton;

	public Button Quantity_IncreaseButton => quantity_IncreaseButton;

	public Slider Quantity_Slider => quantity_Slider;

	public TextMeshProUGUI Quantity_Value => quantity_Value;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public ToggleGroup ToggleGroup => toggleGroup;

	public Toggle ToggleGroup_ToggleIn => toggleGroup_ToggleIn;

	public Toggle ToggleGroup_ToggleOut => toggleGroup_ToggleOut;

	public GameObject Top => top;

	public GameObject Title => title;

	public GameObject CurrencyLayout => currencyLayout;
}
