using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneView : View
{
	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private Button bg;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private Text version;

	[SerializeField]
	private CanvasGroup contents;

	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private CanvasGroup option;

	[SerializeField]
	private Button option_Button;

	[SerializeField]
	private CanvasGroup play;

	[SerializeField]
	private Button play_Button;

	[SerializeField]
	private GameObject play_Button_Normal;

	[SerializeField]
	private GameObject play_Button_BgFx;

	[SerializeField]
	private Image play_Button_ImgIcon;

	[SerializeField]
	private TextMeshProUGUI play_Button_Text;

	[SerializeField]
	private CanvasGroup quit;

	[SerializeField]
	private Button quit_Button;

	[SerializeField]
	private CanvasGroup selectSlot;

	[SerializeField]
	private Button selectSlot_Button;

	[SerializeField]
	private GameObject selectSlot_Normal;

	[SerializeField]
	private GameObject selectSlot_BgFx;

	[SerializeField]
	private Image selectSlot_ImgIcon;

	[SerializeField]
	private TextMeshProUGUI selectSlot_Text;

	[SerializeField]
	private GameObject selectSlot_Player;

	[SerializeField]
	private TextMeshProUGUI selectSlot_Player_Name;

	[SerializeField]
	private TextMeshProUGUI selectSlot_Player_SlotNumber;

	[SerializeField]
	private GameObject social;

	[SerializeField]
	private GameObject discord;

	[SerializeField]
	private Button discord_Button;

	[SerializeField]
	private GameObject feedback;

	[SerializeField]
	private Button feedback_Button;

	[SerializeField]
	private Image logoNemo;

	[SerializeField]
	private GameObject x;

	[SerializeField]
	private Button x_Button;

	[SerializeField]
	private GameObject steam;

	[SerializeField]
	private GameObject wishlist;

	[SerializeField]
	private Button wishlist_Button;

	[SerializeField]
	private Image dimm;

	[SerializeField]
	private CanvasGroup startVideo;

	public Canvas Canvas => null;

	public Button Bg => null;

	public GameObject Bottom => null;

	public Text Version => null;

	public CanvasGroup Contents => null;

	public GameObject Buttons => null;

	public CanvasGroup Option => null;

	public Button Option_Button => null;

	public CanvasGroup Play => null;

	public Button Play_Button => null;

	public GameObject Play_Button_Normal => null;

	public GameObject Play_Button_BgFx => null;

	public Image Play_Button_ImgIcon => null;

	public TextMeshProUGUI Play_Button_Text => null;

	public CanvasGroup Quit => null;

	public Button Quit_Button => null;

	public CanvasGroup SelectSlot => null;

	public Button SelectSlot_Button => null;

	public GameObject SelectSlot_Normal => null;

	public GameObject SelectSlot_BgFx => null;

	public Image SelectSlot_ImgIcon => null;

	public TextMeshProUGUI SelectSlot_Text => null;

	public GameObject SelectSlot_Player => null;

	public TextMeshProUGUI SelectSlot_Player_Name => null;

	public TextMeshProUGUI SelectSlot_Player_SlotNumber => null;

	public GameObject Social => null;

	public GameObject Discord => null;

	public Button Discord_Button => null;

	public GameObject Feedback => null;

	public Button Feedback_Button => null;

	public Image LogoNemo => null;

	public GameObject X => null;

	public Button X_Button => null;

	public GameObject Steam => null;

	public GameObject Wishlist => null;

	public Button Wishlist_Button => null;

	public Image Dimm => null;

	public CanvasGroup StartVideo => null;
}
