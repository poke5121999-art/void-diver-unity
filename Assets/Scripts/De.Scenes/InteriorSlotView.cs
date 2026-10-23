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

	public Image BackLine => backLine;

	public Image Normal => normal;

	public Image CheckBox => checkBox;

	public GameObject Coin => coin;

	public Image Coin_Icon => coin_Icon;

	public TextMeshProUGUI Coin_Value => coin_Value;

	public GameObject Gold => gold;

	public Image Gold_Icon => gold_Icon;

	public TextMeshProUGUI Gold_Value => gold_Value;

	public Image Img => img;

	public Image Line => line;

	public Image Locked => locked;

	public Image Locked_IconLock => locked_IconLock;

	public TextMeshProUGUI Locked_Text => locked_Text;

	public TextMeshProUGUI NameText => nameText;

	public GameObject NewTag => newTag;

	public TextMeshProUGUI TypeText => typeText;
}
