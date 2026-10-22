using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteriorSlotView : View
{
	[SerializeField]
	private Image backLine;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image checkBox;

	[SerializeField]
	private GameObject coin;

	[SerializeField]
	private Image coin_Icon;

	[SerializeField]
	private TextMeshProUGUI coin_Value;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private Image gold_Icon;

	[SerializeField]
	private TextMeshProUGUI gold_Value;

	[SerializeField]
	private Image img;

	[SerializeField]
	private Image line;

	[SerializeField]
	private Image locked;

	[SerializeField]
	private Image locked_IconLock;

	[SerializeField]
	private TextMeshProUGUI locked_Text;

	[SerializeField]
	private TextMeshProUGUI nameText;

	[SerializeField]
	private GameObject newTag;

	[SerializeField]
	private TextMeshProUGUI typeText;

	public Image BackLine => null;

	public Image Normal => null;

	public Image CheckBox => null;

	public GameObject Coin => null;

	public Image Coin_Icon => null;

	public TextMeshProUGUI Coin_Value => null;

	public GameObject Gold => null;

	public Image Gold_Icon => null;

	public TextMeshProUGUI Gold_Value => null;

	public Image Img => null;

	public Image Line => null;

	public Image Locked => null;

	public Image Locked_IconLock => null;

	public TextMeshProUGUI Locked_Text => null;

	public TextMeshProUGUI NameText => null;

	public GameObject NewTag => null;

	public TextMeshProUGUI TypeText => null;
}
