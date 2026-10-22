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

	public Image Normal => normal;

	public Image Bg => bg;

	public Image Deco => deco;

	public Image DecoLine => decoLine;

	public GameObject DisableFront => disableFront;

	public GameObject DisableFront_Tier1 => disableFront_Tier1;

	public GameObject DisableFront_Tier2 => disableFront_Tier2;

	public GameObject DisableFront_Tier3 => disableFront_Tier3;

	public GameObject DisableFront_Tier4 => disableFront_Tier4;

	public GameObject DisableFront_Tier5 => disableFront_Tier5;

	public GameObject Focused => focused;

	public GameObject Focused_Tier1 => focused_Tier1;

	public GameObject Focused_Tier2 => focused_Tier2;

	public GameObject Focused_Tier3 => focused_Tier3;

	public GameObject Focused_Tier4 => focused_Tier4;

	public GameObject Focused_Tier5 => focused_Tier5;

	public CanvasGroup FocusedBack => focusedBack;

	public GameObject FocusedBack_Tier1 => focusedBack_Tier1;

	public GameObject FocusedBack_Tier2 => focusedBack_Tier2;

	public GameObject FocusedBack_Tier3 => focusedBack_Tier3;

	public GameObject FocusedBack_Tier4 => focusedBack_Tier4;

	public GameObject FocusedBack_Tier5 => focusedBack_Tier5;

	public GameObject Layout => layout;

	public Image Line => line;

	public TextMeshProUGUI TextDesc => textDesc;
}
