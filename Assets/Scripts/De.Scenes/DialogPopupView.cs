using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private Image background_Gradient;

	[SerializeField]
	private Image background_GrayDimmer;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private CanvasGroup pivot;

	[SerializeField]
	private GameObject[] backImages;

	[SerializeField]
	private GameObject dim;

	[SerializeField]
	private Image frame;

	[SerializeField]
	private TextMeshProUGUI message;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private TextMeshProUGUI namePlayer;

	[SerializeField]
	private GameObject navi;

	[SerializeField]
	private GameObject pageButton;

	[SerializeField]
	private Button pageButton_NextPageButton;

	[SerializeField]
	private Button pageButton_PrevPageButton;

	[SerializeField]
	private GameObject[] selections;

	[SerializeField]
	private GameObject[] frontImages;

	[SerializeField]
	private GameObject[] keys;

	[SerializeField]
	private GameObject speakers;

	[SerializeField]
	private GameObject center;

	[SerializeField]
	private Image center_Portrait;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private Image left_Portrait;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private Image right_Portrait;

	[SerializeField]
	private GameObject userInfo;

	[SerializeField]
	private GameObject currency;

	[SerializeField]
	private GameObject currency_Info;

	[SerializeField]
	private TextMeshProUGUI currency_Text;

	[SerializeField]
	private GameObject exp;

	[SerializeField]
	private GameObject exp_Info;

	[SerializeField]
	private TextMeshProUGUI exp_Text;

	public Button Background => null;

	public Image Background_Gradient => null;

	public Image Background_GrayDimmer => null;

	public GameObject Contents => null;

	public CanvasGroup Pivot => null;

	public GameObject[] BackImages => null;

	public GameObject Dim => null;

	public Image Frame => null;

	public TextMeshProUGUI Message => null;

	public TextMeshProUGUI Name => null;

	public TextMeshProUGUI NamePlayer => null;

	public GameObject Navi => null;

	public GameObject PageButton => null;

	public Button PageButton_NextPageButton => null;

	public Button PageButton_PrevPageButton => null;

	public GameObject[] Selections => null;

	public GameObject[] FrontImages => null;

	public GameObject[] Keys => null;

	public GameObject Speakers => null;

	public GameObject Center => null;

	public Image Center_Portrait => null;

	public GameObject Left => null;

	public Image Left_Portrait => null;

	public GameObject Right => null;

	public Image Right_Portrait => null;

	public GameObject UserInfo => null;

	public GameObject Currency => null;

	public GameObject Currency_Info => null;

	public TextMeshProUGUI Currency_Text => null;

	public GameObject Exp => null;

	public GameObject Exp_Info => null;

	public TextMeshProUGUI Exp_Text => null;
}
