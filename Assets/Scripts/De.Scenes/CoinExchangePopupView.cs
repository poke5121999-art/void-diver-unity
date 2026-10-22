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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Change => null;

	public Image Change_ImgCoinLeft => null;

	public Image Change_ImgCoinRight => null;

	public Image Change_ImgRealCoinLeft => null;

	public Image Change_ImgRealCoinRight => null;

	public TextMeshProUGUI Desc => null;

	public GameObject Quantity => null;

	public Button Quantity_DecreaseButton => null;

	public Button Quantity_IncreaseButton => null;

	public Slider Quantity_Slider => null;

	public TextMeshProUGUI Quantity_Value => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public ToggleGroup ToggleGroup => null;

	public Toggle ToggleGroup_ToggleIn => null;

	public Toggle ToggleGroup_ToggleOut => null;

	public GameObject Top => null;

	public GameObject Title => null;

	public GameObject CurrencyLayout => null;
}
