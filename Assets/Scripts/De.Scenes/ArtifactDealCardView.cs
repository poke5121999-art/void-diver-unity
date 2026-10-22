using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDealCardView : View
{
	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image bg;

	[SerializeField]
	private Image deco;

	[SerializeField]
	private Image decoLine;

	[SerializeField]
	private GameObject disableFront;

	[SerializeField]
	private GameObject disableFront_Tier1;

	[SerializeField]
	private GameObject disableFront_Tier2;

	[SerializeField]
	private GameObject disableFront_Tier3;

	[SerializeField]
	private GameObject disableFront_Tier4;

	[SerializeField]
	private GameObject disableFront_Tier5;

	[SerializeField]
	private GameObject focused;

	[SerializeField]
	private GameObject focused_Tier1;

	[SerializeField]
	private GameObject focused_Tier2;

	[SerializeField]
	private GameObject focused_Tier3;

	[SerializeField]
	private GameObject focused_Tier4;

	[SerializeField]
	private GameObject focused_Tier5;

	[SerializeField]
	private CanvasGroup focusedBack;

	[SerializeField]
	private GameObject focusedBack_Tier1;

	[SerializeField]
	private GameObject focusedBack_Tier2;

	[SerializeField]
	private GameObject focusedBack_Tier3;

	[SerializeField]
	private GameObject focusedBack_Tier4;

	[SerializeField]
	private GameObject focusedBack_Tier5;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private Image line;

	[SerializeField]
	private TextMeshProUGUI textDesc;

	public Image Normal => null;

	public Image Bg => null;

	public Image Deco => null;

	public Image DecoLine => null;

	public GameObject DisableFront => null;

	public GameObject DisableFront_Tier1 => null;

	public GameObject DisableFront_Tier2 => null;

	public GameObject DisableFront_Tier3 => null;

	public GameObject DisableFront_Tier4 => null;

	public GameObject DisableFront_Tier5 => null;

	public GameObject Focused => null;

	public GameObject Focused_Tier1 => null;

	public GameObject Focused_Tier2 => null;

	public GameObject Focused_Tier3 => null;

	public GameObject Focused_Tier4 => null;

	public GameObject Focused_Tier5 => null;

	public CanvasGroup FocusedBack => null;

	public GameObject FocusedBack_Tier1 => null;

	public GameObject FocusedBack_Tier2 => null;

	public GameObject FocusedBack_Tier3 => null;

	public GameObject FocusedBack_Tier4 => null;

	public GameObject FocusedBack_Tier5 => null;

	public GameObject Layout => null;

	public Image Line => null;

	public TextMeshProUGUI TextDesc => null;
}
