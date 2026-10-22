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

	public Button Background => background;

	public Image Background_Gradient => background_Gradient;

	public Image Background_GrayDimmer => background_GrayDimmer;

	public GameObject Contents => contents;

	public CanvasGroup Pivot => pivot;

	public GameObject[] BackImages => backImages;

	public GameObject Dim => dim;

	public Image Frame => frame;

	public TextMeshProUGUI Message => message;

	public TextMeshProUGUI Name => name;

	public TextMeshProUGUI NamePlayer => namePlayer;

	public GameObject Navi => navi;

	public GameObject PageButton => pageButton;

	public Button PageButton_NextPageButton => pageButton_NextPageButton;

	public Button PageButton_PrevPageButton => pageButton_PrevPageButton;

	public GameObject[] Selections => selections;

	public GameObject[] FrontImages => frontImages;

	public GameObject[] Keys => keys;

	public GameObject Speakers => speakers;

	public GameObject Center => center;

	public Image Center_Portrait => center_Portrait;

	public GameObject Left => left;

	public Image Left_Portrait => left_Portrait;

	public GameObject Right => right;

	public Image Right_Portrait => right_Portrait;

	public GameObject UserInfo => userInfo;

	public GameObject Currency => currency;

	public GameObject Currency_Info => currency_Info;

	public TextMeshProUGUI Currency_Text => currency_Text;

	public GameObject Exp => exp;

	public GameObject Exp_Info => exp_Info;

	public TextMeshProUGUI Exp_Text => exp_Text;
}
