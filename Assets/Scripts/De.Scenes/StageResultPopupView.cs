using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultPopupView : View
{
	[SerializeField]
	private GameObject root;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private ScrollRect scrollView;

	[SerializeField]
	private GameObject viewport;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private CanvasGroup exp;

	[SerializeField]
	private GameObject[] exp_List;

	[SerializeField]
	private CanvasGroup gainedGoods;

	[SerializeField]
	private GameObject[] gainedGoods_List;

	[SerializeField]
	private CanvasGroup lostGoods;

	[SerializeField]
	private GameObject[] lostGoods_List;

	[SerializeField]
	private CanvasGroup paradox;

	[SerializeField]
	private GameObject[] paradox_List;

	[SerializeField]
	private CanvasGroup player;

	[SerializeField]
	private GameObject[] player_List;

	[SerializeField]
	private CanvasGroup quest;

	[SerializeField]
	private GameObject[] quest_List;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private TextMeshProUGUI pressAnyKey;

	[SerializeField]
	private CanvasGroup title;

	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private Button skipButton;

	public GameObject Root => root;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public ScrollRect ScrollView => scrollView;

	public GameObject Viewport => viewport;

	public GameObject Content => content;

	public CanvasGroup Exp => exp;

	public GameObject[] Exp_List => exp_List;

	public CanvasGroup GainedGoods => gainedGoods;

	public GameObject[] GainedGoods_List => gainedGoods_List;

	public CanvasGroup LostGoods => lostGoods;

	public GameObject[] LostGoods_List => lostGoods_List;

	public CanvasGroup Paradox => paradox;

	public GameObject[] Paradox_List => paradox_List;

	public CanvasGroup Player => player;

	public GameObject[] Player_List => player_List;

	public CanvasGroup Quest => quest;

	public GameObject[] Quest_List => quest_List;

	public GameObject Bottom => bottom;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public TextMeshProUGUI PressAnyKey => pressAnyKey;

	public CanvasGroup Title => title;

	public TextMeshProUGUI TitleText => titleText;

	public Button SkipButton => skipButton;
}
