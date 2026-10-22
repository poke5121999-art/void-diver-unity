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

	public GameObject Root => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public ScrollRect ScrollView => null;

	public GameObject Viewport => null;

	public GameObject Content => null;

	public CanvasGroup Exp => null;

	public GameObject[] Exp_List => null;

	public CanvasGroup GainedGoods => null;

	public GameObject[] GainedGoods_List => null;

	public CanvasGroup LostGoods => null;

	public GameObject[] LostGoods_List => null;

	public CanvasGroup Paradox => null;

	public GameObject[] Paradox_List => null;

	public CanvasGroup Player => null;

	public GameObject[] Player_List => null;

	public CanvasGroup Quest => null;

	public GameObject[] Quest_List => null;

	public GameObject Bottom => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public TextMeshProUGUI PressAnyKey => null;

	public CanvasGroup Title => null;

	public TextMeshProUGUI TitleText => null;

	public Button SkipButton => null;
}
