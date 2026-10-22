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

	public Canvas Canvas => canvas;

	public Button Bg => bg;

	public GameObject Bottom => bottom;

	public Text Version => version;

	public CanvasGroup Contents => contents;

	public GameObject Buttons => buttons;

	public CanvasGroup Option => option;

	public Button Option_Button => option_Button;

	public CanvasGroup Play => play;

	public Button Play_Button => play_Button;

	public GameObject Play_Button_Normal => play_Button_Normal;

	public GameObject Play_Button_BgFx => play_Button_BgFx;

	public Image Play_Button_ImgIcon => play_Button_ImgIcon;

	public TextMeshProUGUI Play_Button_Text => play_Button_Text;

	public CanvasGroup Quit => quit;

	public Button Quit_Button => quit_Button;

	public CanvasGroup SelectSlot => selectSlot;

	public Button SelectSlot_Button => selectSlot_Button;

	public GameObject SelectSlot_Normal => selectSlot_Normal;

	public GameObject SelectSlot_BgFx => selectSlot_BgFx;

	public Image SelectSlot_ImgIcon => selectSlot_ImgIcon;

	public TextMeshProUGUI SelectSlot_Text => selectSlot_Text;

	public GameObject SelectSlot_Player => selectSlot_Player;

	public TextMeshProUGUI SelectSlot_Player_Name => selectSlot_Player_Name;

	public TextMeshProUGUI SelectSlot_Player_SlotNumber => selectSlot_Player_SlotNumber;

	public GameObject Social => social;

	public GameObject Discord => discord;

	public Button Discord_Button => discord_Button;

	public GameObject Feedback => feedback;

	public Button Feedback_Button => feedback_Button;

	public Image LogoNemo => logoNemo;

	public GameObject X => x;

	public Button X_Button => x_Button;

	public GameObject Steam => steam;

	public GameObject Wishlist => wishlist;

	public Button Wishlist_Button => wishlist_Button;

	public Image Dimm => dimm;

	public CanvasGroup StartVideo => startVideo;
}
